using System.Numerics;
using MongoDB.Bson.Serialization.Attributes;
using RollBack;
using RollBack.Input;
using System;
using System.Collections.Generic;

namespace ETModel
{
	public enum UnitType
	{
		Hero,
		Npc
	}
    public enum Team
    {
        Red,
        Blue
    }
	[ObjectSystem]
	public class UnitSystem : AwakeSystem<Unit, UnitType , Team>
	{
		public override void Awake(Unit self, UnitType a,Team t)
		{
			self.Awake(a,t);
		}
	}

	public sealed class Unit: Entity
	{
        public RollbackDriver mRollebackDriver;
        public int mPlayerIndex;
        public bool ReadyForUpdate = false;
        Dictionary<int, InputState> mFrameWithInputDic = new Dictionary<int, InputState>();

        Queue<C2SInputMessage> incomingMessageQueue = new Queue<C2SInputMessage>();

        public InputState mNowInpuState = InputState.None;
		public UnitType UnitType { get; private set; }

        public InputAssignment mInputAssignment { get; set; }

        public Team mTeam { get; private set; }
        [BsonIgnore]
		public Vector3 Position { get; set; }
		
        public long mPlayerID { get; set; }

        public void SetUpdateState(bool state)
        {
            this.ReadyForUpdate = state;
        }

        public void SetPlayerInputIndex(int i)
        {
            mPlayerIndex = i;
        }

        public void Awake(UnitType unitType,Team Team)
		{
			this.UnitType = unitType;
            mTeam = Team;

        }

        public void QueueMessage(C2SInputMessage message)
        {
            incomingMessageQueue.Enqueue(message);
        }

        public C2SInputMessage ReadNetMessage()
        {
            if (incomingMessageQueue.Count > 0)
                return incomingMessageQueue.Dequeue();

            return null;
        }

        public void AddInputStateWithFrame(InputState state)
        {
            mNowInpuState = state;
            mFrameWithInputDic[mRollebackDriver.CurrentFrame] = state;
        }

        public void SetRollBackDriver(RollbackDriver d)
        {
            mRollebackDriver = d;
        }

        public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}