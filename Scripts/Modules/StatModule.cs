using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
	public enum StatType {
		Strength, Dexterity, Intelligent, Vitality, Luck
	}

	public enum Stat2ndType {
		PhysicalAttack, PhysicalDefend
		, AttackRating, CriticalRating
		, MagicAttack, MagicResist
		, MaxHp, RegenerateHp
		, LuckDice, LuckReward
	}
	
	public class StatModule : RaceModule
	{
		public int initPoint = 5;

		int logPoint;
		[Header("Gain point")]
		[SyncVar(hook="OnPointChanged")] 	
		public int point;

		void OnPointChanged(int currentPoint){
			EventManager.TriggerEvent (Constants.EVENT_STAT_POINT_CHANGED, new {point = currentPoint, ownNetId = _race.netId.Value });
		}

		[Header("Strength")]
		[SyncVar(hook="OnStrengthChanged")]	
		public float strength = 1f;
		public int addedStrength;

		void OnStrengthChanged(float currentStrength){
			EventManager.TriggerEvent (Constants.EVENT_STAT_STRENGTH_CHANGED, new { strength = currentStrength, ownNetId = _race.netId.Value });
		}

		[Header("Sub-strength")]
		[SyncVar(hook="OnPhysicalAttackChanged")]	
		public float physicalAttack;

		void OnPhysicalAttackChanged (float currentPhysicalAttack){
			EventManager.TriggerEvent (Constants.EVENT_STAT_PHYSICAL_ATTACK_CHANGED, new {physicalAttack = currentPhysicalAttack, ownNetId = _race.netId.Value});
		}

		public float extraPhysicalAttack;
		public float physicalAttackSeed = 1f;

		[SyncVar(hook="OnPhysicalDefendChanged")] 	
		public float physicalDefend;

		void OnPhysicalDefendChanged(float currentPhysicalDefend){
			EventManager.TriggerEvent (Constants.EVENT_STAT_PHYSICAL_DEFEND_CHANGED, new { physicalDefend = currentPhysicalDefend, ownNetId = _race.netId.Value });
		}

		public float extraPhysicalDefend;
		public float physicalDefendSeed = 1f;

		[Header("Dexterity")]
		[SyncVar(hook="OnDexterityChanged")] 	
		public float dexterity = 1f;
		public int addedDexterity;

		void OnDexterityChanged(float currentDexterity){
			EventManager.TriggerEvent (Constants.EVENT_STAT_DEXTERITY_CHANGED, new {dexterity = currentDexterity, ownNetId = _race.netId.Value });
		}

		[Header("Sub-dexterity")]
		[SyncVar(hook="OnAttackRatingChanged")] 	
		public float attackRating;

		void OnAttackRatingChanged(float currentAttackRating){
			EventManager.TriggerEvent (Constants.EVENT_STAT_ATTACK_RATING_CHANGED, new {attackRating = currentAttackRating, ownNetId = _race.netId.Value });
		}

		public float extraAttackRating;
		public float attackRatingSeed = 1.2f;

		[SyncVar(hook="OnCriticalRatingChanged")] 	
		public float criticalRating;

		void OnCriticalRatingChanged(float currentCriticalRating){
			EventManager.TriggerEvent (Constants.EVENT_STAT_CRITICAL_RATING_CHANGED, new {criticalRating = currentCriticalRating, ownNetId = _race.netId.Value });	
		}

		public float extraCriticalRating;
		public float criticalRatingSeed = 0.75f;

		[Header("Intelligent")]
		[SyncVar(hook="OnIntelligentChanged")] 	
		public float intelligent = 1f;
		public int addedIntelligent;

		void OnIntelligentChanged(float currentIntelligent){
			EventManager.TriggerEvent (Constants.EVENT_STAT_INTELLIGENT_CHANGED, new { intelligent = currentIntelligent, ownNetId = _race.netId.Value });
		}

		[Header("Sub-intelligent")]
		[SyncVar(hook="OnMagicAttackChanged")] 	
		public float magicAttack;

		void OnMagicAttackChanged(float currentMagicAttack){
			EventManager.TriggerEvent (Constants.EVENT_STAT_MAGIC_ATTACK_CHANGED, new { magicAttack = currentMagicAttack, ownNetId = _race.netId.Value });
		}

		public float extraMagicAttack;
		public float magicAttackSeed = 1.25f;

		[SyncVar(hook="OnMagicResistChanged")] 	
		public float magicResist;

		void OnMagicResistChanged(float currentMagicResist){
			EventManager.TriggerEvent (Constants.EVENT_STAT_MAGIC_RESIST_CHANGED, new { magicResist = currentMagicResist, ownNetId = _race.netId.Value });
		}

		public float extraMagicResist;
		public float magicResistSeed = 1.25f;

		[Header("Vitality")]
		[SyncVar(hook="OnVitalityChanged")] 	
		public float vitality = 1f;
		public int addedVitality;

		void OnVitalityChanged(float currentVitality){
			EventManager.TriggerEvent (Constants.EVENT_STAT_VITALITY_CHANGED, new { vitality = currentVitality, ownNetId = _race.netId.Value });	
		}

		[Header("Sub-vitality")]
		[SyncVar(hook="OnMaxHpChanged")]	
		public float maxHp;

		void OnMaxHpChanged(float currentMaxHp){
			EventManager.TriggerEvent (Constants.EVENT_STAT_MAX_HP_CHANGED, new { maxHp = currentMaxHp, ownNetId = _race.netId.Value });
		}

		public float extraMaxHp;
		public float maxHpSeed = 3f;

		[SyncVar(hook="OnRegenerateHpChanged")]	
		public float regenerateHp;

		void OnRegenerateHpChanged(float currentRegenerateHp){
			EventManager.TriggerEvent (Constants.EVENT_STAT_REGENERATE_HP_CHANGED, new {regenerateHp = currentRegenerateHp, ownNetId = _race.netId.Value });
		}

		public float extraRegenerateHp;
		public float regenerateHpSeed = 0.75f;

		[Header("Luck")]
		[SyncVar(hook="OnLuckChanged")]	
		public float luck = 1f;
		public int addedLuck;

		void OnLuckChanged(float currentLuck){
			EventManager.TriggerEvent (Constants.EVENT_STAT_LUCK_CHANGED, new { luck = currentLuck, ownNetId = _race.netId.Value });
		}

		[Header("Sub-luck")]
		[SyncVar(hook="OnLuckDiceChanged")]	
		public float luckDice;

		void OnLuckDiceChanged(float currentLuckDice){
			EventManager.TriggerEvent (Constants.EVENT_STAT_LUCK_DICE_CHANGED, new { luckDice = currentLuckDice, ownNetId = _race.netId.Value });
		}

		public float extraLuckDice;
		public float luckDiceSeed = 1f;

		[SyncVar(hook="OnLuckRewardChanged")]	
		public float luckReward;

		void OnLuckRewardChanged(float currentLuckReward){
			EventManager.TriggerEvent (Constants.EVENT_STAT_LUCK_REWARD_CHANGED, new {luckReward = currentLuckReward, ownNetId = _race.netId.Value});
		}

		public float extraLuckReward;
		public float luckRewardSeed = 1f;

		[Header("Stat percent")]
		public float strengthPercent;
		public float dexterityPercent;
		public float intelligentPercent;
		public float vitalityPercent;
		public float luckPercent;

		// allow adding point to stat values
		bool _autoAddPoint;

		public void AutoAddPoint(bool allow){
			_autoAddPoint = allow;
		}

		public void SetPoint(int point){
			this.point += point;
		}

		public void ResetPoint(){
			if (logPoint == 0)
				return;
			point += logPoint;
			logPoint = 0;
		}

		public void AddPoint(StatType statType, int point){
			switch (statType) {
			case StatType.Strength:
				{
					strength += point;
					++addedStrength;
					logPoint += point;
					Calculate2ndPoint (statType);
				}
				break;
			case StatType.Dexterity:
				{
					dexterity += point;
					++addedDexterity;
					logPoint += point;
					Calculate2ndPoint (statType);
				}
				break;
			case StatType.Intelligent:
				{
					intelligent += point;
					++addedIntelligent;
					logPoint += point;
					Calculate2ndPoint (statType);
				}
				break;
			case StatType.Vitality:
				{
					vitality += point;
					++addedVitality;
					logPoint += point;
					Calculate2ndPoint (statType);
				}
				break;
			case StatType.Luck:
				{
					luck += point;
					++addedLuck;
					logPoint += point;
					Calculate2ndPoint (statType);
				}
				break;
			default:
				break;
			}
		}

		public void AddPoint(StatType statType){
			if (point == 0)
				return;
			
			switch (statType) {
			case StatType.Strength:
				{
					++strength;
					++addedStrength;
					++logPoint;
					Calculate2ndPoint (statType);
					point = Mathf.Max(0, point - 1);
				}
				break;
			case StatType.Dexterity:
				{
					++dexterity;
					++addedDexterity;
					++logPoint;
					Calculate2ndPoint (statType);
					point = Mathf.Max(0, point - 1);
				}
				break;
			case StatType.Intelligent:
				{
					++intelligent;
					++addedIntelligent;
					++logPoint;
					Calculate2ndPoint (statType);
					point = Mathf.Max(0, point - 1);
				}
				break;
			case StatType.Vitality:
				{
					++vitality;
					++addedVitality;
					++logPoint;
					Calculate2ndPoint (statType);
					point = Mathf.Max(0, point - 1);
				}
				break;
			case StatType.Luck:
				{
					++luck;
					++addedLuck;
					++logPoint;
					Calculate2ndPoint (statType);
					point = Mathf.Max(0, point - 1);
				}
				break;
			default:
				break;
			}
		}

		bool increaseMaxHP;

		public void Calculate2ndPoint(StatType statType){
			switch (statType) {
			case StatType.Strength:
				{
					physicalAttack = strength * physicalAttackSeed + extraPhysicalAttack;
					physicalDefend = strength * physicalDefendSeed + extraPhysicalDefend;
				}
				break;
			case StatType.Dexterity:
				{
					attackRating = dexterity * attackRatingSeed + extraAttackRating;
					criticalRating = dexterity * criticalRatingSeed + extraCriticalRating;
				}
				break;
			case StatType.Intelligent:
				{
					magicAttack = intelligent * magicAttackSeed + extraMagicAttack;
					magicResist = intelligent * magicResistSeed + extraMagicResist;
				}
				break;
			case StatType.Vitality:
				{
					if (Mathf.Clamp (vitality, 10f, 20f) == vitality) {
						maxHpSeed = 5f;
						increaseMaxHP = vitality <= 10f;
					}
					if (Mathf.Clamp (vitality, 21f, 40f) == vitality) {
						maxHpSeed = 6f;
						increaseMaxHP = vitality <= 21f;
					}
					if (Mathf.Clamp (vitality, 41f, 60f) == vitality) {
						maxHpSeed = 8f;
						increaseMaxHP = vitality <= 41f;
					}
					if (Mathf.Clamp (vitality, 61f, 80f) == vitality) {
						maxHpSeed = 11f;
						increaseMaxHP = vitality <= 61f;
					}
					if (vitality > 80f) {
						maxHpSeed = 15f;
						increaseMaxHP = vitality <= 81f;
					}
					maxHp = vitality * maxHpSeed + extraMaxHp;
					regenerateHp = vitality * regenerateHpSeed + extraRegenerateHp;
					if (increaseMaxHP) {
						GetModule<HealthPowerModule> (x => x.SetMaxHp (setFullHp: false));
						increaseMaxHP = false;
					}
				}
				break;
			case StatType.Luck:
				{
					luckDice = luck * luckDiceSeed + extraLuckDice;
					luckReward = luck * luckRewardSeed + extraLuckReward;
				}
				break;
			default:
				break;
			}
		}

		[Command]
		public void CmdAddPoint(StatType statType){
			if (!isServer)
				return;
			AddPoint (statType);
		}

		public void AutoAddPoint(){
			var total = addedStrength + addedDexterity + addedIntelligent + addedLuck + addedVitality;
			if (total <= 0)
				return;
			var totalP = 100f / total;
			var strengthP = addedStrength * totalP;
			var dexterityP = addedDexterity * totalP;
			var intelligentP = addedIntelligent * totalP;
			var luckP = addedLuck * totalP;
			var vitalityP = addedVitality * totalP;
			AutoCalculateProbability (strengthP, dexterityP, intelligentP, vitalityP, luckP);

			RpcAutoAddPointCallback ();
		}

		[ClientRpc]
		void RpcAutoAddPointCallback(){
			EventManager.TriggerEvent (Constants.EVENT_STAT_AUTO_POINT_ADDED, new { ownNetId = _race.netId.Value });
		}

		[Command]
		public void CmdAutoAddPoint(){
			AutoAddPoint ();
		}

		void AutoCalculatePoint(){
			if (_autoAddPoint) {
				AutoCalculateProbability (strengthPercent, dexterityPercent, intelligentPercent, vitalityPercent, luckPercent);
				_autoAddPoint = false;
			}
		}

		void AutoCalculateProbability(params float[] percents){
			var _0 = 0;
			var _1 = 0;
			var _2 = 0;
			var _3 = 0;
			var _4 = 0;
			foreach (var statIndex in StatCalculator.GetStatWithProbability (point, percents)) {
				switch (statIndex) {
				case 0:
					++_0;
					break;
				case 1:
					++_1;
					break;
				case 2:
					++_2;
					break;
				case 3:
					++_3;
					break;
				case 4:
					++_4;
					break;
				default:
					break;
				}
			}
			this.point = 0;

			AddPoint (StatType.Strength, _0);
			AddPoint (StatType.Dexterity, _1);
			AddPoint (StatType.Intelligent, _2);
			AddPoint (StatType.Vitality, _3);
			AddPoint (StatType.Luck, _4);
		}

		void Update() {
			AutoCalculatePoint ();
		}
	}
}