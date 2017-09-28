using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class AddCriticalChance : Affect, IAdditionalCriticalHitChange
	{
		public float additionalCriticalHitChange {
			get {
				return chance;
			}
		}

		public float chance;

		public override void Init ()
		{
			timeToDestroy = 0f;
			AddPlugin (Effect.CreatePrimitive<AddCriticalChanceEffect> (this, own, targets));
		}

		public override void EmitAffect (EmitAffectArgs args)
		{
			if (typeof(IPhysicalAttackingEventHandler).IsAssignableFrom (args.affect.GetType()) 
				|| typeof(IMagicalAttackingEventHandler).IsAssignableFrom (args.affect.GetType()) ) {
				Destroy (gameObject, 2f);
			}
		}
	}

	public class AddCriticalChanceEffect: Effect {

		public override void InitPlugin ()
		{
			use = false;
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			yield return null;
		}
	}

	public class AddCriticalChangeItem: Item {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% critical";
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<AddCriticalChance> (own, targets, x => x.chance = chance);
			return true;
		}
	}

	public class AddCriticalChangeBoughtItem: BoughtItem {

		public float chance;

		public override void Init ()
		{
			title = "+ " + chance * 100f + "% critical";
		}

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<AddCriticalChangeItem> (who, price, quantity, x => x.chance = chance);
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<AddCriticalChangeItem> (who, targets, price, x => {
//				timeToDestroy = 5f;
				x.chance = chance;
				x.timeToDestroy = 2f;
			});
		}
	}
}

