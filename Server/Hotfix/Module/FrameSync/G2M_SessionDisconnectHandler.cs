using System;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
	[ActorMessageHandler(AppType.Map)]
	public class G2M_SessionDisconnectHandler : AMActorHandler<Unit, G2M_SessionDisconnect>
	{
		protected override async Task Run(Unit unit, G2M_SessionDisconnect message)
		{
            Log.Info("Unit:" + unit.mPlayerID + "ReceiveDisconnectMessage");
			unit.GetComponent<UnitGateComponent>().IsDisconnect = true;
            var worldManager = Game.Scene.GetComponent<WorldManagerComponent>();
            if (worldManager.CheckUnitInWorld(unit))
            {
                var worldEntity = worldManager.GetWorldByUnit(unit);
                worldEntity.RemoveUnitFromWorld(unit);
                if (worldEntity.GetGameState() != null)
                {
                    worldEntity.GetGameState().RemoveGameUnit(unit);
                }
                worldManager.RemoveUnit(unit);

            }
            Game.Scene.GetComponent<UnitComponent>().Remove(unit.Id);
            // unit.Dispose();
            await Task.CompletedTask;
		}
	}
}