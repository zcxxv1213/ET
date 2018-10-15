using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public static class WorldEntityFactory
    {
        public static WorldEntity Create()
        {
            WorldManagerComponent worldManagerComponent = Game.Scene.GetComponent<WorldManagerComponent>();
            WorldEntity worldEntity = ComponentFactory.Create<WorldEntity>();
            worldManagerComponent.AddWorld(worldEntity);
            return worldEntity;
        }
    }
}
