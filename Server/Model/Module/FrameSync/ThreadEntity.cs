using ETModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class ThreadEntityAwakeSystem : AwakeSystem<ThreadEntity>
    {
        public override void Awake(ThreadEntity self)
        {
            self.Awake();
        }
    }

    public class ThreadEntity : Entity
    {
        ThreadFrameComponent mFrameComponent;
        public void Awake()
        {
            Log.Info("AddThreadComponent");
            mFrameComponent = this.AddComponent<ThreadFrameComponent>();
            mFrameComponent.InitComponent();

        }
        //TODO移除组件
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
