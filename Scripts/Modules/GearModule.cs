using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Mob
{
	public class GearModule : RaceModule
	{
		public List<GearBoughtItem> availableGears = new List<GearBoughtItem>();
		[SyncVar]
		public SyncListGearBoughtItem syncAvailableGears = new SyncListGearBoughtItem ();
		[SyncVar]
		public SyncListGearItem syncGear = new SyncListGearItem();
		public GearItem helm;
		public GearItem armor;
		public GearItem cloth;
		public GearItem weapon;
		public GearItem ring;
		public GearItem artifact;

		public float GetStat(string name) {
			var val = .0f;
			Affect.GetSubAffects<GearAffect> (_race, x => {
				
				var fi = x.GetType().GetField(name);
				if(fi != null){
					var v = fi.GetValue(x);
					val += (float) System.Convert.ChangeType(v, typeof(float));
				}
			});
			return val;
		}

		[Command]
		public void CmdGetStat(string name){
			var statVal = GetStat (name);
			RpcReturnStatValue (name, statVal);
		}

		[ClientRpc]
		void RpcReturnStatValue(string name, float statVal){
			EventManager.TriggerEvent (Constants.EVENT_RETURN_STAT_VALUE, new { name = name, statVal = statVal, ownNetId = _race.netId.Value });
		}

		public void AddAvailableGear<T>(Action<T> predicate = null) where T: GearBoughtItem {
			if (!availableGears.Any (x => x.GetType().IsEqual<T> ())) {
				availableGears.Add (GearBoughtItem.CreatePrimitiveWithOwn<T> (_race, predicate));
				RefreshSyncAvailableGears ();
				return;
			}
		}

		public GearBoughtItem[] GetAvailableGearBoughtItemsByType(GearType gearType){
			return availableGears
				.Where (x => x.gearType == gearType && x.inStoreState == InStoreState.Available)
				.ToArray ();
		}

		public SyncGearBoughtItem[] GetSyncAvailableGearsByType(GearType gearType){
			return syncAvailableGears
				.Where (x => x.gearType == gearType && x.inStoreState == InStoreState.Available)
				.ToArray();
		}

		public SyncGearItem[] GetOwnSyncItemByType (params GearType[] gearTypes){
			var ownList = new List<SyncGearItem> ();
			foreach (var gearType in gearTypes) {
				if (!syncGear.Any (x => x.gearType == gearType))
					continue;
				var gear = syncGear.FirstOrDefault (x => x.gearType == gearType);
				ownList.Add (gear);
			}
			return ownList.ToArray();
		}

		public GearItem[] GetOwnItemsByType(params GearType[] gearTypes){
			var ownList = new List<GearItem> ();
			foreach (var gearType in gearTypes) {
				switch (gearType) {
				case GearType.Armor:
					ownList.Add (armor);
					break;
				case GearType.Artifact:
					ownList.Add (artifact);
					break;
				case GearType.Cloth:
					ownList.Add (cloth);
					break;
				case GearType.Ring:
					ownList.Add (ring);
					break;
				case GearType.Helm:
					ownList.Add (helm);
					break;
				case GearType.Weapon:
					ownList.Add (weapon);
					break;
				default:
					break;
				}
			}
			return ownList.Where(x => x != null).ToArray();
		}

		public object[] GetGearItemsByType(GearType gearType){
			var boughtItems = GetAvailableGearBoughtItemsByType (gearType);
			var ownItems = GetOwnItemsByType (gearType);
			var list = new List<object> ();
			list.AddRange (ownItems);
			list.AddRange (boughtItems);
			return list.ToArray ();
		}

//		public object[] GetCombinedSyncGearItemsByType(GearType gearType){
//			var boughtItems = GetSyncAvailableGearsByType (gearType);
//			var ownItems = GetOwnSyncItemByType (gearType);
//			var list = new List<object> ();
//			list.AddRange (ownItems);
//			list.AddRange (boughtItems);
//			return list.ToArray ();	
//		}

		void RefreshSyncGear(){
			syncGear.Clear ();

			if (helm != null && !syncGear.Any (x => x.id == helm.GetInstanceID ())) {
				syncGear.Add (helm.ToSyncGearItem ());
			}

			if (armor != null && !syncGear.Any (x => x.id == armor.GetInstanceID ())) {
				syncGear.Add (armor.ToSyncGearItem ());
			}

			if (cloth != null && !syncGear.Any (x => x.id == cloth.GetInstanceID ())) {
				syncGear.Add (cloth.ToSyncGearItem ());
			}

			if (weapon != null && !syncGear.Any (x => x.id == weapon.GetInstanceID ())) {
				syncGear.Add (weapon.ToSyncGearItem ());
			}

			if (ring != null && !syncGear.Any (x => x.id == ring.GetInstanceID ())) {
				syncGear.Add (ring.ToSyncGearItem ());
			}

			if (artifact != null && !syncGear.Any (x => x.id == artifact.GetInstanceID ())) {
				syncGear.Add (artifact.ToSyncGearItem ());
			}

			RpcReturnStatValue ("damage", GetStat ("damage"));
			RpcReturnStatValue ("magicAttack", GetStat ("magicAttack"));
			RpcReturnStatValue ("defend", GetStat ("defend"));
			RpcReturnStatValue ("magicResist", GetStat ("magicResist"));
			RpcReturnStatValue ("hp", GetStat ("hp"));
			RpcReturnStatValue ("point", GetStat ("point"));

			RpcRefreshSyncGear ();
		}

		[ClientRpc]
		void RpcRefreshSyncGear(){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_GEARS, new { ownNetId = _race.netId.Value });
		}

		void RefreshSyncAvailableGears(){
			var removedItems = new List<SyncGearBoughtItem> ();
			foreach (var syncObj in syncAvailableGears) {
				if (availableGears.Any (x => x.GetInstanceID () == syncObj.id))
					continue;
				removedItems.Add (syncObj);
			}

			foreach (var syncObj in removedItems) {
				syncAvailableGears.RemoveAt (syncAvailableGears.IndexOf(syncObj));
			}

			foreach (var item in availableGears) {
				var syncItem = item.ToSyncGearBoughtItem ();
				if(!syncAvailableGears.Any(x => x.id == item.GetInstanceID())) {
					syncAvailableGears.Add(syncItem);
					continue;
				}
				var syncObj = syncAvailableGears.FirstOrDefault (x => x.id == item.GetInstanceID ());
				var syncObjIndex = syncAvailableGears.IndexOf (syncObj);
				if(!object.ReferenceEquals(syncAvailableGears[syncObjIndex], syncItem)){
					syncAvailableGears[syncObjIndex] = syncItem;	
				}
			}

			RpcRefreshSyncAvaibleGears ();
		}

		[ClientRpc]
		void RpcRefreshSyncAvaibleGears(){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_AVAILABLE_GEARS, new {ownNetId = _race.netId.Value});
		}

		[ClientRpc]
		void RpcRefreshSyncAvaibleGearsByType(GearType gearType){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_AVAILABLE_GEARS_BY_TYPE, new {gearType = gearType, ownNetId = _race.netId.Value});
		}

		public bool HasByType(GearType gearType){
			switch (gearType) {
			case GearType.Armor:
				return armor != null;
			case GearType.Artifact:
				return artifact != null;
			case GearType.Cloth:
				return cloth != null;
			case GearType.Helm:
				return helm != null;
			case GearType.Ring:
				return ring != null;
			case GearType.Weapon:
				return weapon != null;
			default:
				return false;
			}
		}

		public void Buy(GearBoughtItem boughtItem){
			boughtItem.Buy(_race, quantity: 1);
			RefreshSyncAvailableGears ();
			RefreshSyncGear ();
			RpcBought (boughtItem.gearType);
		}

		[ClientRpc]
		void RpcBought(GearType gearType){
			EventManager.TriggerEvent (Constants.EVENT_BOUGHT_GEAR, new { gearType = gearType, ownNetId = _race.netId.Value });
		}

		[Command]
		public void CmdBuy(SyncGearBoughtItem syncBoughtItem){
			var availableGear = availableGears.FirstOrDefault (x => x.GetInstanceID () == syncBoughtItem.id);
			if (availableGear == null)
				return;
			Buy (availableGear);
		}

		public void Upgrade(GearItem item){
			item.Upgrade ();
			RefreshSyncGear ();
			RpcUpgraded (item.gearType);
		}

		[ClientRpc]
		void RpcUpgraded(GearType gearType){
			EventManager.TriggerEvent (Constants.EVENT_UPGRADED_GEAR, new { gearType = gearType, ownNetId = _race.netId.Value });
		}

		[Command]
		public void CmdUpgrade(SyncGearItem syncItem){
			if (helm != null && syncItem.id == helm.GetInstanceID ()) {
				Upgrade (helm);
				return;
			}
			if (armor != null && syncItem.id == armor.GetInstanceID ()) {
				Upgrade (armor);
				return;
			}
			if (cloth != null && syncItem.id == cloth.GetInstanceID ()) {
				Upgrade (cloth);
				return;
			}
			if (weapon != null && syncItem.id == weapon.GetInstanceID ()) {
				Upgrade (weapon);
				return;
			}
			if (artifact != null && syncItem.id == artifact.GetInstanceID ()) {
				Upgrade (artifact);
				return;
			}
			if (ring != null && syncItem.id == ring.GetInstanceID ()) {
				Upgrade (ring);
				return;
			}
		}
	}
}

