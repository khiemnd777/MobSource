using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Mob
{
	public class SkillController : MobBehaviour
	{
		public ScrollableList list;
		public SkillItem skillItemResource;

		Race _character;
		SkillModule _skillModule;

		void Start(){
			list.ClearAll ();
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_AVAILABLE_SKILLS, new Action<uint> (ownNetId =>{
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				RefreshItems();
			}));
		}

		void Update(){
			if (!TryToConnect())
				return;

			CreateItems ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _skillModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if (_character == null)
					return false;
				_skillModule = _character.GetModule<SkillModule> ();
				return false;
			});
		}

		bool isCreateItems;
		void CreateItems(){
			if (!isCreateItems) {
				isCreateItems = true;
				foreach (var item in _skillModule.syncAvailableSkills) {
					PrepareItems (item);
				}
				list.Refresh ();
			}
		}

		void RefreshItems(){
			var itemUIs = list.GetItems ().Select (x => x.GetComponent<SkillItem> ()).ToArray();
			foreach (var item in itemUIs) {
				if (!_skillModule.syncAvailableSkills.Any (x => item.boughtItem.id == x.id)) {
					DestroyImmediate (item.gameObject);
					list.Refresh ();
				}
			}
			foreach (var item in _skillModule.syncAvailableSkills) {
				if(!itemUIs.Any(x => item.id == x.boughtItem.id)){
					PrepareItems (item);
					list.Refresh ();
					continue;
				}
				var itemUI = itemUIs.FirstOrDefault (x => item.id == x.boughtItem.id);
				if (!object.ReferenceEquals (itemUI.boughtItem, item)) {
					itemUI.boughtItem = item;
					itemUI.PrepareItem ();
				}
			}
		}

		void PrepareItems(SyncSkillBoughtItem boughtItem){
			var ui = Instantiate<SkillItem> (skillItemResource, list.transform);
			ui.boughtItem = boughtItem;
			ui.PrepareItem ();
		}
	}
}