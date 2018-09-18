﻿using RollBack;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel {
    [ObjectSystem]
    public class WorldEntityAwakeSystem : AwakeSystem<WorldEntity>
    {
        public override void Awake(WorldEntity self)
        {
            self.Awake();
        }
    }
    public class WorldEntity : Entity
    {
        GameState mGameState = null;
        List<Unit> mUnitList = new List<Unit>();
        Dictionary<int, Unit> mInputIndexDic = new Dictionary<int, Unit>();
        int maxInputCount = 4;
        public void Awake()
        {
            this.AddComponent<WorldManagerComponent>();
            this.AddComponent<RollbackDriver>();
        }

        public void SetGameState(GameState g)
        {
            mGameState = g;
            //TODO 修改
            this.GetComponent<RollbackDriver>().InitialDriver(g,null,4);
        }

        public void AddUnit(Unit u)
        {
            mUnitList.Add(u);
            this.AddUnitWithInputIndex(u);
        }

        private void AddUnitWithInputIndex(Unit mUnit)
        {
            Unit U;
            for (int i = 1; i <= maxInputCount; i++)
            {
                if (mInputIndexDic.TryGetValue(i, out U) == false)
                {
                    mInputIndexDic[i] = mUnit;
                    mUnit.SetPlayerInputIndex(i);
                    mUnit.mInputAssignment = AssignInput();
                }
            }
        }

        #region Input Assignment

        InputAssignment assignedInputs;

        bool IsFull { get { return assignedInputs == InputAssignment.Full; } }

        InputAssignment AssignInput()
        {
            InputAssignment next = assignedInputs.GetNextAssignment();
            if (next == 0)
                Log.Error("没有下一个InputAssign");
            assignedInputs |= next;
            return next;
        }
        //TODO RELEASE
        void ReleaseInputAssignment(InputAssignment assignment)
        {
            assignedInputs &= ~assignment;
        }

        #endregion

        public bool ReadyForUpdate()
        {
            if (mUnitList.Count > 0)
            {
                foreach (var v in mUnitList)
                {
                    if (v.ReadyForUpdate == false)
                        return false;
                }
                return true;
            }
            return false;
        }
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();
        }
    }
}
