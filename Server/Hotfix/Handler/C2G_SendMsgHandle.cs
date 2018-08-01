using ETModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETHotfix.Handler
{
    [MessageHandler(AppType.Gate)]
    class C2G_SendMsgHandle : AMHandler<C2G_SendMsg>
    {
        protected override void Run(Session session, C2G_SendMsg message)
        {
            Log.Warning("成功接收信息 : " + message.Info); 
        }
    }
}
