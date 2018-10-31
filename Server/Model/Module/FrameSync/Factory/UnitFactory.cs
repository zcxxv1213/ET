using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    public static class UnitFactory
    {
        public static Unit Create(long id, string name, UnitType type,Team team)
        {
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            Unit unit = ComponentFactory.CreateWithId<Unit, UnitType, Team>(id,type, team);
            unit.name = name;
         //   unit.AddComponent<MoveComponent>();
            unit.AddComponent<FrameMoveComponent>();
            unitComponent.Add(unit);
            return unit;
        }
    }
}
