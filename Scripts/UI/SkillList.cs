//using UnityEngine;
//using UnityEngine.UI;
//using System.Linq;
//using System.Collections.Generic;
//
//namespace Mob
//{
//	public class SkillList : MobBehaviour
//	{
//		public Skill[] skills;
//		public Button[] children;
//		public RectTransform scrollPanel;
//		public RectTransform listItem;
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
//		public void SetSkills(Skill[] skills){
//			Clear ();
//			this.skills = skills;
//			foreach (var skill in this.skills) {
//				var name = skill.title;
//				var item = Instantiate (listItem, scrollPanel.transform);
//				var text = item.GetComponentInChildren<Text> ();
//				text.text = name;
//				var btn = item.GetComponent<Button> ();
//				btn.interactable = skill.EnoughEnergy () && skill.EnoughLevel () && skill.EnoughCooldown ();
//				btn.onClick.AddListener(() => {
//					BattleController.playerInTurn.GetModule<SkillModule>(x => {
//						x.Use(skill, BattleController.GetTargets());
//						SetSkills(x.skills.ToArray());
//					});
//				});
//			}
//		}
//	}
//}
//
