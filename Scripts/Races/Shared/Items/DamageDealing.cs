using UnityEngine;

namespace Mob
{
	public class DamageDealing : Affect
	{
		public float damage;

		public override void Init ()
		{
			timeToDestroy = 0f;
		}

		public override void Execute (Race target)
		{
			target.GetModule<HealthPowerModule> (hp => hp.SubtractHp (damage));
		}
	}

	public class DamageDealingItem: Item
	{
		public float damage;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<DamageDealing> (own, targets, d => d.damage = damage);
			return true;
		}
	}

	public class DamageDealingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "Deal " + damage + " damage";
		}

		public float damage;

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<DamageDealingItem> (who, price, quantity, x => {
				x.damage = damage;
				x.title = title;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<DamageDealingItem> (who, targets, price, x => {
				x.damage = damage;
				x.title = title;
			});
		}
	}
}

