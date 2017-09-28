using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Mob
{
	public struct SyncBoughtItem {
		public int id;
		public int ownId;
		public string title;
		public string brief;
		public float price;
		public int quantity;
		public string icon;
		public bool interactable;
		public bool visible;
	}

	public class SyncListBoughtItem : SyncListStruct<SyncBoughtItem> { }

	public abstract class BoughtItem : MobBehaviour
	{
		public Race own;

		public Icon icon = new Icon();

		public int quantity = 1;

		public float price = 0f;

		string _title;

		public string title { get { return _title ?? this.name; } set { _title = value; } }

		public string brief;

		public Type effectType;

		public virtual void Init(){
			
		}

		public virtual void Buy(Race who, float price = 0f, int quantity = 0){
			
		}

		public virtual void BuyAndUseImmediately(Race who, Race[] targets, float price = 0f){

		}

		public virtual void Pick(Race who, int quantity = 0){
			
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
			var p = price <= 0f ? this.price : price;
			var q = quantity <= 0 ? this.quantity : quantity;
			who.GetModule<GoldModule> ((g) => {
				g.SubtractGold(p * q);
			});
		}

		public bool EnoughGold(Race who, float price = 0f, int quantity = 1, Action predicate = null){
			var enough = false;
			var p = price < 0f ? this.price : price;
			var q = quantity < 0 ? this.quantity : quantity;

			who.GetModule<GoldModule> ((g) => {
				enough = g.gold >= p * q;
			});
			if (enough && predicate != null) {
				predicate.Invoke ();
			}
			return enough;
		}

		public void Buy<T>(Race who, float price = 0f, int quantity = 0, Action<T> predicate = null, Action postBuying = null) where T: Item{
			EnoughGold (who, price, quantity, () => {
				who.GetModule<BagModule>(i => i.Add<T>(quantity, predicate));
				SubtractGold(who, price, quantity);
				if(postBuying != null)
					postBuying.Invoke();
			});
		}

		public void BuyAndUseImmediately<T>(Race who, Race[] targets, float price = 0f, Action<T> predicate = null, Action postBuying = null) where T: Item{
			EnoughGold (who, price, 1, () => {
				Item.CreatePrimitive<T>(who, 1, predicate: x => {
					if(predicate != null){
						predicate.Invoke(x);
					}
					x.Use(targets);
					x.FlushAll();
				});
				SubtractGold(who, price, 1);
				if(postBuying != null)
					postBuying.Invoke();
			});
			FlushAll ();
		}

		public virtual SyncBoughtItem ToSyncBoughtItem(){
			return new SyncBoughtItem {
				brief = this.brief,
				icon = GetSyncIcon (),
				id = GetInstanceID(),
				ownId = this.own.IsNull() ? default(int) : this.own.GetInstanceID(),
				price = this.price,
				quantity = this.quantity,
				title = this.title,
				interactable = this.Interact(),
				visible = this.visible
			};
		}

		public static T CreatePrimitive<T>(Action<T> predicate = null) where T: BoughtItem {
			var go = new GameObject (typeof(T).Name, typeof(T));
			var a = go.GetComponent<T> ();
			if (predicate != null) {
				predicate.Invoke (a);
			}
			a.Init ();
			a.StartCoroutine (a.Interacting (a.gameObject));

			return a;
		}

		public static T CreatePrimitiveWithOwn<T>(Race own, Action<T> predicate = null) where T: BoughtItem {
			var go = new GameObject (typeof(T).Name, typeof(T));
			var a = go.GetComponent<T> ();
			a.own = own;
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

