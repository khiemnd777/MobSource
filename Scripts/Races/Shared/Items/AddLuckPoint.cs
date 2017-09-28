using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddLuckPoint : Affect
	{
		public float point;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddLuckPointEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => x.luck += point);
		}
	}

	public class AddLuckPointEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddLuckPointItem: Item {

		public float point;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddLuckPoint> (own, targets, x => x.point = point);
			return true;
		}
	}

	public class AddLuckPointBoughtItem: BoughtItem {

		public float point;

		public override void Init ()
		{
			title = "+ " + point + " luck";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddLuckPointItem> (who, price, quantity, x => {
				x.title = title;
				x.point = point;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddLuckPointItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.title = title;
				x.point = point;
				x.timeToDestroy = 2f;
			});
		}
	}
}

