using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class SessionPlayerComponentDestroySystem : DestroySystem<SessionPlayerComponent>
	{
        //对SessionPlayerComponent的Destory方法进行重写，在SessionDispose 的时候，执行Entity的Dispose方法
        //这里是对SessionPlayerComponent的Destory方法同一扩展方法。
        public override void Destroy(SessionPlayerComponent self)
		{
            // 发送断线消息
            //不等于0的时候才进入地图
            if (self.Player.UnitId != 0)
            {
                ActorMessageSender actorMessageSender = Game.Scene.GetComponent<ActorMessageSenderComponent>().Get(self.Player.UnitId);
                actorMessageSender.Send(new G2M_SessionDisconnect());
                Game.Scene.GetComponent<PlayerComponent>()?.Remove(self.Player.Id);
            }
			
		}
	}
}