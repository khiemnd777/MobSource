using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mob 
{
	public class LeftPanelController : MobBehaviour {
		public Button statsGroupBtn;
		public Button shopGroupBtn;
		public Button skillGroupBtn;
		public Button gearGroupBtn;

		public RectTransform statsGroup;
		public RectTransform shopGroup;
		public RectTransform skillGroup;
		public RectTransform gearGroup;

		bool shownStatGroup;
		bool shownShopGroup;
		bool shownSkillGroup;
		bool shownGearGroup;

		const string selectedBtn = "Sprites/gear-equipment => gear_upgrade_btn";
		const string normalBtn = "Sprites/gear-equipment => gear_buy_btn";

		void Init(){
			shopGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			skillGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			statsGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
			shownStatGroup = true;
			ShowGroup (statsGroup);
		}

		void Start(){
			HideAllGroups ();
			Init ();

			EventManager.StartListening ("gear-item-selected", new Action (() => {
				ShowGearGroup();
			}));

			gearGroupBtn.onClick.AddListener (() => {
				ShowGearGroup();
			});

			statsGroupBtn.onClick.AddListener (() => {
				shownShopGroup = false;
				shownSkillGroup = false;
				shownGearGroup = false;

				shopGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				skillGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				gearGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

				if(shownStatGroup)
					return;
				shownStatGroup = true;
				statsGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
				ShowGroup(statsGroup);
			});
			shopGroupBtn.onClick.AddListener (() => {
				shownStatGroup = false;
				shownSkillGroup = false;
				shownGearGroup = false;

				statsGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				skillGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				gearGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

				if(shownShopGroup)
					return;
				shownShopGroup = true;
				shopGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
				ShowGroup(shopGroup);
			});
			skillGroupBtn.onClick.AddListener (() => {
				shownStatGroup = false;
				shownShopGroup = false;
				shownGearGroup = false;

				gearGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				statsGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
				shopGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

				if(shownSkillGroup)
					return;
				shownSkillGroup = true;
				skillGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
				ShowGroup(skillGroup);
			});
		}

		void ShowGearGroup(){
			shownShopGroup = false;
			shownSkillGroup = false;
			shownStatGroup = false;

			statsGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			skillGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);
			shopGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(normalBtn);

			if(shownGearGroup)
				return;
			
			shownGearGroup = true;
			gearGroupBtn.GetComponent<Image>().sprite = IconHelper.instance.GetIcon(selectedBtn);
			ShowGroup(gearGroup);
		}

		void ShowGroup(RectTransform rt){
			rt.transform.SetAsLastSibling();
			var height = rt.parent.GetComponent<RectTransform>().rect.height;
			rt.SetPositionOfPivot (new Vector2(0f, -height));
			MathfLerp(-height, 0f, r => {
				rt.SetPositionOfPivot (new Vector2(0f, r));	
			}, 0.25f);
		}

		void HideAllGroups() {
			statsGroup.SetPositionOfPivot (new Vector2(0f, -statsGroup.parent.GetComponent<RectTransform>().rect.height));
			shopGroup.SetPositionOfPivot (new Vector2(0f, -shopGroup.parent.GetComponent<RectTransform>().rect.height));
			skillGroup.SetPositionOfPivot (new Vector2(0f, -skillGroup.parent.GetComponent<RectTransform>().rect.height));
		}
	}
}