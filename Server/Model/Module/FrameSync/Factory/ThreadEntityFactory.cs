using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public static class ThreadEntityFactory
    {
        public static ThreadEntity Create()
        {
            ThreadComponent threadComponent = Game.Scene.GetComponent<ThreadComponent>();
            ThreadEntity threadEntity = ComponentFactory.Create<ThreadEntity>();
            threadComponent.Add(threadEntity);
            return threadEntity;
        }
    }
}
