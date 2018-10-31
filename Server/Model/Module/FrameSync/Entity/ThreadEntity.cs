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
        WorldEntity mWorldEntity;
        ThreadFrameComponent mFrameComponent;
        public void Awake()
        {
            Log.Info("AddThreadComponent");
            mFrameComponent = this.AddComponent<ThreadFrameComponent>();
            mFrameComponent.InitComponent(this);
        }

        public void InitWorldEntity(WorldEntity w)
        {
            mWorldEntity = w;
            mFrameComponent.SetWorldEntity(w);
        }

        //TODO移除组件
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }
            ThreadComponent threadComponent = Game.Scene.GetComponent<ThreadComponent>();
            threadComponent.Remove(this.Id);
            base.Dispose();
        }
    }
}
