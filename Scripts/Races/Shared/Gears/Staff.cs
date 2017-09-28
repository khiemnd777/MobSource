using UnityEngine;
using System.Linq;
using System.Collections;

namespace Mob
{
	public class Staff : GearAffect
	{
		public float magicAttack = 20f;

		public override void Init ()
		{
			gainPoint = 6f;
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicAttack = CalculatorUtility.AddExtraValueByPercent(x.extraMagicAttack, magicAttack, .2f, upgradeCount);
				x.Calculate2ndPoint(StatType.Intelligent);
			});
		}

		public override bool Upgrade ()
		{
			++upgradeCount;
			gainPoint += upgradeCount % 3 == 0 ? Mathf.Ceil(1f * 1.175f) : 0f;
			magicAttack *= 1.2f;
			Execute ();
			AddGainPoint ();

			return true;
		}

		public override void Disuse ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicAttack -= magicAttack;
				x.Calculate2ndPoint(StatType.Intelligent);
			});	
			DestroyImmediate (gameObject);
		}
	}

	public class StaffItem: GearItem, ISelfUsable {

		public override void Init ()
		{
			upgradePrice = 50f;

			icon.prefabs.Add ("lvl1", "Sprites/Gears => staff_1");
			icon.prefabs.Add ("lvl5", "Sprites/Gears => staff_5");
			icon.prefabs.Add ("lvl10", "Sprites/Gears => staff_10");
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
			if (Affect.HasAffect<Staff> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Staff> (own, targets);
			upgradePrice *= 1.2f;
			return true;
		}

		public override bool Upgrade (float price = 0)
		{
			if (upgradeCount == 9)
				return false;
			if (EnoughGold (own, upgradePrice)) {
				++upgradeCount;
				Affect.HasAffect<Staff> (own, (a) => {
					a.Upgrade();
					title = "Staff lv." + (upgradeCount + 1);
					brief = "+" + Mathf.Floor(a.magicAttack) + " magic";
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
			Affect.GetAffects<Staff> (own, x => x.Disuse ());
			DestroyImmediate (gameObject);
			return true;
		}
	}

	public class StaffBoughtItem: GearBoughtItem {
		public override void Init ()
		{
			gearType = GearType.Weapon;
			title = "Staff lv.1";
			brief = "+20 magic";
			price = 50f;
		}

		public override void Buy (Race who, float price = 0f, int quantity = 0)
		{
			BuyAndUseImmediately<StaffItem> (who, new Race[]{ who }, price, a => {
				AlternateInStoreState();
				who.GetModule<GearModule> (x => {
					if(x.weapon != null){
						x.weapon.Disuse();
					}
				});
				a.title = title;
				a.brief = brief;
				a.gearType = gearType;
				a.upgradePrice = this.price;
				who.GetModule<GearModule>(x => x.weapon = a);
			});
		}
	}
}

