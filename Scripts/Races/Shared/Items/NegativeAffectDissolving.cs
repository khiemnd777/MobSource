using UnityEngine;

namespace Mob
{
	public class NegativeAffectDissolving : Affect
	{
		public override void Init ()
		{
			timeToDestroy = 0f;
		}

		public override void Execute ()
		{
			own.GetModule<AffectModule>((a) => {
				a.RemoveAffect(m => typeof(INegativeAffect).IsAssignableFrom(m.GetType()));
			});
		}
	}

	public class NegativeAffectDissolvingItem: Item
	{
		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<NegativeAffectDissolving> (own, targets);
			return true;
		}
	}

	public class NegativeAffectDissolvingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "Dissolve all negative affects";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<NegativeAffectDissolvingItem> (who, price, quantity, x => x.title = title);
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<NegativeAffectDissolvingItem> (who, targets, price, x => x.title = title);
		}
	}
}

