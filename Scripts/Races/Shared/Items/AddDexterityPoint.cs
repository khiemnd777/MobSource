using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddDexterityPoint : Affect
	{
		public float point;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddDexterityPointEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => x.dexterity += point);
		}
	}

	public class AddDexterityPointEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddDexterityPointItem: Item {

		public float point;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddDexterityPoint> (own, targets, x => x.point = point);
			return true;
		}
	}

	public class AddDexterityPointBoughtItem: BoughtItem {

		public float point;

		public override void Init ()
		{
			title = "+ " + point + " dex";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddDexterityPointItem> (who, price, quantity, x => {
				x.point = point;
				x.title = title;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddDexterityPointItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.point = point;
				x.title = title;
				x.timeToDestroy = 2f;
			});
		}
	}
}

