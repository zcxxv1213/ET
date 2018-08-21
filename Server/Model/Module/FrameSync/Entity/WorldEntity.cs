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
        public void Awake()
        {
            this.AddComponent<WorldManagerComponent>();
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
