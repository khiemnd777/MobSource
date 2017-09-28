using UnityEngine;
using System;
using System.Linq;

namespace Mob
{
	public class Ring : GearAffect
	{
		float[] upPoints = new float[] { 1f, 3f, 6f };
		float[] upGainPoints = new float[] { 6f, 8f, 12f };

		public override void Init ()
		{
			gainPoint = 6f;
		}

		public float point = 1f;

		public override void Execute ()
		{
			AddAllStats ();
		}

		public override bool Upgrade ()
		{
			SubtractAllStats ();
			++upgradeCount;
			Execute ();
			AddAlternativeGainPoint ();
			return true;
		}

		public override void Disuse ()
		{
			SubtractAllStats ();
			DestroyImmediate (gameObject);
		}

		void AddAllStats(){
			point = upPoints [upgradeCount];

			own.GetModule<StatModule> (x => {
				x.strength += point;
				x.dexterity += point;
				x.intelligent += point;
				x.vitality += point;
				x.luck += point;
			});
		}

		void SubtractAllStats(){
			point = upPoints[upgradeCount];
			own.GetModule<StatModule> (x => {
				x.strength -= point;
				x.dexterity -= point;
				x.intelligent -= point;
				x.vitality -= point;
				x.luck -= point;
			});
		}

		void AddAlternativeGainPoint(){
			gainPoint = upGainPoints [upgradeCount];
			AddGainPoint ();
		}
	}

	public class RingItem: GearItem, ISelfUsable {
		
		public override void Init ()
		{
			upgradePrice = 50f;

			icon.prefabs.Add ("lvl1", "Sprites/Gears => ring_1");
			icon.prefabs.Add ("lvl2", "Sprites/Gears => ring_2");
			icon.prefabs.Add ("lvl3", "Sprites/Gears => ring_3");
		}

		public override string GetSyncIcon ()
		{
			if (upgradeCount >= 0 && upgradeCount < 1) {
				return icon.GetIconName("lvl1");
			} else if (upgradeCount == 1) {
				return icon.GetIconName("lvl2");
			} else {
				return icon.GetIconName("lvl3");
			}
		}

		public override Sprite GetIcon(){
			if (upgradeCount >= 0 && upgradeCount < 1) {
				return GetIcon ("lvl1");	
			} else if (upgradeCount == 1) {
				return GetIcon ("lvl2");	
			} else {
				return GetIcon ("lvl3");	
			}
		}

		public override bool Use (Race[] targets)
		{
			if (Affect.HasAffect<Ring> (own))
				return false;

			Affect.CreatePrimitiveAndUse<Ring> (own, targets);
			upgradePrice = 80f;
			return true;
		}

		public override bool Upgrade (float price = 0)
		{
			if (upgradeCount == 2)
				return false;
			if (EnoughGold (own, upgradePrice)) {
				++upgradeCount;
				Affect.HasAffect<Ring> (own, (a) => {
					a.Upgrade();
					title = "Ring lv." + (upgradeCount + 1);
					brief = "+" + Mathf.Floor(a.point) + " all stats";
					SubtractGold (own, upgradePrice);
					if(upgradeCount == 1){
						upgradePrice = 120f;
					}
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
			Affect.GetAffects<Ring> (own, x => x.Disuse());
			DestroyImmediate (gameObject);
			return true;
		}
	}

	public class RingBoughtItem: GearBoughtItem {
		public override void Init ()
		{
			gearType = GearType.Ring;
			title = "Ring lv.1";
			brief = "+1 all stats";
			price = 50f;
		}

		public override void Buy (Race who, float price = 0f, int quantity = 0)
		{
			BuyAndUseImmediately<RingItem> (who, new Race[]{ who }, price, a => {
				AlternateInStoreState();
				who.GetModule<GearModule> (x =>{
					if(x.ring != null){
						x.ring.Disuse();
					}
				});
				a.title = title;
				a.brief = brief;
				a.gearType = gearType;
				a.upgradePrice = this.price;
				who.GetModule<GearModule>(x => x.ring = a);
			});
		}
	}
}

