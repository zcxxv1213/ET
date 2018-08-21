﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public class WorldManagerComponent: Component
    {
        Dictionary<long, WorldEntity> mWorldDic = new Dictionary<long, WorldEntity>();
        List<WorldEntity> mEntityList = new List<WorldEntity>();
        public void AddWorld(WorldEntity entity)
        {
            mEntityList.Add(entity);
        }

        public void RemoveWorld()
        {
            //RemoveUnit - > Remove World
        }

        public void AddUnitToWorld(Unit u,WorldEntity world)
        {
            mWorldDic.Add(u.Id, world);
        }
        public bool CheckUnitInWorld(Unit u)
        {
            WorldEntity entity;
            this.mWorldDic.TryGetValue(u.Id, out entity);
            if (entity!=null)
                return true;
            return false;
        }
    }
}
