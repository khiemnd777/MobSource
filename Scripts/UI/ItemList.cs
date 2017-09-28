//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//using System.Collections.Generic;
//
//namespace Mob
//{
//	public class ItemList : MobBehaviour
//	{
//		public Item[] items;
//		public Button[] children;
//		public RectTransform scrollPanel;
//		public RectTransform listItem;
//
//		public void Clear(){
//			if (scrollPanel == null)
//				return;
//			children = scrollPanel.GetComponentsInChildren<Button> ();
//			foreach (var child in children) {
//				Destroy (child.gameObject);
//			}
//			children = new Button[0];
//		}
//
//		public void SetItems(Item[] items){
//			Clear ();
//			this.items = items;
//			foreach (var item in this.items) {
//				var name = "(" + item.quantity + ") " + item.title;
//				var li = Instantiate (listItem, scrollPanel.transform);
//				var text = li.GetComponentInChildren<Text> ();
//				text.text = name;
//				var btn = li.GetComponent<Button> ();
//				btn.interactable = item.EnoughEnergy () && item.EnoughLevel () && item.EnoughCooldown ();
//				btn.onClick.AddListener(() => {
//					BattleController.playerInTurn.GetModule<BagModule>(x => {
//						x.Use(item, BattleController.GetTargets());
//						SetItems(x.items.ToArray());
//					});
//				});
//			}
//		}
//	}
//}
//
