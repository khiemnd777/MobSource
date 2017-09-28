using System;
using System.Linq;
using UnityEngine.Networking;

namespace Mob
{
	public struct SyncGearBoughtItem {
		public int id;
		public int ownId;
		public string title;
		public string brief;
		public float price;
		public int quantity;
		public string icon;
		public bool interactable;
		public bool visible;
		public GearType gearType;
		public InStoreState inStoreState;
	}

	public class SyncListGearBoughtItem : SyncListStruct<SyncGearBoughtItem> { }

	public abstract class GearBoughtItem : BoughtItem
	{
		public GearType gearType;
		public InStoreState inStoreState;

		public void AlternateInStoreState(){
			own.GetModule<GearModule> (gear => {
				foreach (var item in gear.availableGears) {
					if (item.gearType != gearType)
						continue;
					item.inStoreState = InStoreState.Available;
				}
			});
			inStoreState = InStoreState.Bought;
		}

		public virtual SyncGearBoughtItem ToSyncGearBoughtItem(){
			return new SyncGearBoughtItem {
				brief = this.brief,
				icon = GetSyncIcon (),
				id = GetInstanceID(),
				ownId = this.own.GetInstanceID(),
				price = this.price,
				quantity = this.quantity,
				title = this.title,
				interactable = this.Interact(),
				visible = this.visible,
				gearType = this.gearType,
				inStoreState = this.inStoreState
			};
		}
	}
}

