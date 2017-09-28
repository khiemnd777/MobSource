using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class GearListUpgradeItem : MobBehaviour
	{
		public GearType gearType;
		public Text titleText;
		public Text briefText;
		public Text priceText;
		public Text statusText;
		public Button upgradeBtn;
		public SyncGearItem item;
		public GearController gearController;

		Race _character;
		GearModule _gearModule;

		void Start(){
			upgradeBtn.onClick.AddListener (() => {
				_gearModule.CmdUpgrade(item);
			});
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_GEARS, new Action<uint>(ownNetId => {
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				Refresh();
			}));
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

		void Refresh(){
			if (!_gearModule.syncGear.Any (x => x.gearType == gearType && x.id == item.id))
				return;
			var gearItem = _gearModule.syncGear.FirstOrDefault (x => x.gearType == gearType && x.id == item.id);
			titleText.text = gearItem.title;
			briefText.text = gearItem.brief;
			priceText.text = Mathf.Floor(gearItem.upgradePrice) + "g";
		}

		public void Prepare(){
			titleText.text = item.title;
			briefText.text = item.brief;
			priceText.text = Mathf.Floor(item.upgradePrice) + "g";
		}
	}	
}