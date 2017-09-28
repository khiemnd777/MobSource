using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddIntelligentPoint : Affect
	{
		public float point;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddIntelligentPointEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => x.intelligent += point);
		}
	}

	public class AddIntelligentPointEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddIntelligentPointItem: Item {

		public float point;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddIntelligentPoint> (own, targets, x => x.point = point);
			return true;
		}
	}

	public class AddIntelligentPointBoughtItem: BoughtItem {

		public float point;

		public override void Init ()
		{
			title = "+ " + point + " intelligent";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddIntelligentPointItem> (who, price, quantity, x => {
				x.title = title;
				x.point = point;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddIntelligentPointItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.title = title;
				x.point = point;
				x.timeToDestroy = 2f;
			});
		}
	}
}

