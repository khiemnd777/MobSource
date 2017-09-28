using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class TabViewContainer : MobBehaviour 
	{
		RectTransform _rect;

		void Start(){
			_rect = GetComponent<RectTransform> ();
		}

		void Update(){
			FitSize ();
		}

		void FitSize(){
			var items = GetItems ();
			var index = 0;
			TabItem prevItem = null;
			foreach (var item in items) {
				item.transform.position = Vector3.zero;
				var itemRect = item.GetComponent<RectTransform> ();
				var itemWidth = itemRect.rect.width;
				itemRect.SetHeight (_rect.GetHeight ());
				if (index == 0) {
					item.transform.position = new Vector3 (_rect.position.x - (_rect.GetWidth() * _rect.pivot.x) + (itemRect.GetWidth() * itemRect.pivot.x), _rect.position.y, 0f);
				}
				if (prevItem != null) {
					var nextPosition = prevItem.transform.position + new Vector3 (itemWidth, 0f, 0f);
					item.transform.position = nextPosition;
				}
				prevItem = item;
				++index;
			}
		}

		public void ClearAll(){
			foreach (var item in GetItems()) {
				DestroyImmediate (item.gameObject);
			}
			(_rect ?? (_rect = GetComponent<RectTransform>())).SetHeight(0f);
		}

		public TabItem[] GetItems(){
			return GetComponentsInChildren<TabItem> (false);
		}

		public void Refresh(){
			FitSize ();
		}
	}	
}