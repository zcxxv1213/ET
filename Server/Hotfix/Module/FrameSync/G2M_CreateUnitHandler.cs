using System;
using ETModel;
using Google.Protobuf;

namespace ETHotfix
{
	[MessageHandler(AppType.Map)]
	public class G2M_CreateUnitHandler : AMRpcHandler<G2M_CreateUnit, M2G_CreateUnit>
	{
        protected override async void Run(Session session, G2M_CreateUnit message, Action<M2G_CreateUnit> reply)
        {
            M2G_CreateUnit response = new M2G_CreateUnit();
            try
            {
                Unit unit = ComponentFactory.Create<Unit, UnitType, Team>(UnitType.Hero, Team.Blue);
                unit.AddComponent<MoveComponent>();
                unit.AddComponent<FrameMoveComponent>();

                WorldEntity worldEntity = ComponentFactory.Create<WorldEntity>();
                await unit.AddComponent<MailBoxComponent>().AddLocation();
                unit.AddComponent<UnitGateComponent, long>(message.GateSessionId);
                unit.mPlayerID = message.PlayerId;
                Game.Scene.GetComponent<UnitComponent>().Add(unit);
                Game.Scene.GetComponent<WorldManagerComponent>().AddWorld(worldEntity);
                Game.Scene.GetComponent<WorldManagerComponent>().AddUnitToWorld(unit, worldEntity);

               // worldEntity.GetComponent<WorldManagerComponent>().AddWorld(worldEntity);
              //  worldEntity.GetComponent<WorldManagerComponent>().AddUnitToWorld(unit, worldEntity);
                response.UnitId = unit.Id;
                response.Count = Game.Scene.GetComponent<UnitComponent>().Count;
                reply(response);
                //TODO 人数达到两人-》Creat;
                ThreadEntity threadEntity = ComponentFactory.Create<ThreadEntity>();
                threadEntity.InitWorldEntity(worldEntity);
                Game.Scene.GetComponent<ThreadComponent>().Add(threadEntity);
                /*if (response.Count == 2)
				{
					Actor_CreateUnits actorCreateUnits = new Actor_CreateUnits();
					Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
					foreach (Unit u in units)
					{
						actorCreateUnits.Units.Add(new UnitInfo() {UnitId = u.Id, X = (int)(u.Position.X * 1000), Z = (int)(u.Position.Z * 1000) });
					}
					MessageHelper.Broadcast(actorCreateUnits);
				}*/
                Actor_CreateUnits actorCreateUnits = new Actor_CreateUnits();
                Unit[] units = Game.Scene.GetComponent<UnitComponent>().GetAll();
                GameState g = new GameState();
                worldEntity.SetGameState(g);
                foreach (Unit u in units)
                {
                    g.AddGameUnit(u);
                    actorCreateUnits.Units.Add(new UnitInfo() { UnitId = u.Id, X = (int)(u.Position.X * 1000), Z = (int)(u.Position.Z * 1000), PlayerId = u.mPlayerID });
                }
                MessageHelper.Broadcast(actorCreateUnits);
                //Test
                UnitSnapshotMsg msg = new UnitSnapshotMsg();
                msg.Units.Add(new UnitSnatshot { Id = unit.Id, MoveComponentBytes = new MoveInfo() { PosX = unit.GetComponent<FrameMoveComponent>().moveData.posX, PosY = unit.GetComponent<FrameMoveComponent>().moveData.posY }, PlayerIndex = unit.mPlayerIndex, Info = new PeerInfo() {
                    PlayerName = message.PlayerId.ToString(), InputAssignment = (int)unit.mInputAssignment
                } });
                msg.Frame = worldEntity.GetComponent<RollBack.RollbackDriver>().CurrentFrame;
                Log.Info("CurrentFrame = " + msg.Frame);
                MessageHelper.Broadcast(msg);
            }
            catch (Exception e)
			{
				ReplyError(response, e, reply);
			}
		}
	}
}