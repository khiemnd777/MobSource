using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class SwordmanD1 : SkillAffect, ICriticalHandler, IPhysicalAttackingEventHandler
	{
		public float storedEnergy;
	
		public override void Init ()
		{
			timeToDestroy = 0f;
			gainPoint = 18f;
		}

		#region ICriticalHandler implementation

		public float HandleCriticalDamage (float damage, Race target)
		{
			own.GetModule<EnergyModule> (x => x.AddEnergy(storedEnergy / 2));
			return damage;
		}

		#endregion

		#region IPhysicalAttackingEventHandler implementation

		public float bonusDamage {
			get {
				var stat = own.GetModule<StatModule>();
				return 20f + 10f * storedEnergy + 1.4f * stat.magicAttack;
			}
		}

		public void HandleAttack(Race target){
			
		}

		#endregion
	}

	public class SwordmanD1Effect: Effect {

		Text targetHpLabel;
		public override void InitPlugin ()
		{
			use = true;
			targetHpLabel = GetMonoComponent<Text> (Constants.TARGET_HP_LABEL);
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			if ((bool)effectValues ["isHit"]) {
				var damage = (float)effectValues["damage"];
				var target = (Race)effectValues ["target"];
				if (targetHpLabel == null) {
					target.GetModule<HealthPowerModule> (x => x.SubtractHpEffect ());
					Destroy (((Affect)host).gameObject, Constants.WAIT_FOR_DESTROY);
				} else {
					yield return OnSetTimeout (() => {
						var slashLine = InstantiateFromMonoResource<SlashLine>(Constants.EFFECT_SLASH_LINE);
						slashLine.target = targetHpLabel.transform;
					}, 0.05f);

					target.GetModule<HealthPowerModule> (x => x.SubtractHpEffect ());

					JumpEffect (targetHpLabel.transform, Vector3.one);

					ShowSubLabel (Constants.DECREASE_LABEL, targetHpLabel.transform, damage);
				}
			}
		}
	}

	public class SwordmanD1Skill : Skill
	{
		public override void Init ()
		{
			level = 12;
			cooldown = 3;
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<SwordmanD1> (own, targets, t => {
				t.gainPoint = gainPoint;
				t.storedEnergy = Mathf.Max (1f, own.GetModule<EnergyModule> ().energy);
			});
			return true;
		}

		protected override bool Interact ()
		{
			energy = Mathf.Max(1f, own.GetModule<EnergyModule> ().energy);
			return base.Interact ();
		}
	}

	public class SwordmanD1BoughtSkill : SkillBoughtItem
	{
		public override void Init ()
		{
			title = "D1";
			brief = "20 magical attack (extra 10 x energy + 40% magic damage). Yet to restore 1/2 of enegery if it be occurred the critical damage.";
			cooldown = 3;
			learnedLevel = 12;
			gainPoint = 18f;
			reducedEnergy = 12f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/swordman-skills => swordman-skills-d1");
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default")? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override void Pick (Race who, int quantity)
		{
			var skillModule = who.GetModule<SkillModule> ();
			if (skillModule.evolvedSkillPoint <= 0)
				return;
			who.GetModule<SkillModule> (x => x.Add<SwordmanD1Skill> (quantity, t => {
				t.icon = icon;
				t.title = title;
				t.brief = brief;
				t.cooldown = cooldown;
				t.level = learnedLevel;
				t.gainPoint = gainPoint;
			}));
			--skillModule.evolvedSkillPoint;
			enabled = false;
		}

		LevelModule _level;
		SkillModule _skill;

		protected override bool Interact ()
		{
			var level = _level ?? (_level = own.GetModule<LevelModule> ());
			var skill = _skill ?? (_skill = own.GetModule<SkillModule> ());
			return level.level >= 12 && skill.evolvedSkillPoint > 0 && !skill.HasSkill<SwordmanD1Skill>();
		}
	}
}