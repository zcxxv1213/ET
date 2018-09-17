using ETModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix.Handler
{
    [ActorMessageHandler(AppType.Map)]
    public class M2C_ReadyStartGame_Handle : AMActorRpcHandler<Unit, C2M_ReadyStartGame,M2C_ReadyStartGame>
    {
        protected override async Task Run(Unit unit, C2M_ReadyStartGame message, Action<M2C_ReadyStartGame> reply)
        {
            await Task.CompletedTask;
            WorldEntity w = Game.Scene.GetComponent<WorldManagerComponent>().GetWorldByUnit(unit);
            if (w!=null)
            {

            }
            reply(new M2C_ReadyStartGame() { Message = "StartGame" });
        }
    }
}
