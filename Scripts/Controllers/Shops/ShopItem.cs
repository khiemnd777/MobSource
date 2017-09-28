using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class ShopItem : MobBehaviour 
	{
		public Text title;
		public Text brief;
		public Text price;
		public Image icon;
		public Button buyBtn;

		public SyncBoughtItem boughtItem;

		Race _character;
		ShopModule _shopModule;

		void Start(){
			buyBtn.onClick.AddListener (() => {
				_shopModule.CmdBuy(boughtItem);
			});
		}

		void Update(){
			if (!TryToConnect())
				return;
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _shopModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if (_character == null)
					return false;
				_shopModule = _character.GetModule<ShopModule> ();
				return false;
			});
		}

		public void PrepareItem(){
			title.text = boughtItem.title;
			brief.text = boughtItem.brief;
			price.text = Mathf.Floor(boughtItem.price) + "p";
			icon.sprite = IconHelper.instance.GetIcon (boughtItem.icon);
		}
	}	
}