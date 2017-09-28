using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddStrengthPoint : Affect
	{
		public float point;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddStrengthPointEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => x.strength += point);
		}
	}

	public class AddStrengthPointEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddStrengthPointItem: Item {

		public float point;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddStrengthPoint> (own, targets, x => x.point = point);
			return true;
		}
	}

	public class AddStrengthPointBoughtItem: BoughtItem {

		public float point;

		public override void Init ()
		{
			title = "+ " + point + " strength";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddStrengthPointItem> (who, price, quantity, x => {
				x.point = point;
				x.title = title;	
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddStrengthPointItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.title = title;
				x.point = point;
				x.timeToDestroy = 2f;
			});
		}
	}
}

