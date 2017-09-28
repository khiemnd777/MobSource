using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class Stat2ndHController : MobNetworkBehaviour
	{
		public ScalableGridView list;
		public Stat2ndHListItem stat2ndListItemResource;

		Race _character;

		void Start() {
			
			list.ClearAll ();
		}

		void Update(){
			if (!TryToConnect())
				return;
			
			CreateItems ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null)
					return true;
				_character = Race.GetLocalCharacter ();
				return false;
			});
		}

		bool isCreateItem;
		void CreateItems(){
			if (isCreateItem)
				return;
			isCreateItem = true;
			// Strength
			PrepareItem(Stat2ndType.PhysicalAttack);
			PrepareItem(Stat2ndType.PhysicalDefend);
			// Dexterity
			PrepareItem(Stat2ndType.AttackRating);
			PrepareItem(Stat2ndType.CriticalRating);
			// Intelligent
			PrepareItem(Stat2ndType.MagicAttack);
			PrepareItem(Stat2ndType.MagicResist);
			// Vitality
			PrepareItem(Stat2ndType.MaxHp);
			PrepareItem(Stat2ndType.RegenerateHp);
			// Luck
			PrepareItem(Stat2ndType.LuckDice);
			PrepareItem(Stat2ndType.LuckReward);
		}

		void PrepareItem(Stat2ndType stat2ndType){
			var ui = Instantiate<Stat2ndHListItem> (stat2ndListItemResource, list.transform);
			ui.stat2ndType = stat2ndType;
		}
	}
}

