using System;

namespace Mob
{
	public class Artifact1 : Affect, IAdditionalCriticalHitChange, IAdditionalAffectChange<StunAffect>
	{
		public float additionalCriticalHitChange {
			get {
				return .2f;
			}
		}

		public float AdditionalAffectChange (StunAffect affect, Race target)
		{
			return Affect.HasAffect<StunAffect>(target) ? 0f : 1f;
		}

		public void DefineAffect (StunAffect affect)
		{
			affect.turnNumber = 1;
		}

		public bool UseDefineAffect(StunAffect affect) 
		{
			return true;
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraPhysicalAttack += 50f;
				x.Calculate2ndPoint(StatType.Strength);
			});
		}

		public override void Disuse ()
		{
			own.GetModule<StatModule> (x => {
				x.extraPhysicalAttack -= 50f;
				x.Calculate2ndPoint(StatType.Strength);
			});
		}
	}

	public class Artifact1Item: Item, ISelfUsable
	{
		public override bool Use (Race[] targets)
		{
			if (Affect.HasAffect<Artifact1> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Artifact1> (own, targets);
			return true;
		}

		public override bool Disuse ()
		{
			Affect.GetAffects<Artifact1> (own, x => {
				x.Disuse();
				Destroy(x.gameObject);
			});
			return true;
		}
	}
}