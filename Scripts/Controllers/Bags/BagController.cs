using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class BagController : MobBehaviour
	{
		public ScalableGridView gridView;
		public BagItem bagItemResource;

		Race _character;
		BagModule _bagModule;

		void Start(){
			gridView.ClearAll ();
			EventManager.StartListening(Constants.EVENT_REFRESH_SYNC_BAG_ITEMS, new Action<uint>(ownNetId => {
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

		void RefreshItems(){
			var itemUIs = gridView.GetItems ().Select (x => x.GetComponent<BagItem> ()).ToArray();
			foreach (var item in itemUIs) {
				if (!_bagModule.syncItems.Any (x => item.item.id == x.id && x.visible)) {
					DestroyImmediate (item.gameObject);
				}
			}
			foreach (var item in _bagModule.syncItems) {
				if (!item.visible)
					continue;
				if(!itemUIs.Any(x => item.id == x.item.id)){
					var ui = Instantiate<BagItem> (bagItemResource, gridView.transform);
					ui.item = item;	
					ui.Prepare ();
					continue;
				}
				var itemUI = itemUIs.FirstOrDefault (x => item.id  == x.item .id);
				if (!object.ReferenceEquals (itemUI.item, item)) {
					itemUI.item = item;
					itemUI.Prepare ();
				}
			}
		}
	}
}