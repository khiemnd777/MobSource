using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mob
{
	public class ShopController : MobBehaviour
	{
		public ScrollableList list;
		public ShopItem shopItemResource;

		Race _character;
		ShopModule _shopModule;

		void Start(){
			list.ClearAll ();
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_SHOP_ITEMS, new Action<uint>(ownNetId => {
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
				if (_character != null && _shopModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if (_character == null)
					return false;
				_shopModule = _character.GetModule<ShopModule> ();
				return false;
			});
		}

		bool isCreateItems;
		void CreateItems(){
			if (isCreateItems)
				return;
			isCreateItems = true;
			list.ClearAll ();
			foreach (var item in _shopModule.syncItems) {
				PrepareItem (item);
			}
			list.Refresh ();
		}

		void PrepareItem(SyncBoughtItem boughtItem){
			var ui = Instantiate<ShopItem> (shopItemResource, list.transform);
			ui.boughtItem = boughtItem;
			ui.PrepareItem ();
		}

		void RefreshItems(){
			var itemUIs = list.GetItems ().Select (x => x.GetComponent<ShopItem> ()).ToArray();
			foreach (var item in itemUIs) {
				if (!_shopModule.syncItems.Any (x => item.boughtItem.id == x.id && x.visible)) {
					DestroyImmediate (item.gameObject);
					list.Refresh ();
				}
			}
			foreach (var item in _shopModule.syncItems) {
				if (!item.visible)
					continue;
				if(!itemUIs.Any(x => item.id == x.boughtItem.id)){
					PrepareItem (item);
					list.Refresh ();
					continue;
				}
				var itemUI = itemUIs.FirstOrDefault (x => item.id  == x.boughtItem.id);
				if (!object.ReferenceEquals (itemUI.boughtItem, item)) {
					itemUI.boughtItem = item;
					itemUI.PrepareItem ();
				}
			}
		}
	}	
}