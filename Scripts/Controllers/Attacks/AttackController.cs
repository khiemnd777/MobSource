using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class AttackController : MobBehaviour
	{
		public ScrollableList list;
		public AttackItem skillItemResource;

		Race _character;
		SkillModule _skillModule;

		void Start(){
			list.ClearAll ();
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_SKILLS, new Action<uint>(ownNetId => {
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				PrepareItems();
			}));
		}

		void Update(){
			if (!TryToConnect())
				return;
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _skillModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if(_character == null)
					return false;
				_skillModule = _character.GetModule<SkillModule>();
				return false;
			});
		}

		void PrepareItems(){
			var itemUIs = list.GetItems ().Select (x => x.GetComponent<AttackItem> ()).ToArray();
			foreach (var item in itemUIs) {
				if (!_skillModule.syncSkills.Any (x => item.skill.id == x.id && x.visible)) {
					DestroyImmediate (item.gameObject);
					list.Refresh ();
				}
			}
			foreach (var item in _skillModule.syncSkills) {
				if (!item.visible)
					continue;
				if(!itemUIs.Any(x => item.id == x.skill.id)){
					PrepareItem (item);
					list.Refresh ();
					continue;
				}
				var itemUI = itemUIs.FirstOrDefault (x => item.id  == x.skill.id);
				if (!object.ReferenceEquals (itemUI.skill, item)) {
					itemUI.skill = item;
					itemUI.PrepareItem ();	
				}
			}
		}

		void PrepareItem(SyncItem skill){
			var ui = Instantiate<AttackItem> (skillItemResource, list.transform);
			ui.skill = skill;
			ui.PrepareItem ();
		}
	}
}