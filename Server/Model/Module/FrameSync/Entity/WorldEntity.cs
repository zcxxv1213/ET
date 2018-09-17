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
        List<Unit> mUnitList = new List<Unit>();
        public void Awake()
        {
            this.AddComponent<WorldManagerComponent>();
        }
        public void AddUnit(Unit u)
        {
            mUnitList.Add(u);
        }
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
