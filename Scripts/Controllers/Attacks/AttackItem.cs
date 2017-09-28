using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Mob
{
	public class AttackItem : MobBehaviour
	{
		public Text title;
		public Text brief;
		public Text useTxt;
		public Image icon;
		public Button useBtn;

		public SyncItem skill;

		Race _character;
		SkillModule _skillModule;

		void Start(){
			useBtn.onClick.AddListener (() => {
				var opponentNetIds = Race.GetOpponentCharacterNetIds();
				skill.targetNetIds = opponentNetIds;
				skill.ownNetId = Race.GetLocalCharacter().netId.Value;
				_skillModule.CmdUse(skill);
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
			useTxt.text = skill.cooldownable ? "Cooldown" : "Use";
			useBtn.interactable = skill.interactable;
			title.text = skill.title + "";
			brief.text = skill.brief;
			icon.sprite = IconHelper.instance.GetIcon (skill.icon); //skill.GetIcon("default") ?? skill.GetIcon("none") ?? null;
		}
	}
}

