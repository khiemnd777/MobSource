using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class HealthPowerRestoring : Affect
	{
		public float extraHp;

		public override void Init ()
		{
			timeToDestroy = 5f;
			AddPlugin (Effect.CreatePrimitive<HealthPowerRestoringEffect> (this, own, targets));
		}

		public override void Execute ()
		{
			own.GetModule<HealthPowerModule>(hp => {
				var _ = float.MinValue;
				own.GetModule<AffectModule>(am => {
					am.GetSubAffects<IRestorableHealthPower>(a => {
						_ = Mathf.Max(_, a.RestoreHealthPower(extraHp));
					});
				});
				extraHp = Mathf.Max (_, extraHp);
				hp.AddHp(extraHp);
				effectValues.Add("extraHp", extraHp);
			});
		}
	}

	public class HealthPowerRestoringEffect: Effect
	{
		Text attackerHpLabel;

		public override void InitPlugin ()
		{
			attackerHpLabel = GetMonoComponent<Text> (Constants.ATTACKER_HP_LABEL);
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			var extraHp = (float)effectValues ["extraHp"];

			attacker.GetModule<HealthPowerModule> (hp => hp.AddHpEffect ());

			JumpEffect (attackerHpLabel.transform, Vector3.one);

			ShowSubLabel (Constants.INCREASE_LABEL, attackerHpLabel.transform, extraHp);

			yield return null;
		}
	}

	public class HealthPowerRestoringItem: Item
	{
		public float extraHp;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<HealthPowerRestoring> (own, targets, hp => hp.extraHp = extraHp);
			return true;
		}
	}

	public class HealthPowerRestoringBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraHp + " HP";
		}

		public float extraHp;

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<HealthPowerRestoringItem> (who, price, quantity, e => {
				e.title = title;
				e.extraHp = extraHp;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<HealthPowerRestoringItem> (who, targets, price, e => {
				e.title = title;
				e.extraHp = extraHp;
			});
		}
	}
}

