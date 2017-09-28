using UnityEngine;
using System.Linq;
using System.Collections;

namespace Mob
{
	public class Armor : GearAffect
	{
		public float defend = 20f;

		public override void Init ()
		{
			gainPoint = 6f;
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraPhysicalDefend = CalculatorUtility.AddExtraValueByPercent(x.extraPhysicalDefend, defend, .2f, upgradeCount);
				x.Calculate2ndPoint(StatType.Strength);
			});
		}

		public override bool Upgrade ()
		{
			++upgradeCount;
			gainPoint += upgradeCount % 3 == 0 ? Mathf.Ceil(1f * 1.175f) : 0f;
			defend *= 1.2f;
			Execute ();
			AddGainPoint ();

			return true;
		}

		public override void Disuse ()
		{
			own.GetModule<StatModule> (x => {
				x.extraPhysicalDefend -= defend;
				x.Calculate2ndPoint(StatType.Strength);
			});
			DestroyImmediate (gameObject);
		}
	}

	public class ArmorItem: GearItem, ISelfUsable {

		public override void Init ()
		{
			upgradePrice = 50f;

			// => The new way to init an icon
			icon.prefabs.Add ("lvl1", "Sprites/Gears => armor_1");
			icon.prefabs.Add ("lvl5", "Sprites/Gears => armor_5");
			icon.prefabs.Add ("lvl10", "Sprites/Gears => armor_10");
		}

		public override string GetSyncIcon ()
		{
			if (upgradeCount >= 0 && upgradeCount < 4) {
				return icon.GetIconName("lvl1");
			} else if (upgradeCount >= 4 && upgradeCount < 9) {
				return icon.GetIconName("lvl5");
			} else {
				return icon.GetIconName("lvl10");
			}
		}

		public override Sprite GetIcon(){
			if (upgradeCount >= 0 && upgradeCount < 4) {
				return GetIcon ("lvl1");	
			} else if (upgradeCount >= 4 && upgradeCount < 9) {
				return GetIcon ("lvl5");	
			} else {
				return GetIcon ("lvl10");	
			}
		}

		public override bool Use (Race[] targets)
		{
			if (Affect.HasAffect<Armor> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Armor> (own, targets);
			upgradePrice *= 1.2f;
			return true;
		}

		public override bool Upgrade (float price = 0)
		{
			if (upgradeCount == 9)
				return false;
			
			if (EnoughGold (own, upgradePrice)) {
				++upgradeCount;

				Affect.HasAffect<Armor> (own, (a) => {
					a.Upgrade();
					title = "Armor lv." + (upgradeCount + 1);
					brief = "+" + Mathf.Floor(a.defend) + " defend";
					SubtractGold (own, upgradePrice);
					upgradePrice *= 1.2f;
				});
				var addingItem = GetRandomItem ();
				if (addingItem) {
					brief += ", " + addingItem.brief;
				}
				return true;
			}
			return false;
		}

		public override bool Disuse ()
		{
			Affect.GetAffects<Armor> (own, x => x.Disuse ());
			DestroyImmediate (gameObject);
			return true;
		}
	}

	public class ArmorBoughtItem: GearBoughtItem 
	{
		public override void Init ()
		{
			gearType = GearType.Armor;
			title = "Armor lv.1";
			brief = "+20 defend";
			price = 50f;
		}

		public override void Buy (Race who, float price = 0f, int quantity = 0)
		{
			BuyAndUseImmediately<ArmorItem> (who, new Race[]{ who }, price, a => {
				AlternateInStoreState();
				who.GetModule<GearModule> (x => {
					if(x.armor != null)
						x.armor.Disuse();
				});
				a.title = title;
				a.brief = brief;
				a.gearType = gearType;
				a.upgradePrice = this.price;
				who.GetModule<GearModule>(x => x.armor = a);
			});
		}
	}
}

