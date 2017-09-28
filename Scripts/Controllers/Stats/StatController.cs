using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mob
{
	public class StatController : MobBehaviour
	{
		public ScrollableList list;
		public StatListItem statListItemResource;

		Race _character;

		void Start() {
			list.ClearAll ();
		}

		void Update(){
			if (!NetworkHelper.instance.TryToConnect (() => {
				if (_character != null)
					return true;
				_character = Race.GetLocalCharacter ();
				return false;
			}))
				return;
			
			CreateItems ();	
		}

		bool isCreateItem;
		void CreateItems(){
			if (isCreateItem)
				return;
			
			isCreateItem = true;
			// Strength
			PrepareItem (StatType.Strength);
			// Dexterity
			PrepareItem (StatType.Dexterity);
			// Intelligent
			PrepareItem (StatType.Intelligent);
			// Vitality
			PrepareItem (StatType.Vitality);
			// Luck
			PrepareItem (StatType.Luck);

			list.Refresh ();
		}

		void PrepareItem(StatType statType){
			var ui = Instantiate<StatListItem> (statListItemResource, list.transform);
			ui.statType = statType;
		}
	}
}

