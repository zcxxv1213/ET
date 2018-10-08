using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ETModel;

namespace ETHotfix
{
    [ActorMessageHandler(AppType.Map)]
    public class PlayerInputHandle : AMActorHandler<Unit, C2SCoalesceInput>
    {
        protected async override Task Run(Unit entity, C2SCoalesceInput message)
        {
            await Task.CompletedTask;
            entity.QueueMessage(message);
        }
    }
}
