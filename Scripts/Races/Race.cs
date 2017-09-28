using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mob
{
	[RequireComponent(typeof(NetworkIdentity))]
	public class Race : MobNetworkBehaviour
	{
		[SyncVar]
		public string className;
		[SyncVar]
		public uint playerNetId;

		public Race[] targets;

		[Command]
		void CmdCheckAuthority(){
			
		}

		public override void OnStartClient ()
		{
			var playerGo = ClientScene.FindLocalObject(new NetworkInstanceId(playerNetId));
			var netIdentity = playerGo.GetComponent<NetworkIdentity> ();
			tag = netIdentity.isLocalPlayer ? Constants.LOCAL_CHARACTER : Constants.OPPONENT_CHARACTER;
		}

		public T GetModule<T>(System.Action<T> predicate = null) where T : RaceModule
		{
			var module = GetComponent<T> ();	
			if (predicate != null) {
				predicate.Invoke (module);
			}
			return module;
		}

		public T[] GetModules<T>(System.Action<T> predicate = null){
			var modules = GetComponents<T>();
			foreach (var module in modules) {
				if (predicate != null) {
					predicate.Invoke (module);
				}
			}
			return modules;
		}

		public T AddModule<T>(System.Action<T> predicate = null) where T: RaceModule{
			var module = gameObject.AddComponent<T> ();
			if (predicate != null) {
				predicate.Invoke (module);
			}
			return module;
		}

		protected bool _isTurn;
		protected int _turnNumber;

		public bool isTurn {
			get {
				return _isTurn;
			}
		}

		public int turnNumber{
			get{
				return _turnNumber;
			}
		}

		public void AllowTurn (bool allow)
		{
			_isTurn = allow;
		}

		#region Gain point functions

		// Gain point
		public float gainPoint;
		public float gainPointLabel;

		public void AddGainPoint(float p){
			gainPoint += p;
			While ((inc, step) => {
				gainPointLabel += inc;
			}, p, 1f);
			var gainPointValue = GetMonoComponent<Text> (Constants.ATTACKER_GAIN_POINT_LABEL);
			if (p > 0) {
				if (gainPointValue != null) {
					JumpEffect (gainPointValue.transform, Vector3.one);
					ShowSubLabel (Constants.INCREASE_LABEL, gainPointValue.transform, p);
				}
			}
		}

		public void SubtractGainPoint(float p){
			gainPoint = Mathf.Max (gainPoint - p, 0f);
		}

		#endregion

		public virtual void DefaultValue(){

		}

		protected bool _enableAttack;
		public void AllowAttack(bool allow)
		{
			_enableAttack = allow;
		}

		protected bool _enableBuy;
		public void AllowBuy(bool allow)
		{
			_enableBuy = allow;
		}

		protected bool _enableUpgrade;
		public void AllowUpgrade(bool allow)
		{
			_enableUpgrade = allow;
		}

		public Skill[] GetAvailableSkills(){
			var skills = new Skill[0];
			GetModule<SkillModule> (x => {
				skills = x.skills.ToArray();
			});
			return skills;
		}

		public virtual void Attack(Race[] targets, Skill skill){
			if (!_enableAttack)
				return;
			skill.Use (targets);
		}

		public virtual void Attack<T>(Race[] targets) where T: Skill{
			if (!_enableAttack)
				return;
			GetModule<SkillModule> (x => x.Use<T> (targets));
		}

		public virtual void BuyItem (){

		}

		public virtual void Upgrade (){

		}

		public virtual void OpenSkillTree(){

		}

		public virtual void StartTurn(){
			_isTurn = true;
			_isEndTurn = false;
			_turnNumber++;
			GetModule<StatModule> (s => {
				GetModule<HealthPowerModule>(hp => {
					Affect.CreatePrimitiveAndUse<HealthPowerRestoring> (this, new Race[]{this}, hpr => hpr.extraHp = s.regenerateHp * hp.hp);
				});
			});
		}

		protected bool _isEndTurn;
		public bool isEndTurn{
			get{
				return _isEndTurn;
			}
		}
		public virtual void EndTurn(){
			_isTurn = false;
			_isEndTurn = true;
		}

		public static T Create<T>(string resource, Action<T> predicate = null) where T: Race{
			var go = InstantiateFromMonoResource<GameObject> (resource);
			if (go == null)
				return null;

			var race = go.GetComponent<T> ();
			if (predicate != null) {
				predicate.Invoke (race);
			}
			return race;
		}

		public static T Create<T>(T prefab, Action<T> predicate = null) where T: Race{
			var go = Instantiate<T> (prefab);

			var race = go.GetComponent<T> ();
			if (predicate != null) {
				predicate.Invoke (race);
			}
			return race;
		}

		public static T CreatePrimitive<T>(Action<T> predicate = null) where T: Race {
			var go = new GameObject (typeof(T).Name, typeof(T));

			// Network identity
			var networkIdentity = go.GetComponent<NetworkIdentity> ();
			networkIdentity.localPlayerAuthority = true;

//			// Network transform
//			var networkTransform = go.GetComponent<NetworkTransform>();
//			networkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncCharacterController;

			// Own component via Race
			var race = go.GetComponent<T> ();
			if (predicate != null) {
				predicate.Invoke (race);
			}

			return race;
		}

		public static Race GetLocalCharacter(){
			var go = GameObject.FindGameObjectWithTag (Constants.LOCAL_CHARACTER);
			return go != null ? go.GetComponent<Race> () : null;
		}

		public static Race GetOpponentCharacter(){
			var go = GameObject.FindGameObjectWithTag (Constants.OPPONENT_CHARACTER);
			return go != null ? go.GetComponent<Race> () : null;
		}

		public static Race[] GetOpponentCharacters(){
			var go = GameObject.FindGameObjectsWithTag (Constants.OPPONENT_CHARACTER);
			return go.Length > 0 ? go.Select (x => x.GetComponent<Race> ()).ToArray() : new Race[0];
		}

		public static Race GetCharacterByNetId(uint netId, bool isServer = true){
			GameObject targetGo = null;
			if (isServer) {
				targetGo = NetworkServer.FindLocalObject (new NetworkInstanceId (netId));	
			} else {
				targetGo = ClientScene.FindLocalObject(new NetworkInstanceId(netId));
			}
			if (targetGo == null)
				return null;
			return targetGo.GetComponent<Race> ();
		}

		public static Race[] GetCharactersByNetIds(uint[] netId, bool isServer = true){
			var targetsGo = netId
				.Select (x => GetCharacterByNetId (x, isServer))
				.Where (x => x != null)
				.ToArray ();
			return targetsGo;
		}

		public static uint GetOpponentCharacterNetId(){
			var race = GetOpponentCharacter ();
			if (race == null)
				return new uint();
			return race.netId.Value;
		}

		public static uint[] GetOpponentCharacterNetIds(){
			var races = GetOpponentCharacters ();
			if (races.Length <= 0)
				return new uint[0];
			return races.Select (x => x.netId.Value).ToArray ();
		}
	}
}

