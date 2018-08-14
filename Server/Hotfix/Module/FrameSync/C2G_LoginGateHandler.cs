﻿using System;
using ETModel;

namespace ETHotfix
{
	[MessageHandler(AppType.Gate)]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override void Run(Session session, C2G_LoginGate message, Action<G2C_LoginGate> reply)
		{
            Log.Info(message.RpcId.ToString());
            G2C_LoginGate response = new G2C_LoginGate();
			try
			{
				string account = Game.Scene.GetComponent<GateSessionKeyComponent>().Get(message.Key);
				if (account == null)
				{
					response.Error = ErrorCode.ERR_ConnectGateKeyError;
					response.Message = "Gate key验证失败!";
					reply(response);
					return;
				}
				Player player = ComponentFactory.Create<Player, string>(account);
                //UnitID 从数据库得到
                player.Id = 10000L + Game.Scene.GetComponent<PlayerComponent>().Count;
                Game.Scene.GetComponent<PlayerComponent>().Add(player);
				session.AddComponent<SessionPlayerComponent>().Player = player;
				session.AddComponent<MailBoxComponent, string>(ActorType.GateSession);
                response.BaseInfo = new Player_Info_Base() { NickName = player.Account, PlayerId = player.Id };

                response.PlayerId = player.Id;
				reply(response);

				//session.Send(new G2C_TestHotfixMessage() { Info = "recv hotfix message success" });
			}
			catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}