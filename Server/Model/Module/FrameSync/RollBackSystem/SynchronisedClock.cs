﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollBack
{
    class SynchronisedClock
    {
        public SynchronisedClock(PacketTimeTracker packetTimeTracker)
        {
            this.packetTimeTracker = packetTimeTracker;
            this.CurrentFrame = (int)Math.Round(packetTimeTracker.DesiredCurrentFrame);
        }

        PacketTimeTracker packetTimeTracker;

        public int CurrentFrame { get; private set; }
        public double CurrentFrameContinuious { get { return (frameTimeAccumulator * RollbackDriver.FramesPerSecond) + CurrentFrame; } }



        #region Timer Drift Correction and Panic Level

        /// <summary>Correction factor for incoming timesteps to try to move closer into sync with the server</summary>
        public double TimerCorrectionRate { get; private set; }

        /// <summary>Value indicating how long the current frame has existed outside of the "safe" range (as determined by incoming packets).</summary>
        /// <remarks>
        /// 0 is operating entirely inside the safe range.
        /// 0 to 1 is to disregard temporary excursion outside the safe range.
        /// 1 to 2 is for applying significant time correct to try and get the current frame under control.
        /// </remarks>
        public double PanicLevel { get; private set; }



        void UpdatePanicLevelAndCorrectionRate(double elapsedTime)
        {
            // This is what the current time will be if we updated without any drift correction
            double nominalCurrentFrameContinuious = CurrentFrameContinuious + (elapsedTime * RollbackDriver.FramesPerSecond);
            // Compare that to the desired frame number to figure out what direction to drift in
            double desiredCurrentFrameContinuious = packetTimeTracker.DesiredCurrentFrame;

            // This is how far ahead/behind the desired time we will be (with an update at an uncorrected rate)
            double offset = nominalCurrentFrameContinuious - desiredCurrentFrameContinuious;


            #region Panic Level Calculation

            // If we're outside the safe area, start panicing
            bool doPanic = (nominalCurrentFrameContinuious < packetTimeTracker.SafeFrameRangeBegin || nominalCurrentFrameContinuious > packetTimeTracker.SafeFrameRangeEnd);

            // Panic cools down faster than it heats up
            PanicLevel += elapsedTime * (doPanic ? 1 : -2);

            // Clamp
            if (PanicLevel < 0)
            {
                PanicLevel = 0;
            }
            else if (PanicLevel > 2.0)
            {
                PanicLevel = 2.0;
            }

            #endregion


            #region Full Panic Reset

            // When we start panicing, if we're over the given number of frames away from the 
            // target time, perform a full panic reset of the game time
            //
            // (It's better to immediately panic-reset, rather than trying to drift-correct first and
            // failing at it, or be drift-correcting for a long period of time. Get it over with.)

            if (PanicLevel > 1 && Math.Abs(offset) > 30)
            {
                CurrentFrame = (int)Math.Round(desiredCurrentFrameContinuious);
            }

            #endregion


            #region Drift Correction Handling

            // Determine correction rates. Correct harder if we panic.
            const double normalMinCorrectionRate = 1.001;
            const double normalMaxCorrectionRate = 1.02;
            const double panicMinCorrectionRate = 1.02;
            const double panicMaxCorrectionRate = 1.5;

            const double correctRampUpRange = 2;

            const double correctionEpsilon = 0.05; // frames
            if (offset < -correctionEpsilon || offset > correctionEpsilon)
            {
                // Ramp up correction rate as we panic
                double panicAmount = Math.Max(1, Math.Min(PanicLevel, 2)) - 1.0; // value from 0 to 1
                double minCorrectionRate = normalMinCorrectionRate + panicAmount * (panicMinCorrectionRate - normalMinCorrectionRate);
                double maxCorrectionRate = normalMaxCorrectionRate + panicAmount * (panicMaxCorrectionRate - normalMaxCorrectionRate);

                // Ramp up correction rate as we get further away from the desired time
                double correctionAmount = Math.Min(Math.Abs(offset) - correctionEpsilon, correctRampUpRange) / correctRampUpRange;
                double correctionRate = minCorrectionRate + correctionAmount * (maxCorrectionRate - minCorrectionRate);

                if (offset > 0) // If we're ahead, slow down instead of speeding up
                    correctionRate = 1.0 / correctionRate;

                TimerCorrectionRate = correctionRate;
            }
            else
            {
                TimerCorrectionRate = 1;
            }

            #endregion

        }

        #endregion



        /// <summary>Accumulated time between frames, in seconds</summary>
        double frameTimeAccumulator;


        public void Update(double elapsedTime)
        {
            // IMPORTANT: This assumes that the PacketTimeTracker gets updated first!

            UpdatePanicLevelAndCorrectionRate(elapsedTime);

            // Now advance time, taking into account the correction rate
            frameTimeAccumulator += elapsedTime * TimerCorrectionRate;
            while (frameTimeAccumulator >= RollbackDriver.FrameTime.TotalSeconds)
            {
                frameTimeAccumulator -= RollbackDriver.FrameTime.TotalSeconds;
                CurrentFrame++;
            }
        }



    }
}
