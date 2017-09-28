using UnityEngine;
using System.Linq;
using System.Collections;

namespace Mob
{
	public class Cloth : GearAffect
	{
		public float magicResist = 15f;

		public override void Init ()
		{
			gainPoint = 5f;
		}

		public override void Execute ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicResist = CalculatorUtility.AddExtraValueByPercent(x.extraMagicResist, magicResist, .2f, upgradeCount);
				x.Calculate2ndPoint(StatType.Intelligent);
			});
		}

		public override bool Upgrade ()
		{
			++upgradeCount;
			gainPoint += upgradeCount % 3 == 0 ? Mathf.Ceil(1f * 1.175f) : 0f;
			magicResist *= 1.2f;
			Execute ();
			AddGainPoint ();

			return true;
		}

		public override void Disuse ()
		{
			own.GetModule<StatModule> (x => {
				x.extraMagicResist -= magicResist;
				x.Calculate2ndPoint(StatType.Intelligent);
			});
			DestroyImmediate (gameObject);
		}
	}

	public class ClothItem: GearItem, ISelfUsable {

		public override void Init ()
		{
			upgradePrice = 40f;

			icon.prefabs.Add ("lvl1", "Sprites/Gears => cloth_1");
			icon.prefabs.Add ("lvl5", "Sprites/Gears => cloth_5");
			icon.prefabs.Add ("lvl10", "Sprites/Gears => cloth_10");
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
			if (Affect.HasAffect<Cloth> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Cloth> (own, targets);
			upgradePrice *= 1.2f;
			return true;
		}

		public override bool Upgrade (float price = 0)
		{
			if (upgradeCount == 9)
				return false;
			
			if (EnoughGold (own, upgradePrice)) {
				++upgradeCount;
				Affect.HasAffect<Cloth> (own, (a) => {
					a.Upgrade();
					title = "Cloth lv." + (upgradeCount + 1);
					brief = "+" + Mathf.Floor(a.magicResist) + " magic resist";
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
			Affect.GetAffects<Cloth> (own, x => x.Disuse ());
			DestroyImmediate (gameObject);
			return true;
		}
	}

	public class ClothBoughtItem: GearBoughtItem {
		public override void Init ()
		{
			gearType = GearType.Cloth;
			title = "Cloth lv.1";
			brief = "+15 magic resist";
			price = 40f;
		}

		public override void Buy (Race who, float price = 0f, int quantity = 0)
		{
			BuyAndUseImmediately<ClothItem> (who, new Race[]{ who }, price, a => {
				AlternateInStoreState();
				who.GetModule<GearModule> (x => {
					if(x.cloth != null)
						x.cloth.Disuse();
				});
				a.title = title;
				a.brief = brief;
				a.gearType = gearType;
				a.upgradePrice = this.price;
				who.GetModule<GearModule>(x => x.cloth = a);
			});
		}
	}
}

