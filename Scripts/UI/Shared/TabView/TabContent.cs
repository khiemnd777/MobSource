using System;
using UnityEngine;

namespace Mob
{
	public class TabContent : MobBehaviour
	{
		public string key;

		void Start(){
			EventManager.StartListening(Constants.EVENT_TAB_CONTENT_FIRED, new Action<string>(OnVisible));
		}

		void OnVisible(string key){
			gameObject.SetActive(this.key == key);
		}
	}
}