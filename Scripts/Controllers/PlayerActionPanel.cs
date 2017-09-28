using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class PlayerActionPanel : MobBehaviour
	{
		public Button attackBtn;
		public Button bagBtn;
		public Button cardBtn;

		public RectTransform attackGroup;
		public RectTransform bagGroup;
		public RectTransform cardGroup;

		bool shownAttack;
		bool shownBag;
		bool shownCard;

		Race _character;

		const string selectedBtn = "Sprites/gear-equipment => gear_upgrade_btn";
		const string normalBtn = "Sprites/gear-equipment => gear_buy_btn";

		void Start() {
			HideAllGroups ();

			attackBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
			bagBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			cardBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

			shownAttack = true;
			ShowGroup (attackGroup);

			attackBtn.onClick.AddListener (ShowAttack);
			bagBtn.onClick.AddListener (ShowBag);
			cardBtn.onClick.AddListener (() => {
				shownBag = false;
				shownAttack = false;

				bagBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				attackBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

				if(shownCard)
					return;
				shownCard = true;
				ShowGroup(cardGroup);
				cardBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
			});

			EventManager.StartListening (Constants.EVENT_BOUGHT_ITEM_FROM_SHOP, new Action<SyncBoughtItem, uint>((syncItem, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				ShowBag();
			}));
			EventManager.StartListening (Constants.EVENT_SKILL_PICKED, new Action<uint>(ownNetId => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				ShowAttack();
			}));
		}

		void Update(){
			if (!TryToConnect ())
				return;
		}

		void ShowBag(){
			shownAttack = false;
			shownCard = false;

			attackBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			cardBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

			if(shownBag)
				return;
			shownBag = true;
			ShowGroup(bagGroup);
			bagBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
		}

		void ShowAttack(){
			shownBag = false;
			shownCard = false;

			bagBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			cardBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

			if(shownAttack)
				return;
			shownAttack = true;
			ShowGroup(attackGroup);
			attackBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
		}

		void ShowGroup(RectTransform rt){
			rt.transform.SetAsLastSibling();
			var height = rt.parent.GetComponent<RectTransform>().rect.height;
			rt.SetPositionOfPivot (new Vector2(0f, height));
			MathfLerp(height, 0f, r => {
				rt.SetPositionOfPivot (new Vector2(0f, r));	
			}, 0.25f);
		}

		void HideAllGroups() {
			attackGroup.SetPositionOfPivot (new Vector2(0f, attackGroup.parent.GetComponent<RectTransform>().rect.height));
			bagGroup.SetPositionOfPivot (new Vector2(0f, bagGroup.parent.GetComponent<RectTransform>().rect.height));
			cardGroup.SetPositionOfPivot (new Vector2(0f, cardGroup.parent.GetComponent<RectTransform>().rect.height));
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (!_character.IsNull())
					return true;
				_character = Race.GetLocalCharacter ();
				return false;
			});
		}
	}
}

