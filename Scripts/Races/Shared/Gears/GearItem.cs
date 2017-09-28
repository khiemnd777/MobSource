using System;
using UnityEngine.Networking;

namespace Mob
{
	public struct SyncGearItem {
		public int id;
		public int ownId;
		public uint ownNetId;
		public int effectName;
		public int[] targetId;
		public uint[] targetNetIds;
		public string title;
		public string brief;
		public float energy;
		public float gainPoint;
		public int level;
		public int upgradeCount;
		public float upgradePrice;
		public int usedTurn;
		public int cooldown;
		public string icon;
		public int usedNumber;
		public bool interactable;
		public bool cooldownable;
		public bool visible;
		public GearType gearType;
	}

	public class SyncListGearItem : SyncListStruct<SyncGearItem> { }
	
	public class GearItem : Item
	{
		public GearType gearType;

		public override bool Use (Race[] targets)
		{
			return false;
		}

		public virtual SyncGearItem ToSyncGearItem(){
			return new SyncGearItem {
				brief = this.brief,
				icon = GetSyncIcon (),
				id = GetInstanceID(),
				ownId = this.own.GetInstanceID(),
				title = this.title,
				interactable = this.Interact(),
				cooldown = this.cooldown,
				energy = this.energy,
				gainPoint = this.gainPoint,
				level = this.level,
				upgradeCount = this.upgradeCount,
				upgradePrice = this.upgradePrice,
				usedTurn = this.usedTurn,
				usedNumber = this.usedNumber,
				cooldownable = this.cooldownable,
				visible = this.visible,
				gearType = this.gearType
			};
		}
		
		public BoughtItem GetRandomItem(){
			switch (upgradeCount) {
			case 4:
				var item = GearUpgradedItems.GetIn (GearUpgradedItems.Case1 ());
				item.BuyAndUseImmediately (own, new[] { own });
				return item;
			case 9:
			case 10:
				item = GearUpgradedItems.GetIn (GearUpgradedItems.Case2 ());
				item.BuyAndUseImmediately (own, new[] { own });
				return item;
			default:
				break;
			}
			return null;
		}
	}
}

