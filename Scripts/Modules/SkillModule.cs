using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
	public class SkillModule : RaceModule
	{
		public int evolvedSkillPoint;
		public RectTransform skillTreeUIPrefab;
		public SkillTreeUI skillTreeUI;
		public List<Skill> skills = new List<Skill>();
		public Dictionary<string, Type> skillEffects = new Dictionary<string, Type> ();
		public List<SkillBoughtItem> availableSkills = new List<SkillBoughtItem>();

		[SyncVar] 
		//public SyncListString networkSkills = new SyncListString();
		public SyncListItem syncSkills = new SyncListItem();

		[SyncVar] 
//		public SyncListString syncAvailableSkills = new SyncListString();
		public SyncListSkillBoughtItem syncAvailableSkills = new SyncListSkillBoughtItem();

		void Start(){
			if (skillTreeUIPrefab != null) {
				var instPfb = Instantiate (skillTreeUIPrefab, GetMonoComponent<Canvas> ("Canvas").transform);
				skillTreeUI = instPfb.GetComponent<SkillTreeUI> ();
				skillTreeUI.own = _race;
				skillTreeUI.gameObject.SetActive (false);
			}
		}

		void RefeshSyncSkills(){
			var removedItems = new List<SyncItem> ();
			foreach (var syncObj in syncSkills) {
				if (skills.Any (x => x.GetInstanceID () == syncObj.id))
					continue;
				removedItems.Add (syncObj);
			}

			foreach (var syncObj in removedItems) {
				syncSkills.RemoveAt (syncSkills.IndexOf(syncObj));
			}

			foreach (var item in skills) {
				var syncItem = item.ToSyncItem ();
				if(!syncSkills.Any(x => x.id == item.GetInstanceID())) {
					syncSkills.Add(syncItem);
					continue;
				}
				var syncObj = syncSkills.FirstOrDefault (x => x.id == item.GetInstanceID ());
				var syncObjIndex = syncSkills.IndexOf (syncObj);
				if(!object.ReferenceEquals(syncSkills[syncObjIndex], syncItem)){
					syncSkills[syncObjIndex] = syncItem;	
				}
			}
			RpcRefreshSyncSkillCallback ();
		}

		[ClientRpc]
		void RpcRefreshSyncSkillCallback(){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_SKILLS, new { ownNetId = _race.netId.Value });
		}

		void RefreshSyncAvailableSkills(){
			var removedItems = new List<SyncSkillBoughtItem> ();
			foreach (var syncObj in syncAvailableSkills) {
				if (availableSkills.Any (x => x.GetInstanceID () == syncObj.id))
					continue;
				removedItems.Add (syncObj);
			}

			foreach (var syncObj in removedItems) {
				syncSkills.RemoveAt (syncAvailableSkills.IndexOf(syncObj));
			}

			foreach (var item in availableSkills) {
				var syncItem = item.ToSyncSkillBoughtItem ();
				if(!syncAvailableSkills.Any(x => x.id == item.GetInstanceID())) {
					syncAvailableSkills.Add(syncItem);
					continue;
				}
				var syncObj = syncAvailableSkills.FirstOrDefault (x => x.id == item.GetInstanceID ());
				var syncObjIndex = syncAvailableSkills.IndexOf (syncObj);
				if (!object.ReferenceEquals (syncAvailableSkills [syncObjIndex], syncItem)) {
					syncAvailableSkills[syncObjIndex] = syncItem;	
				}
			}

			RpcRefreshSyncAvailableSkillCallback ();
		}

		[ClientRpc]
		void RpcRefreshSyncAvailableSkillCallback(){
			EventManager.TriggerEvent(Constants.EVENT_REFRESH_SYNC_AVAILABLE_SKILLS, new { ownNetId = _race.netId.Value });
		}

		public void Add<T>(int quantity, Action<T> predicate = null) where T: Skill{
			if (!skills.Any (x => x.GetType().IsEqual<T> ())) {
				var skill = Skill.CreatePrimitive<T> (_race, quantity, predicate);
				skills.Add (skill);
//				RefeshSyncSkills ();
				if (skill.effectType != null) {
					RpcAddEffect (skill.name, skill.effectType.FullName);	
				}
				return;
			}
		}

		[ClientRpc]
		public void RpcAddEffect(string key, string effectType){
			if (!isClient)
				return;
			var type = Type.GetType (effectType);
			skillEffects.Add (key, type);
		}

		public T GetSkill<T>() where T: Skill{
			if (!skills.Any (x => x.GetType ().IsEqual<T> ()))
				return null;
			var skill = skills.FirstOrDefault (x => x.GetType().IsEqual<T> ());
			return (T)skill;
		}

		public bool HasSkill<T>() where T: Skill{
			return skills.Any (x => x.GetType ().IsEqual<T> ());
		}

		public bool HasSkill(Skill skill){
			return skills.Any (x => x.GetType ().IsAssignableFrom(skill.GetType()));
		}

		public void AddAvailableSkill<T>(Action<T> predicate = null) where T: SkillBoughtItem{
			if (!availableSkills.Any (x => x.GetType().IsEqual<T> ())) {
				availableSkills.Add (SkillBoughtItem.CreatePrimitiveWithOwn<T> (_race, predicate));
				RefreshSyncAvailableSkills ();
				return;
			}
		}

		public void PickAvailableSkill<T>() where T: SkillBoughtItem{
			var skill = GetAvailableSkill<T> ();
			if (skill == null)
				return;
			PickAvailableSkill (skill);
		}

		public void PickAvailableSkill(SkillBoughtItem boughtItem){
			if (!availableSkills.Any (x => x.GetType ().IsAssignableFrom (boughtItem.GetType ())))
				return;
			boughtItem.Pick (_race, 1);
			boughtItem.learned = true;

			RefreshSyncAvailableSkills ();
			RefeshSyncSkills ();
			RpcPickedCallback ();
		}

		[ClientRpc]
		void RpcPickedCallback(){
			EventManager.TriggerEvent (Constants.EVENT_SKILL_PICKED, new { ownNetId = _race.netId.Value });
		}

		[Command]
		public void CmdPickAvailableSkill(SyncSkillBoughtItem syncBoughtItem){
			var boughtItem = availableSkills.FirstOrDefault (x => x.GetInstanceID () == syncBoughtItem.id);
			if (boughtItem == null)
				return;
			PickAvailableSkill (boughtItem);
		}

		public T GetAvailableSkill<T>() where T: SkillBoughtItem{
			if(!availableSkills.Any(x => x.GetType().IsEqual<T>()))
				return null;
			var skill = availableSkills.FirstOrDefault (x => x.GetType ().IsEqual<T> ());
			return (T)skill;
		}

		public void Remove<T>() where T: Skill{
			if (!skills.Any (x => x.GetType ().IsEqual<T> ()))
				return;
			var skill = skills.FirstOrDefault (x => x.GetType().IsEqual<T> ());
			DestroyImmediate (skill.gameObject);
			skills = skills.Where (x => x != null).ToList ();
		}

		public void Remove(Skill skill){
			DestroyImmediate (skill.gameObject);
			skills = skills.Where (x => x != null).ToList();
		}

		public void Use<T>(Race[] targets){
			if (!skills.Any (x => x.GetType().IsEqual<T> ())) 
				return;
			var item = skills.FirstOrDefault (x => x.GetType().IsEqual<T> ());

			Use (item, targets);
		}

		public void Use(Skill skill, Race[] targets) {
			skill.Use (targets);
			skill.SubtractEnergy ();
			++skill.usedNumber;
			skill.usedTurn = _race.turnNumber;
			--skill.quantity;

			RefeshSyncSkills ();

			if (skill.quantity == 0) {
				skill.quantity = 1;
				return;
			}
		}

		[Command]
		public void CmdUse(SyncItem syncItem){
			var skill = skills.FirstOrDefault (x => x.GetInstanceID () == syncItem.id);
			if (skill == null)
				return;
			
			var opponents = Race.GetCharactersByNetIds(syncItem.targetNetIds);
			Use (skill, opponents);

			// EXECUTE EFFECT ON CLIENT
			RpcInvokeEffect (skill.name, syncItem);
		}

		[ClientRpc]
		public void RpcInvokeEffect(string key, SyncItem syncItem){
			if (!isClient)
				return;
			var effectType = skillEffects [key];
			if (effectType == null)
				return;
			var attacker = Race.GetCharacterByNetId (syncItem.ownNetId, false);
			var opponents = Race.GetCharactersByNetIds(syncItem.targetNetIds, false);
			Effect.CreatePrimitiveAndUse (effectType, null, attacker, opponents);
		}

		public void OpenSkillTreeUI(){
			if (skillTreeUI == null)
				return;
			skillTreeUI.gameObject.SetActive (true);
		}
	}
}