using UnityEngine;

namespace Mob
{
	public class GoldAdding : Affect
	{
		public float extraGold;

		public override void Init ()
		{
			timeToDestroy = 0f;
		}

		public override void Execute ()
		{
			own.GetModule<GoldModule>(g => g.AddGold(extraGold));
		}
	}

	public class GoldAddingItem: Item
	{
		public float extraGold;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<GoldAdding> (own, targets, g => g.extraGold = extraGold);
			return true;
		}
	}

	public class GoldAddingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraGold + " gold";
		}

		public float extraGold;

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<GoldAddingItem> (who, price, quantity, e => {
				e.extraGold = extraGold;
				e.title = title;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<GoldAddingItem> (who, targets, price, e => {
				e.extraGold = extraGold;
				e.title = title;
			});
		}
	}
}