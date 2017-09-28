using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Mob
{	
	public struct SyncItem {
		public int id;
		public int ownId;
		public uint ownNetId;
		public int effectName;
		public int[] targetId;
		public uint[] targetNetIds;
		public string title;
		public int quantity;
		public string brief;
		public float energy;
		public float gainPoint;
		public int level;
		public int upgradeCount;
		public float upgradePrice;
		public int usedTurn;
		public int cooldown;
		public string icon;
		public int usedNumber;
		public bool interactable;
		public bool cooldownable;
		public bool visible;
	}

	public class SyncListItem : SyncListStruct<SyncItem> { }

	public abstract class Item : MobBehaviour
	{
		public Icon icon = new Icon();
		public int quantity;
		public Race own;
		public int usedTurn = 0;
		public int usedNumber = 0;
		public int upgradeCount;
		public float gainPoint = 0f;
		public float energy = 0f;
		public int level = 0;
		string _title;
		public string title { get { return _title ?? this.name; } set { _title = value; }}
		public string brief;
		public int cooldown = 0;
		public float upgradePrice = 0f;
		public bool cooldownable { get; private set; }
		public Type effectType;

		public virtual void Init(){
				
		}

		public virtual bool Interact(Race target){
			return true;
		}

		public abstract bool Use (Race[] targets);

		public virtual bool Disuse(){
			return false;	
		}

		public bool Use<T>(Race[] targets, Action<T> predicate = null) where T: Affect {
			Affect.CreatePrimitiveAndUse<T> (own, targets, predicate);
			SubtractEnergy ();
			return true;
		}

		public virtual bool Upgrade(float price = 0f){
			return false;
		}

		public virtual Sprite GetIcon(string key, Func<bool> predicate){
			return icon.GetIconFromPrefab(key, predicate);
		}

		public virtual Sprite GetIcon(string key){
			return icon.GetIconFromPrefab(key);
		}

		public virtual Sprite GetIcon(){
			return icon.GetIconFromPrefab();
		}

		public virtual string GetSyncIcon(){
			return icon.prefabs.Count == 0 ? null : icon.prefabs.FirstOrDefault().Value;
		}

		public void SubtractGold(Race who, float price = 0f, int quantity = 0){
			var p = price <= 0f ? this.upgradePrice : price;
			var q = quantity <= 0 ? this.quantity : quantity;
			who.GetModule<GoldModule> ((g) => {
				g.SubtractGold(p * q);
			});
		}

		public bool EnoughGold(Race who, float price = 0f, int quantity = 1, Action predicate = null){
			var enough = false;
			var p = price < 0f ? this.upgradePrice : price;
			var q = quantity < 0 ? this.quantity : quantity;

			who.GetModule<GoldModule> ((g) => {
				enough = g.gold >= p * q;
			});
			if (enough && predicate != null) {
				predicate.Invoke ();
			}
			return enough;
		}

		public bool EnoughLevel(Action predicate = null){
			var result = false;
			own.GetModule<LevelModule> (x => {
				result = x.level >= level;
			});
			if (result && predicate != null) {
				predicate.Invoke ();
			}
			return result;
		}

		public bool EnoughEnergy(Action predicate = null){
			var result = false;
			own.GetModule<EnergyModule> (x => {
				result = x.energy >= energy;
			});
			if (result && predicate != null) {
				predicate.Invoke ();
			}
			return result;
		}

		public bool EnoughCooldown(Action predicate = null){
			cooldownable = !(cooldown == 0 || usedTurn == 0 || usedTurn + cooldown == own.turnNumber);
			if (!cooldownable && predicate != null) {
				predicate.Invoke ();
			}
			return !cooldownable;
		}

		public void SubtractEnergy(float energy = 0f){
			own.GetModule<EnergyModule> ((e) => {
				e.SubtractEnergy (energy == 0f ? this.energy : energy);
			});
		}

		public virtual SyncItem ToSyncItem(){
			return new SyncItem {
				brief = this.brief,
				icon = GetSyncIcon (),
				id = GetInstanceID(),
				ownId = this.own.GetInstanceID(),
				title = this.title,
				interactable = this.Interact(),
				cooldown = this.cooldown,
				energy = this.energy,
				gainPoint = this.gainPoint,
				level = this.level,
				upgradeCount = this.upgradeCount,
				upgradePrice = this.upgradePrice,
				usedTurn = this.usedTurn,
				usedNumber = this.usedNumber,
				cooldownable = this.cooldownable,
				quantity = this.quantity,
				visible = this.visible
			};
		}

		public static T Create<T>(string resource, Race own, int quantity, Action<T> predicate = null) where T : Item {
			var a = GetMonoResource<T> (resource);
			return Create<T> (a, own, quantity, predicate);
		}

		public static T Create<T>(T item, Race own, int quantity, Action<T> predicate = null) where T : Item {
			var a = Instantiate<T>(item);
			a.own = own;
			a.quantity = quantity;
			if (predicate != null) {
				predicate.Invoke (a);
			}
			a.transform.SetParent (own.transform);
			a.Init ();
			a.StartCoroutine (a.Interacting (a.gameObject));

			return a;
		}

		public static T CreatePrimitive<T>(Race own, int quantity, Action<T> predicate = null) where T: Item {
			var go = new GameObject (typeof(T).Name, typeof(T));
			var a = go.GetComponent<T> ();
			a.own = own;
			a.quantity = quantity;
			if (predicate != null) {
				predicate.Invoke (a);
			}
			a.transform.SetParent (own.transform);
			a.Init ();
			a.StartCoroutine (a.Interacting (a.gameObject));

			return a;
		}
	}
}