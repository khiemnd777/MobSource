using UnityEngine;
using System.Linq;
using System.Collections;

namespace Mob
{
	public class Helm : GearAffect
	{
		public float hp = 50f;

		public override void Init ()
		{
			gainPoint = 5f;
		}

		public override void Execute ()
		{
			own.GetModule<HealthPowerModule>(x => x.maxHp = CalculatorUtility.AddExtraValueByPercent(x.maxHp, hp, .2f, upgradeCount));
		}

		public override bool Upgrade ()
		{
			++upgradeCount;
			gainPoint += upgradeCount % 3 == 0 ? Mathf.Ceil(1f * 1.175f) : 0f;
			hp *= 1.2f;
			Execute ();
			AddGainPoint ();

			return true;
		}

		public override void Disuse ()
		{
			own.GetModule<HealthPowerModule>(x => x.maxHp -= hp);
			DestroyImmediate (gameObject);
		}
	}

	public class HelmItem: GearItem, ISelfUsable {
		
		public override void Init ()
		{
			upgradePrice = 40f;

			icon.prefabs.Add ("lvl1", "Sprites/Gears => helm_1");
			icon.prefabs.Add ("lvl5", "Sprites/Gears => helm_5");
			icon.prefabs.Add ("lvl10", "Sprites/Gears => helm_10");
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

		public override bool Use (Race[] targets)
		{
			if (Affect.HasAffect<Helm> (own))
				return false;
			
			Affect.CreatePrimitiveAndUse<Helm> (own, targets);
			upgradePrice *= 1.2f;
			return true;
		}

		public override bool Upgrade (float price = 0)
		{
			if (upgradeCount == 9)
				return false;
			
			if (EnoughGold (own, upgradePrice)) {
				++upgradeCount;
				Affect.HasAffect<Helm> (own, (a) => {
					a.Upgrade ();
					title = "Helm lv." + (upgradeCount + 1);
					brief = "+" + Mathf.Floor(a.hp) + " hp";
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
			Affect.GetAffects<Helm> (own, x => x.Disuse ());
			DestroyImmediate (gameObject);
			return true;
		}
	}

	public class HelmBoughtItem: GearBoughtItem {
		public override void Init ()
		{
			gearType = GearType.Helm;
			title = "Helm lv.1";
			brief = "+50 hp";
			price = 40f;
		}

		public override void Buy (Race who, float price = 0f, int quantity = 0)
		{
			BuyAndUseImmediately<HelmItem> (who, new Race[]{ who }, price, a => {
				AlternateInStoreState();
				who.GetModule<GearModule> (x => {
					if(x.helm != null)
						x.helm.Disuse();
				});
				a.title = title;
				a.brief = brief;
				a.gearType = gearType;
				a.upgradePrice = this.price;
				who.GetModule<GearModule>(x => x.helm = a);
			});
		}
	}
}

