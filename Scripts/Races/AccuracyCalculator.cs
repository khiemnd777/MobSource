using UnityEngine;
using System.Linq;

namespace Mob
{
	public class AccuracyCalculator
	{
		public static float ToPercent(float attackerAR, float targetAR){
			var acNum = (attackerAR * 0.4f - targetAR) + 9f;
			if (acNum < 0f) {
				return 0f;
			} else if (Mathf.Clamp (acNum, 0f, 0.9f) == acNum) {
				return .25f;
			} else if (Mathf.Clamp (acNum, 1f, 2f) == acNum) {
				return .3f;
			} else if (Mathf.Clamp (acNum, 3f, 4f) == acNum) {
				return .4f;
			} else if (Mathf.Clamp (acNum, 5f, 5.9f) == acNum) {
				return .5f;
			} else if (Mathf.Clamp (acNum, 6f, 6.9f) == acNum) {
				return .6f;
			} else if (Mathf.Clamp (acNum, 7f, 7.9f) == acNum) {
				return .8f; 
			} else {	
				return 1f;
			}
		}

		public static bool IsHit(float attackerAR, float targetAR) {
			var accuracy = Mathf.Clamp((attackerAR / targetAR) / 2f, 0.2f, 0.95f);
			var accuracyProbability = Probability.Initialize(new bool[]{false, true}, new float[] {100f - (accuracy * 100f), accuracy * 100f});
			var accuracyResult = Probability.GetValueInProbability(accuracyProbability);
			return accuracyResult;
		}

		public static bool MakeSureHit(Race own){
			var result = false;
			own.GetModule<AffectModule> (am => {
				result = am.HasSubAffect<IHittable>();
			});
			return result;
		}

		public static bool MakeSureCritical(Race own){
			var result = false;
			own.GetModule<AffectModule> (am => {
				result = am.HasSubAffect<ICritical>();
			});
			return result;
		}

		public static bool IsCriticalHit(Race who, float attackerCHC) {
			var addition = 0f;
			var percentCHC = attackerCHC * 100f;
			Affect.GetSubAffects<IAdditionalCriticalHitChange> (who, x => {
				addition += x.additionalCriticalHitChange;
			});
			addition *= 100f;
			percentCHC = Mathf.Min(percentCHC + addition, 100f);

			var chcProbability = Probability.Initialize (new bool[]{ false, true }, new float[] {
				100f - percentCHC,
				percentCHC
			});
			var chcResult = Probability.GetValueInProbability (chcProbability);
			return chcResult;
		}

		public static void HandleCriticalDamage(ref float damage, Race own, Race target) {
			var _ = float.MinValue;
			var d = damage;
			own.GetModule<AffectModule> (am => {
				am.GetSubAffects<ICriticalHandler>(c => {
					_ = Mathf.Max(_, c.HandleCriticalDamage(d, target));
				});
			});
			damage = Mathf.Max (_, damage);
		}

		public static void HandleAccuracy(ref float accuracy, Race own, Race target){
			var _ = float.MinValue;
			own.GetModule<AffectModule>(am => {
				am.GetSubAffects<IAccurate>(a => {
					_ = Mathf.Max(_, a.HandleAccuracy(target));
				});
			});
			accuracy = Mathf.Max (_, accuracy);
		}

		public static bool IsDodgeable(Race own, Race target){
			var _ = 0f;
			target.GetModule<AffectModule>(am => {
				am.GetSubAffects<IDodgeableChance>(a => {
					_ = Mathf.Min(_, a.dodgeChance);
				});
			});

			var percents = new float[] {_, 100f - _};
			// init accuracy values
			var accuracies = new bool[] {true, false};
			var arr = Probability.Initialize(accuracies, percents);
			var index = Random.Range (0, arr.Length - 1);
			return arr [index];
		}

		public static float GetAccuracyWithProbability(float chance, float currentAccuracy){
			// init percent of chance
			var percents = new float[] {chance, 100f - chance};
			// init accuracy values
			var accuracies = new float[] {0f, currentAccuracy};
			var arr = Probability.Initialize(accuracies, percents);
			var index = Random.Range (0, arr.Length - 1);
			return arr [index];

		}
	}
}

