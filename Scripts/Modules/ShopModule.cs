using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
	public class ShopModule : RaceModule
	{
		public List<BoughtItem> items = new List<BoughtItem>();
		[SyncVar]
		public SyncListBoughtItem syncItems = new SyncListBoughtItem();

		public override void Init ()
		{
			items = AvailableItem.instance.shopItems;
			foreach (var item in items) {
				syncItems.Add (item.ToSyncBoughtItem ());
			}
		}

		public bool HasItem(BoughtItem boughtItem){
			return items.Any (x => x.GetType ().IsAssignableFrom (boughtItem.GetType ()));
		}

		public void Buy(BoughtItem boughtItem){
			if (!HasItem (boughtItem))
				return;
			boughtItem.Buy (_race, boughtItem.price, 1);
			RefeshSyncItems ();
			EventManager.TriggerEvent (Constants.EVENT_ITEM_BOUGHT_FIRED, new { ownNetId = _race.netId.Value });
		}

		[Command]
		public void CmdBuy(SyncBoughtItem syncBoughtItem){
			var item = items.FirstOrDefault (x => x.GetInstanceID () == syncBoughtItem.id);
			if (item == null)
				return;
			Buy (item);
			RpcBoughtItemCallback (syncBoughtItem);
		}

		[ClientRpc]
		void RpcBoughtItemCallback(SyncBoughtItem syncBoughtItem){
			var syncItem = syncItems.FirstOrDefault (x => x.id == syncBoughtItem.id);
			EventManager.TriggerEvent (Constants.EVENT_BOUGHT_ITEM_FROM_SHOP, new { syncItem = syncItem, ownNetId = _race.netId.Value });
		}

		void RefeshSyncItems(){
			var removedItems = new List<SyncBoughtItem> ();
			foreach (var syncObj in syncItems) {
				if (items.Any (x => x.GetInstanceID () == syncObj.id))
					continue;
				removedItems.Add (syncObj);
			}

			foreach (var syncObj in removedItems) {
				syncItems.RemoveAt (syncItems.IndexOf(syncObj));
			}

			foreach (var item in items) {
				var syncItem = item.ToSyncBoughtItem ();
				if(!syncItems.Any(x => x.id == item.GetInstanceID())) {
					syncItems.Add(syncItem);
					continue;
				}
				var syncObj = syncItems.FirstOrDefault (x => x.id == item.GetInstanceID ());
				var syncObjIndex = syncItems.IndexOf (syncObj);
				if(!object.ReferenceEquals(syncItems[syncObjIndex], syncItem)){
					syncItems[syncObjIndex] = syncItem;	
				}
			}
			RpcRefreshSyncItemCallback ();
		}

		[ClientRpc]
		void RpcRefreshSyncItemCallback(){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_SHOP_ITEMS, new { ownNetId = _race.netId.Value });
		}
	}
}