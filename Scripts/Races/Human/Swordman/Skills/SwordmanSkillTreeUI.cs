//using UnityEngine;
//using UnityEngine.UI;
//
//namespace Mob
//{
//	public class SwordmanSkillTreeUI : SkillTreeUI
//	{
//		public Button A1;
//		public Button A2;
//		public Button B1;
//		public Button B2;
//		public Button B3;
//		public Button C1;
//		public Button C2;
//		public Button D1;
//		public Button E1;
//		public Text evolvedPointValue;
//
//		void Start(){
//			A1.interactable = false;
//			B1.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanB1BoughtSkill>());
//				B1.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//			B2.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanB2BoughtSkill>());
//				B2.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//			B3.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanB3BoughtSkill>());
//				B3.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//			C1.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanC1BoughtSkill>());
//				C1.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//			C2.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanC2BoughtSkill>());
//				C2.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//			D1.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanD1BoughtSkill>());
//				D1.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//			E1.onClick.AddListener (() => {
//				own.GetModule<SkillModule>(x => x.PickAvailableSkill<SwordmanE1BoughtSkill>());
//				E1.interactable = false;
//				BattleController.EmitPickAvailableSkillEvent(own);
//			});
//		}
//
//		void Update(){
//			own.GetModule<SkillModule> (s => {
//				evolvedPointValue.text = s.evolvedSkillPoint.ToString();
//				own.GetModule<LevelModule> (x => {
//					B1.interactable = x.level >= 4 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanB1BoughtSkill>().enabled;
//					B1.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanB1Skill>() != null ? "B1√" : "B1";
//					B2.interactable = x.level >= 4 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanB2BoughtSkill>().enabled;
//					B2.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanB2Skill>() != null ? "B2√" : "B2";
//					B3.interactable = x.level >= 4 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanB3BoughtSkill>().enabled;
//					B3.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanB3Skill>() != null ? "B3√" : "B3";
//					C1.interactable = x.level >= 8 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanC1BoughtSkill>().enabled;
//					C1.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanC1Skill>() != null ? "C1√" : "C1";
//					C2.interactable = x.level >= 8 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanC2BoughtSkill>().enabled;
//					C2.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanC2Skill>() != null ? "C2√" : "C2";
//					D1.interactable = x.level >= 12 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanD1BoughtSkill>().enabled;
//					D1.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanD1Skill>() != null ? "D1√" : "D1";
//					E1.interactable = x.level == 16 && s.evolvedSkillPoint > 0 && s.GetAvailableSkill<SwordmanE1BoughtSkill>().enabled;
//					E1.GetComponentInChildren<Text>().text = s.GetSkill<SwordmanE1Skill>() != null ? "E1√" : "E1";
//				});	
//			});
//		}
//	}
//}
//
