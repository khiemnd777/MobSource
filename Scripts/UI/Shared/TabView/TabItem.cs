using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class TabItem : MobBehaviour
	{
		public string key;
		public bool selected;
		public Button actionBtn;

		void Start(){
			actionBtn.onClick.AddListener (Select);
			EventManager.StartListening (Constants.EVENT_TAB_CONTENT_FIRED, new Action<string> (OnSelected));
		}

		void Select(){
			EventManager.TriggerEvent (Constants.EVENT_TAB_CONTENT_FIRED, new { key = this.key });
		}

		void OnSelected(string key){
			selected = this.key == key;
		}
	}
}

