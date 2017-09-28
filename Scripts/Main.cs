//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//
//namespace Mob
//{
//	public class Main : MobBehaviour 
//	{
//		public SkillList skillList;
//		public ItemList itemList;
//		public TreasureList treasureList;
//		public NegativeAffectList negativeAffectList;
//		public Text strengthValue;
//		public Text dexterityValue;
//		public Text intelligentValue;
//		public Text vitalityValue;
//		public Text luckValue;
//		public Text hpValue;
//		public Text levelValue;
//		public Text gainPointValue;
//		public Text goldValue;
//		public Text energyValue;
//		public Text goldDiceValue;
//		public Text energyDiceValue;
//		public Text targetHpValue;
//		public Text targetLevelValue;
//		public Text countdownValue;
//		public Text pointValue;
//		public Button rollDice;
//		public Button endTurn;
//		public Button attackBtn;
//		public Button skillTreeBtn;
//		public Button addStrength;
//		public Button addDexterity;
//		public Button addIntelligent; 
//		public Button addVitality;
//		public Button addLuck;
//
//		CountdownModule cdm;
//		
//		void Start () {
//			cdm = GetComponent<CountdownModule> ();
//			cdm.RefreshAndRun ();
//
//			BattleController.Init ();
//			foreach (var player in BattleController.players) {
//				player.GetModule<HealthPowerModule> ();
//			}
//			BattleController.playerInTurn.GetModule<BagModule> (x => itemList.SetItems (x.items.ToArray()));
//			skillList.gameObject.SetActive (false);
//			treasureList.gameObject.SetActive (false);
//
//			// roll two dices
//			rollDice.onClick.AddListener(() =>{
//				var goldDice = Random.Range(1, 10);
//				var energyDice = Random.Range(1, 10);
//				goldDiceValue.text = goldDice.ToString();
//				energyDiceValue.text = energyDice.ToString();
//				BattleController.playerInTurn.GetModule<GoldModule>(x => x.AddGold(goldDice));
//				BattleController.playerInTurn.GetModule<EnergyModule>(x => x.AddEnergy(energyDice));
//				BattleController.playerInTurn.GetModule<SkillModule> (x => skillList.SetSkills (x.skills.ToArray()));
//				rollDice.interactable = false;
//
//				JumpEffect (goldValue.transform, Vector3.one);
//				ShowSubLabel (Constants.INCREASE_LABEL, goldValue.transform, goldDice * 10f);
//
//				JumpEffect (energyValue.transform, Vector3.one);
//				ShowSubLabel (Constants.INCREASE_LABEL, energyValue.transform, energyDice);
//			});
//
//			endTurn.onClick.AddListener (() => {
//				EndTurn();
//			});
//
//			attackBtn.onClick.AddListener (() => {
//				VisibleSkillList(!skillList.isActiveAndEnabled);
//				attackBtn.GetComponentInChildren<Text>().text = skillList.isActiveAndEnabled ? "Cancel attack" : "Attack";
//			});
//
//			skillTreeBtn.onClick.AddListener (() => {
//				BattleController.playerInTurn.GetModule<SkillModule>(x => x.OpenSkillTreeUI());
//			});
//
//			// stat buttons
//			addStrength.gameObject.SetActive(false);
//			addDexterity.gameObject.SetActive(false);
//			addIntelligent.gameObject.SetActive(false);
//			addVitality.gameObject.SetActive(false);
//			addLuck.gameObject.SetActive(false);
//
//			addStrength.onClick.AddListener(() => {
//				AddPoint(StatType.Strength);
//			});
//			addDexterity.onClick.AddListener(() => {
//				AddPoint(StatType.Dexterity);
//			});
//			addIntelligent.onClick.AddListener(() => {
//				AddPoint(StatType.Intelligent);
//			});
//			addVitality.onClick.AddListener(() => {
//				AddPoint(StatType.Vitality);
//			});
//			addLuck.onClick.AddListener(() => {
//				AddPoint(StatType.Luck);
//			});
//
//			// register emitLevelUpEvent
//			BattleController.emitLevelUpEvent = (who, level, upLevel) => {
//				addStrength.gameObject.SetActive(true);
//				addDexterity.gameObject.SetActive(true);
//				addIntelligent.gameObject.SetActive(true);
//				addVitality.gameObject.SetActive(true);
//				addLuck.gameObject.SetActive(true);	
//			};
//
//			// register emitPickAvailableSkillEvent
//			BattleController.emitPickAvailableSkillEvent = (who) => {
//				BattleController.playerInTurn.GetModule<SkillModule> (x => skillList.SetSkills (x.skills.ToArray()));
//			};
//		}
//
//		void VisibleSkillList(bool visible){
//			if (visible) {
//				BattleController.playerInTurn.GetModule<SkillModule> (x => skillList.SetSkills (x.skills.ToArray()));
//			}
//			skillList.gameObject.SetActive (visible);
//		}
//
//		void Update(){
//			countdownValue.text = cdm.minutes + ":" + cdm.secondsString;
//
//			if (cdm.isEnd) {
//				EndTurn ();
//			}
//
//			if (BattleController.treasure != null && BattleController.treasure.Length > 0) {
//				treasureList.SetItems (BattleController.treasure);
//				BattleController.treasure = new BoughtItem[0];
//				treasureList.gameObject.SetActive (true);
//			}
//			BattleController.playerInTurn.GetModule<StatModule> (x => {
//				pointValue.text = x.point.ToString();
//				strengthValue.text = x.strength.ToString();
//				dexterityValue.text = x.dexterity.ToString();
//				intelligentValue.text = x.intelligent.ToString();
//				vitalityValue.text = x.vitality.ToString();
//				luckValue.text = x.luck.ToString();
//			});
//			BattleController.playerInTurn.GetModule<HealthPowerModule> (x => {
//				hpValue.text = Mathf.RoundToInt(x.hpEffect) + "/" + Mathf.RoundToInt(x.maxHpEffect);
//			});
//			BattleController.playerInTurn.GetModule<GoldModule> (x => {
//				goldValue.text = x.goldLabel.ToString();
//			});
//			BattleController.playerInTurn.GetModule<EnergyModule> (x => {
//				energyValue.text = x.energyLabel.ToString();
//			});
//			BattleController.playerInTurn.GetModule<LevelModule> (x => {
//				levelValue.text = x.level.ToString();
//				gainPointValue.text = Mathf.CeilToInt(BattleController.playerInTurn.gainPointLabel) + "/" + Mathf.RoundToInt(LevelCalculator.GetPointAt(x.level + 1));
//			});
//
//			BattleController.GetTargets()[0].GetModule<HealthPowerModule> (x => {
//				targetHpValue.text = Mathf.RoundToInt(x.hpEffect) + "/" + Mathf.RoundToInt(x.maxHpEffect);
//			});
//
//			BattleController.GetTargets()[0].GetModule<LevelModule> (x => {
//				targetLevelValue.text = x.level.ToString();
//			});
//
//			BattleController.playerInTurn.GetModule<AffectModule> (x => {
//				negativeAffectList.SetAffects(x.GetSubAffects<INegativeAffect>().Cast<Affect>().ToArray());
//			});
//
//			BattleController.playerInTurn.GetModule<StatModule> (x => {
//				addStrength.gameObject.SetActive(x.point > 0);
//				addDexterity.gameObject.SetActive(x.point > 0);
//				addIntelligent.gameObject.SetActive(x.point > 0);
//				addVitality.gameObject.SetActive(x.point > 0);
//				addLuck.gameObject.SetActive(x.point > 0);		
//			});
//		}
//
//		void AddPoint(StatType statType){
//			BattleController.playerInTurn.GetModule<StatModule> (x => x.AddPoint (statType));
//		}
//
//		void EndTurn(){
//			BattleController.EndTurn ();
//			goldDiceValue.text = "0";
//			energyDiceValue.text = "0";
//			VisibleSkillList(false);
//			BattleController.playerInTurn.GetModule<BagModule> (x => itemList.SetItems (x.items.ToArray()));
//			attackBtn.GetComponentInChildren<Text>().text = "Attack";
//			treasureList.Clear ();
//			treasureList.gameObject.SetActive (false);
//			cdm.RefreshAndRun ();
//			rollDice.interactable = true;
//		}
//	}	
//}