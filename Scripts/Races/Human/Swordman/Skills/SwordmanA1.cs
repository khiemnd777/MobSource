using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class SwordmanA1 : SkillAffect, IPhysicalAttackingEventHandler
	{
		public float bonusDamage {
			get {
				var stat = own.GetModule<StatModule>();
				return 2.1f * stat.physicalAttack;
			}
		}

		public void HandleAttack(Race target){
			
		}

		public override void Init(){
			timeToDestroy = 0f;
			gainPoint = 5f;
//			AddPlugin (Effect.CreatePrimitive<SwordmanA1Effect>(this, own, targets));
		}
	}

	public class SwordmanA1Effect: Effect {
		
		Text targetHpLabel;
		public override void InitPlugin ()
		{
			use = true;
			targetHpLabel = GetMonoComponent<Text> (Constants.TARGET_HP_LABEL);
		}

		public override IEnumerator Define (Dictionary<string, object> effectValues)
		{
			var evt = attacker.GetModule<EffectValueTransferModule> ();
			if (evt.GetValue<bool>("isHit")) {
				var damage = evt.GetValue<float> ("damage");
				var targetNetId = evt.GetValue<uint>("targetNetId");
				var targetGo = ClientScene.FindLocalObject(new NetworkInstanceId(targetNetId));
				var target = targetGo.GetComponent<Race> ();
				EventManager.TriggerEvent (Constants.EVENT_HP_SUBTRACTING_EFFECT, new { evt = evt, targetNetId = targetNetId });

				if (targetHpLabel == null) {
					target.GetModule<HealthPowerModule> (x => x.SubtractHpEffect ());
				} else {
					yield return OnSetTimeout (() => {
						var slashLine = InstantiateFromMonoResource<SlashLine> (Constants.EFFECT_SLASH_LINE);
						slashLine.target = targetHpLabel.transform;
					}, 0.05f);

					target.GetModule<HealthPowerModule> (x => x.SubtractHpEffect ());

					JumpEffect (targetHpLabel.transform, Vector3.one);

					ShowSubLabel (Constants.DECREASE_LABEL, targetHpLabel.transform, damage);
				}
			}
		}
	}

	public class SwordmanA1Skill : Skill
	{
		public override void Init ()
		{
			level = 1;
			energy = 4f;
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<SwordmanA1> (own, targets, t => {
				t.gainPoint = gainPoint;

				if(usedNumber == 9){
					own.GetModule<SkillModule> (s => {
						s.Add<SwordmanA2Skill> (1);
					});
					SetVisible(false);
				}
			});
			return true;
		}

		protected override bool Interact ()
		{
			return usedNumber < 10 && base.Interact ();
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}
	}

	public class SwordmanA1BoughtSkill : SkillBoughtItem
	{
		public override void Init ()
		{
			title = "A1";
			brief = "Increasing 110% physical damage to opponent, when it's used 10 times will be self-upgrading to A2.";
			cooldown = 0;
			learnedLevel = 1;
			reducedEnergy = 4f;
			gainPoint = 5f;
			effectType = typeof(SwordmanA1Effect);
			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/swordman-skills => swordman-skills-a1");
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override void Pick (Race who, int quantity)
		{
			who.GetModule<SkillModule> (x => x.Add<SwordmanA1Skill> (quantity, t => {
				t.icon = icon;
				t.title = title;
				t.brief = brief;
				t.gainPoint = gainPoint;
				t.level = learnedLevel;
				t.cooldown = cooldown;
				t.energy = reducedEnergy;
				t.effectType = effectType;
			}));
		}

		LevelModule _level;
		SkillModule _skill;

		protected override bool Interact ()
		{
			var level = _level ?? (_level = own.GetModule<LevelModule> ());
			var skill = _skill ?? (_skill = own.GetModule<SkillModule> ());
			return level.level == 1 && !skill.HasSkill<SwordmanA1Skill>();
		}
	}
}

