using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddMagicResistChance : Affect
	{
		public float chance;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddMagicResistChanceEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => x.extraMagicResist *= (1f + chance));
		}
	}

	public class AddMagicResistChanceEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddMagicResistChanceItem: Item {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% magic resist";
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddMagicResistChance> (own, targets, x => x.chance = chance);
			return true;
		}
	}

	public class AddMagicResistChanceBoughtItem: BoughtItem {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% magic resist";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddMagicResistChanceItem> (who, price, quantity, x => x.chance = chance);
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddMagicResistChanceItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.chance = chance;
				x.timeToDestroy = 2f;
			});
		}
	}
}

