using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddMagicChance : Affect
	{
		public float chance;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddMagicChanceEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicAttack *= (1f + chance);
				x.Calculate2ndPoint(StatType.Intelligent);
			});
		}
	}

	public class AddMagicChanceEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddMagicChanceItem: Item {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% magic";
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddMagicChance> (own, targets, x => x.chance = chance);
			return true;
		}
	}

	public class AddMagicChanceBoughtItem: BoughtItem {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% magic";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddMagicChanceItem> (who, price, quantity, x => x.chance = chance);
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddMagicChanceItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.chance = chance;
				x.timeToDestroy = 2f;
			});
		}
	}
}

