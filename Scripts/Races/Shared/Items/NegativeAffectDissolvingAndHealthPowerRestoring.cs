using UnityEngine;

namespace Mob
{
	public class NegativeAffectDissolvingAndHealthPowerRestoring : Affect
	{
		public float extraHp = 50f;

		public override void Init ()
		{
			timeToDestroy = 0f;
		}

		public override void Execute ()
		{
			own.GetModule<AffectModule>((a) => {
				a.RemoveAffect(m => typeof(INegativeAffect).IsAssignableFrom(m.GetType()));
			});
			Affect.CreatePrimitiveAndUse<HealthPowerRestoring> (own, new[]{ own }, hp => hp.extraHp = extraHp);
		}
	}

	public class NegativeAffectDissolvingAndHealthPowerRestoringItem: Item
	{
		public float extraHp = 50f;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<NegativeAffectDissolvingAndHealthPowerRestoring> (own, targets, n => n.extraHp = extraHp);
			return true;
		}
	}

	public class NegativeAffectDissolvingAndHealthPowerRestoringBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "Dissolve negative affects and restore " + extraHp + " HP";
		}

		public float extraHp = 50f;

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<NegativeAffectDissolvingAndHealthPowerRestoringItem> (who, price, quantity, e => {
				e.extraHp = extraHp;
				e.title = title;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<NegativeAffectDissolvingAndHealthPowerRestoringItem> (who, targets, price, e => {
				e.extraHp = extraHp;
				e.title = title;
			});
		}
	}
}

