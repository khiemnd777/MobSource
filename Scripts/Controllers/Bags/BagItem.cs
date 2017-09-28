using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class BagItem : MobBehaviour 
	{
		public Image icon;
		public Text quantity;
		public Button useBtn;
		public SyncItem item;

		Race _character;
		BagModule _bagModule;

		void Start(){
			useBtn.onClick.AddListener (() => {
				var opponentNetIds = Race.GetOpponentCharacterNetIds();
				item.targetNetIds = opponentNetIds;
				item.ownNetId = Race.GetLocalCharacter().netId.Value;
				_bagModule.CmdUse(item);
			});
		}

		public void Prepare(){
			useBtn.interactable = item.interactable;
			quantity.text = "x" + item.quantity;
			icon.sprite = IconHelper.instance.GetIcon (item.icon);
		}

		void Update(){
			if (!TryToConnect())
				return;
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _bagModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if (_character == null)
					return false;
				_bagModule = _character.GetModule<BagModule> ();
				return false;
			});
		}
	}	
}