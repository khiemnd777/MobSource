using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
	public struct SyncSkillBoughtItem {
		public int id;
		public int ownId;
		public string title;
		public string brief;
		public float price;
		public int quantity;
		public string icon;
		public bool learned;
		public int learnedLevel;
		public float reducedEnergy;
		public int cooldown;
		public float gainPoint;
		public bool interactable;
		public bool visible;
	}

	public class SyncListSkillBoughtItem : SyncListStruct<SyncSkillBoughtItem> { }

	public class SkillBoughtItem : BoughtItem
	{
		public bool learned;
		public int learnedLevel;
		public float reducedEnergy;
		public int cooldown;
		public float gainPoint;

		public virtual SyncSkillBoughtItem ToSyncSkillBoughtItem(){
			return new SyncSkillBoughtItem {
				brief = this.brief,
				icon = GetSyncIcon (),
				id = GetInstanceID(),
				ownId = this.own.GetInstanceID(),
				price = this.price,
				quantity = this.quantity,
				title = this.title,
				learned = this.learned,
				learnedLevel = this.learnedLevel,
				reducedEnergy = this.reducedEnergy,
				cooldown = this.cooldown,
				gainPoint = this.gainPoint,
				interactable = this.Interact(),
				visible = this.visible
			};
		}
	}
}

