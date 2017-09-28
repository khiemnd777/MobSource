using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class TouchOnGear : MobBehaviour
	{
		public GearType gearType;
		public GearController gearController;

		Button _btn;

		Race _character;
		GearModule _gearModule;

		void Start(){
			_btn = GetComponent<Button> ();
			_btn.onClick.AddListener (() => {
				Act();
			});
		}

		void Update(){
			if (!TryToConnect ())
				return;
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if(_character != null && _gearModule != null)
					return true;
				_character = Race.GetLocalCharacter();
				if(_character == null)
					return false;
				_gearModule = _character.GetModule<GearModule>();
				return false;
			});
		}

		void Act(){
			if (!TryToConnect ())
				return;
			gearController.FilterItemsByType (gearType);
			EventManager.TriggerEvent ("gear-item-selected");
		}
	}	
}