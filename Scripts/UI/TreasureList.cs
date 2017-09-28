//using UnityEngine;
//using UnityEngine.UI;
//
//namespace Mob
//{
//	public class TreasureList : MobBehaviour
//	{
//		public BoughtItem[] items;
//		public Button[] children;
//		public RectTransform scrollPanel;
//		public RectTransform listItem;
//		public ItemList itemList;
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
//		public void SetItems(BoughtItem[] items){
//			Clear ();
//			this.items = items;
//			foreach (var item in this.items) {
//				var name = "(" + item.quantity + ") " + item.title;
//				var li = Instantiate (listItem, scrollPanel.transform);
//				var text = li.GetComponentInChildren<Text> ();
//				text.text = name;
//				var btn = li.GetComponent<Button> ();
//				btn.onClick.AddListener(() => {
//					item.Buy(BattleController.playerInTurn, 0f, 1);
//					BattleController.playerInTurn.GetModule<BagModule> (x => itemList.SetItems (x.items.ToArray()));
//					gameObject.SetActive (false);
//					Clear();
//				});
//			}
//		}
//	}
//}
//
