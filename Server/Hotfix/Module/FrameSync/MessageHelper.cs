using ETModel;
using System.Collections.Generic;

namespace ETHotfix
{
	public static class MessageHelper
	{
		public static void Broadcast(IActorMessage message)
		{
			Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
			ActorMessageSenderComponent actorMessageSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
			foreach (Unit unit in units)
			{
				UnitGateComponent unitGateComponent = unit.GetComponent<UnitGateComponent>();
				if (unitGateComponent.IsDisconnect)
				{
					continue;
				}
				actorMessageSenderComponent.GetWithActorId(unitGateComponent.GateSessionActorId).Send(message);
			}
		}
        public static void Broadcast(List<Unit> units,IActorMessage message)
        {
            ActorMessageSenderComponent actorMessageSenderComponent = Game.Scene.GetComponent<ActorMessageSenderComponent>();
            foreach (var v in units)
            {
                UnitGateComponent unitGateComponent = v.GetComponent<UnitGateComponent>();
                if (unitGateComponent.IsDisconnect)
                {
                    continue;
                }
                actorMessageSenderComponent.GetWithActorId(unitGateComponent.GateSessionActorId).Send(message);
            }
        }
    }
}
