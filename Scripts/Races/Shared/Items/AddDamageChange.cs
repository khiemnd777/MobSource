using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddDamageChance : Affect
	{
		public float chance;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddDamageChanceEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraPhysicalAttack *= (1f + chance);
				x.Calculate2ndPoint(StatType.Strength);
			});
		}
	}

	public class AddDamageChanceEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddDamageChanceItem: Item {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% damage";
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddDamageChance> (own, targets, x => x.chance = chance);
			return true;
		}
	}

	public class AddDamageChanceBoughtItem: BoughtItem {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% damage";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddDamageChanceItem> (who, price, quantity, x => x.chance = chance);
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddDamageChanceItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.chance = chance;
				x.timeToDestroy = 2f;
			});
		}
	}
}

