using System.Numerics;
using RollBack;
using RollBack.Input;
using System;
using System.Collections.Generic;
using System.IO;

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

        Queue<C2SCoalesceInput> incomingMessageQueue = new Queue<C2SCoalesceInput>();

        public InputState mNowInpuState = InputState.None;
		public UnitType UnitType { get; private set; }

        public InputAssignment mInputAssignment { get; set; }

        public Team mTeam { get; private set; }
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

        public void QueueMessage(C2SCoalesceInput message)
        {
            incomingMessageQueue.Enqueue(message);
        }

        public C2SCoalesceInput ReadNetMessage()
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
        public void Serialize(BinaryWriter bw)
        {
            //序列化位置等等信息
            bw.Write(this.mPlayerIndex);
        }

        public void DeSerialize(BinaryReader br)
        {
            this.mPlayerIndex = br.ReadInt32();
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