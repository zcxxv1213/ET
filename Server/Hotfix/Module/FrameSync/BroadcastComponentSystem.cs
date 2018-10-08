using ETHotfix;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETModel
{
    [ObjectSystem]
    public class BroadcastComponentSystem : AwakeSystem<BroadcastComponent>
    {
        public override void Awake(BroadcastComponent self)
        {
            self.Awake();
        }
    }
    public static class BroadcastComponentEx
    {
        public static void Awake(this BroadcastComponent self)
        {
            EventDispatcher.Instance.AddEventListener<List<Unit>, C2SCoalesceInput>(EventConstant.SEND_OR_COALESCE_INPUT, Broadcast);
        }
        private static void Broadcast(List<Unit> units, C2SCoalesceInput message)
        {
            MessageHelper.Broadcast(units, message);
        }
    }
}
