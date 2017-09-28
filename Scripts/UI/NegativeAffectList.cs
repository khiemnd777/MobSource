using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

namespace Mob
{
	public class NegativeAffectList : MobBehaviour
	{
		public Affect[] affects;
		public Button[] children;
		public RectTransform scrollPanel;
		public RectTransform listItem;

		public void Clear(){
			if (scrollPanel == null)
				return;
			children = scrollPanel.GetComponentsInChildren<Button> ();
			foreach (var child in children) {
				Destroy (child.gameObject);
			}
			children = new Button[0];
		}

		public void SetAffects(Affect[] affects){
			Clear ();
			this.affects = affects;
			foreach (var skill in this.affects) {
				var name = skill.title;
				var item = Instantiate (listItem, scrollPanel.transform);
				var text = item.GetComponentInChildren<Text> ();
				text.text = name;
			}
		}
	}
}

