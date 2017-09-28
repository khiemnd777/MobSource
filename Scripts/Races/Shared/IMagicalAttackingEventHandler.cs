using System;
using System.Collections;

namespace Mob
{
	public interface IMagicalAttackingEventHandler {
		float bonusDamage { get; }
		void HandleAttack(Race target);
	}

	public class MagicAttackCalculator
	{
		public static void Calculate(float bonusDamage, Race attacker, Race[] targets){
			var stat = attacker.GetModule<StatModule> ();
			foreach (var target in targets) {
				var targetStat = target.GetModule<StatModule> ();
				var isHit = AccuracyCalculator.IsHit (stat.attackRating, targetStat.attackRating);
				isHit = !isHit ? AccuracyCalculator.MakeSureHit(attacker) : isHit;
				if (isHit) {
					var isCritHit = AccuracyCalculator.IsCriticalHit (attacker, stat.criticalRating);
					isCritHit = !isCritHit ? AccuracyCalculator.MakeSureCritical (attacker) : isCritHit;
					// optional Damage
					var outputDamage = AttackPowerCalculator.TakeDamage(bonusDamage, targetStat.magicResist, isCritHit);
					AccuracyCalculator.HandleCriticalDamage (ref outputDamage, attacker, target);
					AttackPowerCalculator.HandleDamage(ref outputDamage, attacker, target);
					target.GetModule<HealthPowerModule> (x => x.SubtractHp (outputDamage));
				} else {
					var isCritHit = AccuracyCalculator.IsCriticalHit (attacker, stat.criticalRating);
					var damage = AttackPowerCalculator.TakeDamage(bonusDamage, targetStat.physicalDefend, isCritHit);
					Affect.GetSubAffects<IMissingHandler>(target, handler => handler.HandleMissing(damage, attacker));
				}
			}
		}
	}
}

