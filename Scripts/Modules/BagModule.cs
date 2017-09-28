using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
	public class BagModule : RaceModule
	{
		public List<Item> items = new List<Item>();
		[SyncVar]
		public SyncListItem syncItems = new SyncListItem();

		public override void Init ()
		{
			EventManager.StartListening (Constants.EVENT_ITEM_BOUGHT_FIRED, new Action<uint>(ownNetId =>{
				if (!_race.netId.Value.Equals (ownNetId))
					return;
				RefreshSyncItems();
			}));
		}

		public void Add<T>(int quantity, Action<T> predicate = null) where T: Item{
			if (!items.Any (x => x.GetType().IsEqual<T> ())) {
				var item = Item.CreatePrimitive<T> (_race, quantity, predicate);
				items.Add (item);
				return;
			}
			items.FirstOrDefault (x => x.GetType().IsEqual<T> ()).quantity += quantity;
		}

		void RefreshSyncItems(){
			var removedItems = new List<SyncItem> ();
			foreach (var syncObj in syncItems) {
				if (items.Any (x => x.GetInstanceID () == syncObj.id))
					continue;
				removedItems.Add (syncObj);
			}

			foreach (var syncObj in removedItems) {
				syncItems.RemoveAt (syncItems.IndexOf(syncObj));
			}

			foreach (var item in items) {
				var syncItem = item.ToSyncItem ();
				if(!syncItems.Any(x => x.id == item.GetInstanceID())) {
					syncItems.Add(syncItem);
					continue;
				}
				var syncObj = syncItems.FirstOrDefault (x => x.id == item.GetInstanceID ());
				var syncObjIndex = syncItems.IndexOf (syncObj);
				if (!object.ReferenceEquals (syncItems [syncObjIndex], syncItem)) {
					syncItems[syncObjIndex] = syncItem;	
				}
			}
			RpcRefreshSyncItemsCallback ();
		}

		[ClientRpc]
		void RpcRefreshSyncItemsCallback(){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_BAG_ITEMS, new { ownNetId = _race.netId.Value });
		}

		public void Use<T>(Race[] targets){
			if (!items.Any (x => x.GetType().IsEqual<T> ())) 
				return;
			var item = items.FirstOrDefault (x => x.GetType().IsEqual<T> ());
			Use (item, targets);
		}

		public void Use(Item item, Race[] targets){
			var t = targets;
			if (typeof(ISelfUsable).IsAssignableFrom (item.GetType ())) {
				t = new Race[]{ item.own };
			} 
//			else if (typeof(ITargetUsable).IsAssignableFrom (item.GetType ())) {
//				t = targets;
//			}

			if (item.Use (t)) {
				item.usedTurn = _race.turnNumber;
				--item.quantity;	
			}

			if (item.quantity == 0) {
				items.Remove (item);
				DestroyImmediate (item.gameObject);
//				EventManager.TriggerEvent (Constants.EVENT_ITEM_OVER_IN_BAG);
			}

			RefreshSyncItems ();
		}

		[Command]
		public void CmdUse(SyncItem syncItem){
			var item = items.FirstOrDefault (x => x.GetInstanceID() == syncItem.id);
			if (item == null)
				return;
			var opponents = Race.GetCharactersByNetIds (syncItem.targetNetIds);
			Use (item, opponents);
		}
	}
}