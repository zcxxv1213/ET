using ETModel;
using RollBack.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class C2SActorInputStateMessageHandler : AMActorHandler<Unit, C2SOnlyInputState>
    {
        protected async override Task Run(Unit entity, C2SOnlyInputState message)
        {
            //WorldEntity w = Game.Scene.GetComponent<WorldManagerComponent>().GetWorldByUnit(entity);
            //w.GetComponent<RollBack.RollbackDriver>()
            entity.AddInputStateWithFrame((InputState)message.Inputstate);
            // await Task.CompletedTask;
        }
    }
}
