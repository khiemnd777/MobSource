using System;

namespace Mob
{
	public class Artifact2 : Affect, IAdditionalAffectChange<BurnAffect>
	{
		#region IAdditionalAffectChange implementation

		public float AdditionalAffectChange (BurnAffect affect, Race target)
		{
			return !ableBurnAffect || Affect.HasAffect<BurnAffect> (target) ? 0f : 1f;
		}

		public void DefineAffect (BurnAffect affect)
		{
			var stat = own.GetModule<StatModule> ();
			var damage = 5f + 1.05f * stat.magicAttack;
			affect.subtractHp += damage;
		}

		public bool UseDefineAffect (BurnAffect affect)
		{
			return true;
		}

		#endregion

		bool ableBurnAffect;

		public override void EmitAffect (EmitAffectArgs args)
		{
			ableBurnAffect = typeof(IMagicalAttackingEventHandler).IsAssignableFrom (args.affect.GetType ());
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicAttack += 50f;
				x.luck += 10f;
				x.Calculate2ndPoint(StatType.Intelligent);
				x.Calculate2ndPoint(StatType.Luck);
			});
		}

		public override void Disuse ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicAttack -= 50f;
				x.luck -= 10f;
				x.Calculate2ndPoint(StatType.Intelligent);
				x.Calculate2ndPoint(StatType.Luck);
			});
		}
	}

	public class Artifact2Item: Item, ISelfUsable
	{
		public override bool Use (Race[] targets)
		{
			if (Affect.HasAffect<Artifact1> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Artifact2> (own, targets);
			return true;
		}

		public override bool Disuse ()
		{
			Affect.GetAffects<Artifact2> (own, x => {
				x.Disuse();
				Destroy(x.gameObject);
			});
			return true;
		}
	}
}

