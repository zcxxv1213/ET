using System.Numerics;
using MongoDB.Bson.Serialization.Attributes;
using RollBack;

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
        public int mPlayerIndex;
        public bool ReadyForUpdate = false;
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

        public void SerializeComponentsAndData()
        {

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