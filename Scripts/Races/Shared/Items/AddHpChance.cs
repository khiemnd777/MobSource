using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddHpChance : Affect
	{
		public float chance;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddHpChanceEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<HealthPowerModule> (x => x.hp *= (1f + chance));
		}
	}

	public class AddHpChanceEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			foreach (var target in targets) {
				target.GetModule<HealthPowerModule> (x => x.SubtractHpEffect ());
			}
			yield return null;
		}
	}

	public class AddHpChanceItem: Item {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% hp";
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddHpChance> (own, targets, x => x.chance = chance);
			return true;
		}
	}

	public class AddHpChanceBoughtItem: BoughtItem {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% hp";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddHpChanceItem> (who, price, quantity, x => x.chance = chance);
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddHpChanceItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.chance = chance;
				x.timeToDestroy = 2f;
			});
		}
	}
}

