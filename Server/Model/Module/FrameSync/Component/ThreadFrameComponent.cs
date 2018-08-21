using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ETModel
{
    public class ThreadFrameComponent : Component
    {
        WorldEntity mWorldEntity;
        public static int s_intervalTime = 100; //单位毫微秒
        public int FrameRate = 32;
        public int InfluenceResolution = 2;
        public long DeltaTime = 0;
        public float DeltaTimeF = 0;
        public int FrameCount;
        public bool mStartGame = false;
        private long _playRate = FixedMath.One;
        private int InfluenceCount;
        private long mStartTime;
        Thread t;
        public int PauseCount { get; private set; }
        public bool IsPaused { get { return PauseCount > 0; } }
        public long PlayRate
        {
            get
            {
                return _playRate;
            }
            set
            {
                if (value != _playRate)
                {
                    _playRate = value;
                    //Time.fixedDeltaTime = BaseDeltaTime / _playRate.ToFloat();
                }
            }
        }
        public void InitComponent()
        {
            FrameCount = 0;
            DeltaTime = FixedMath.One / FrameRate;
            Log.Info(DeltaTime.ToString());
            DeltaTimeF =(DeltaTime / FixedMath.OneF);

            Log.Info(DeltaTimeF.ToString());
            PlayRate = FixedMath.One;
            InfluenceCount = 0;
            PauseCount = 0;
            t = new Thread(UpdateLogic);
            t.Start();
        }
        public void SetWorldEntity(WorldEntity w)
        {
            mWorldEntity = w;
        }
        private void UpdateLogic()
        {
            int time = ServiceTime.GetServiceTime();
            int lastTime = ServiceTime.GetServiceTime();
            while (true)
            {
                if (this.IsDisposed)
                    break;
                lastTime = ServiceTime.GetServiceTime();

          //      UpdateWorld(DeltaTimeF);

                time = ServiceTime.GetServiceTime();

                int sleepTime = s_intervalTime - (time - lastTime);
                if (sleepTime > 0)
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
          
        }
    }
}
