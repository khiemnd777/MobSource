using System;
using System.Linq;
using UnityEngine;

namespace Mob
{
	public class SwordmanE1 : SkillAffect, IDamaged, IHittable, ICritical
	{

		public override void Init ()
		{
			gainPoint = 30f;
		}

		void Update(){
			ExecuteInTurn (own, () => {
				if(turn == 3){
					Destroy(gameObject);
				}
			});
		}

		#region IDamaged implementation

		public float HandleDamage (float damage, Race target)
		{
			var result = 0f;
			target.GetModule<AffectModule> (x => {
				if(x.HasSubAffect<INegativeAffect>()){
					result = damage + damage * 1.5f;
				}
			});
			return result;
		}

		#endregion
	}

	public class SwordmanE1Skill : Skill
	{
		public override void Init ()
		{
			title = "Holy Knight";
			energy = 12f;
			level = 16;
			cooldown = 6;
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<SwordmanE1> (own, targets, t => {
				t.gainPoint = gainPoint;
			});
			return true;
		}
	}

	public class SwordmanE1BoughtSkill : SkillBoughtItem
	{
		public override void Init ()
		{
			title = "E1";
			brief = "Transforming to Holy Knight in 3 turns for being critical damage at all. Yet to increase to 50% for all attack damages if the opponent has any negative effects.";
			cooldown = 6;
			learnedLevel = 16;
			gainPoint = 30f;
			reducedEnergy = 12f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/swordman-skills => swordman-skills-e1");
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
			who.GetModule<SkillModule> (x => x.Add<SwordmanE1Skill> (quantity, t => 
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
			return level.level >= 16 && skill.evolvedSkillPoint > 0 && !skill.HasSkill<SwordmanE1Skill>();
		}
	}
}

