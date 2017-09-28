using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class SwordmanC1: SkillAffect, IPhysicalAttackingEventHandler
	{
		public override void Init ()
		{
			timeToDestroy = 0f;
			gainPoint = 12f;
//			AddPlugin (Effect.CreatePrimitive<SwordmanC1Effect> (this, own, targets));
		}

		#region IPhysicalAttackingEventHandler implementation

		public float bonusDamage {
			get {
				var stat = own.GetModule<StatModule>();
				return 80f + 1.5f * stat.physicalAttack;
			}
		}

		public void HandleAttack(Race target){
			
		}

		#endregion
	}

	public class SwordmanC1Effect: Effect {

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

	public class SwordmanC1Skill: Skill
	{
		public override void Init ()
		{
			level = 8;
			energy = 8f;
			cooldown = 2;
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

//		bool subEnable;
//
//		void Update(){
//			if (own.isEndTurn) {
//				subEnable = false;	
//			}
//			if (!own.isTurn && subEnable) {
//				return;
//			}
//			foreach (var target in own.targets) {
//				if (Affect.HasAffect<StunAffect> (target)) {
//					usedTurn = 0;
//					subEnable = true;
//					break;
//				}	
//			}
//		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<SwordmanC1> (own, targets, t => t.gainPoint = gainPoint);
			return true;
		}

		protected override bool Interact ()
		{
			energy = own.targets.Any (x => Affect.HasAffect<StunAffect> (x)) ? 4f : 8f;
			return base.Interact ();
		}
	}

	public class SwordmanC1BoughtSkill : SkillBoughtItem
	{
		public override void Init ()
		{
			title = "C1";
			brief = "80 physical damage (+50% physcial attack) to the opponent. Yet to use with 4 energy if the opponent had be stun affect.";
			cooldown = 2;
			learnedLevel = 8;
			gainPoint = 12f;
			reducedEnergy = 8f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/swordman-skills => swordman-skills-c1");
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
			who.GetModule<SkillModule> (x => x.Add<SwordmanC1Skill> (quantity, t => 
				{
					t.icon = icon;
					t.title = title;
					t.brief = brief;
					t.cooldown = cooldown;
					t.level = learnedLevel;
					t.gainPoint = gainPoint;
					t.energy = reducedEnergy;
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
			return level.level >= 8 && skill.evolvedSkillPoint > 0 && !skill.HasSkill<SwordmanC1Skill>();
		}
	}
}

