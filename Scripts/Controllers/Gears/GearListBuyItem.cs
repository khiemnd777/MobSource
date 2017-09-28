using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class GearListBuyItem : MobBehaviour
	{
		public GearType gearType;
		public Text titleText;
		public Text briefText;
		public Text priceText;
		public Text statusText;
		public Button buyBtn;
		public SyncGearBoughtItem boughtItem;
		public GearController gearController;

		Race _character;
		GearModule _gearModule;

		void Start(){
			buyBtn.onClick.AddListener (() => {
				_gearModule.CmdBuy(boughtItem);
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

		public void Prepare(){
			titleText.text = boughtItem.title;
			briefText.text = boughtItem.brief;
			priceText.text = Mathf.Floor(boughtItem.price) + "g";
		}
	}	
}