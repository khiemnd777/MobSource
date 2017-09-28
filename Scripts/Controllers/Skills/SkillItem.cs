using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class SkillItem : MobBehaviour
	{
		public Text title;
		public Text brief;
		public Image icon;
		public Button learnBtn;
		public Text learnTxt;

		public SyncSkillBoughtItem boughtItem;

		Race _character;
		SkillModule _skillModule;

		void Start(){
			learnBtn.onClick.AddListener (() => {
				_skillModule.CmdPickAvailableSkill(boughtItem);
				EventManager.TriggerEvent(Constants.EVENT_SKILL_LEARNED);
			});
		}

		void Update(){
			if (!NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _skillModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if(_character == null)
					return false;
				_skillModule = _character.GetModule<SkillModule>();
				return false;
			}))
				return;
		}

		public void PrepareItem(){
			learnTxt.text = boughtItem.learned ? "Learned" : "Learn";
			learnBtn.interactable = boughtItem.interactable;
			title.text = boughtItem.title;
			brief.text = boughtItem.brief;
			icon.sprite = IconHelper.instance.GetIcon(boughtItem.icon);
		}
	}
}

