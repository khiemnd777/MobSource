using UnityEngine;

namespace Mob
{
	public class Swordman : Human, ILevelUpEventHandler
	{
		StatModule _stat;
		HealthPowerModule _hp;
		SkillModule _skill;

		void Start(){
			_stat = GetModule<StatModule>();
			_hp = GetModule<HealthPowerModule> ();
			_skill = GetModule<SkillModule> ();
		}

		#region ILevelUpEventHandler implementation

		public void OnLevelUp (int level, int levelUp)
		{
			// increase evolved skill point
			if (level % 4 == 0) {
				++_skill.evolvedSkillPoint;
				for (var i = 1; i <= levelUp; i++) {
					if ((level - i) % 4 == 0) {
						++_skill.evolvedSkillPoint;
					}
				}
			}

			// Stat
			_stat.AutoAddPoint(false);
			var point = StatCalculator.GeneratePoint(levelUp, _stat.initPoint);
			_stat.SetPoint(point);
			// HP
			_hp.SetMaxHp(levelUp, false);
			// Treasures
//			BattleController.treasure = Treasure.GetFor(this);
		}

		#endregion

		public override void DefaultValue ()
		{
			className = "Swordman";

			GetModule<AffectModule> ();

			GetModule<ShopModule> (x => x.Init ());

			GetModule<StatModule> ((stat) => {
				stat.strengthPercent = 25f;
				stat.dexterityPercent = 15f;
				stat.intelligentPercent = 30f;
				stat.vitalityPercent = 20f;
				stat.luckPercent = 10f;

				stat.strength = 5f;
				stat.dexterity = 4f;
				stat.intelligent = 6f;
				stat.vitality = 5f;
				stat.luck = 5f;

				stat.SetPoint(stat.initPoint);
				stat.AddPoint(StatType.Strength);
				stat.AddPoint(StatType.Dexterity);
				stat.AddPoint(StatType.Intelligent);
				stat.AddPoint(StatType.Vitality);
				stat.AddPoint(StatType.Luck);

				stat.Init();
			});

			GetModule<LevelModule> ((level) => {
				level.level = 1;
				level.maxLevel = 16;
				level.seed = 20;
				level.Init();
			});
			GetModule<HealthPowerModule> ((hp) => {
				hp.hp = 300f;
				hp.maxHp = 300f;
				hp.hpPercent = 10f;
				hp.Init();
			});
			GetModule<GoldModule> ((gold) => {
				gold.maxGold = 9999;
				gold.Init();
			});
			GetModule<EnergyModule> ((energy) => {
				energy.maxEnergy = 12f;
				energy.Init();
			});

			// Inventory is used to store the items
			GetModule<BagModule> (inventory => {
				inventory.Init();
//				inventory.Add<SpeedyItem>(99);
//				inventory.Add<PotionItem>(99);
//				inventory.Add<GreatPotionItem>(99);
//				inventory.Add<BurstStrengthItem>(99);
//				inventory.Add<AntidoteItem>(99);
			});

			// Skill is used to store the skills
			GetModule<SkillModule> (skill => {				
				//skill.Add<SwordmanA1Skill>(1);
				skill.AddAvailableSkill<SwordmanA1BoughtSkill>();
				skill.AddAvailableSkill<SwordmanB1BoughtSkill>();
				skill.AddAvailableSkill<SwordmanB2BoughtSkill>();
				skill.AddAvailableSkill<SwordmanB3BoughtSkill>();
				skill.AddAvailableSkill<SwordmanC1BoughtSkill>();
				skill.AddAvailableSkill<SwordmanC2BoughtSkill>();
				skill.AddAvailableSkill<SwordmanD1BoughtSkill>();
				skill.AddAvailableSkill<SwordmanE1BoughtSkill>();

//				skill.skillEffects.Add("SwordmanA1Skill",);
			});

			// Available gears are used in during game
			GetModule<GearModule> (gear => {
				gear.AddAvailableGear<HelmBoughtItem>(x => x.inStoreState = InStoreState.Available);
				gear.AddAvailableGear<ArmorBoughtItem>(x => x.inStoreState = InStoreState.Available);
				gear.AddAvailableGear<ClothBoughtItem>(x => x.inStoreState = InStoreState.Available);
				gear.AddAvailableGear<SwordBoughtItem>(x => x.inStoreState = InStoreState.Available);
				gear.AddAvailableGear<StaffBoughtItem>(x => x.inStoreState = InStoreState.Available);
				gear.AddAvailableGear<RingBoughtItem>(x => x.inStoreState = InStoreState.Available);
			});
		}

		public override void OpenSkillTree(){
			
		}

		public override void BuyItem ()
		{
			
		}

		public override void Upgrade ()
		{
			
		}
	}
}

