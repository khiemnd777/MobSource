using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public abstract class Affect : MobBehaviour
	{
		public int level = 1;
		public int upgradeCount = 0;
		string _title;
		public virtual string title { get { return _title ?? this.name; }  set { _title = value; }}
		public float gainPoint = 0f;
		public Dictionary<string, object> effectValues = new Dictionary<string, object>();

		protected int turn = 1;
		protected bool isExecuted;

		public Race own;
		public Race[] targets;

		public virtual void Init(){
			
		}

		public virtual void Execute(Race target){
			
		}

		public virtual void Execute(){
			
		}

		public virtual void Disuse(){
			
		}

		public virtual bool Upgrade (){
			return false;	
		}

		public virtual void EmitAffect(EmitAffectArgs args){
			
		}

		public void AddGainPoint(float gainPoint = 0f){
			gainPoint = gainPoint > 0f ? gainPoint : this.gainPoint;
			own.AddGainPoint (gainPoint);
		}

		public void SubtractEnergy(float energy){
			own.GetModule<EnergyModule> ((e) => {
				e.SubtractEnergy (energy);
			});
		}

		public void SubtractGold(float gold){
			own.GetModule<GoldModule> ((g) => {
				g.SubtractGold(gold);
			});
		}

		public bool EnoughGold(float gold, Action predicate){
			var enough = false;
			own.GetModule<GoldModule> ((g) => {
				enough = g.gold >= gold;
			});
			return enough;
		}

		public void ExecuteInTurn(Race who, Action predicate){
			if (who.isTurn) {
				if (!isExecuted) {
					if (predicate != null) {
						predicate.Invoke ();
					}
					isExecuted = true;		
				}
			} else {
				if (isExecuted) {
					turn++;
					isExecuted = false;
				}
			}
		}

		public bool EnoughLevel(int level, Action predicate){
			var levelModule = own.GetModule<LevelModule> ();
			if (levelModule.level < level) {
				Destroy (gameObject);	
				return false;
			}
			if (predicate != null) {
				predicate.Invoke ();
			}
			return true;
		}

		public static bool HasAffect<T>(Race who, Action<T> predicate = null) where T: Affect{
			var affectModule = who.GetModule<AffectModule> ();
			var result = affectModule != null && affectModule.HasAffect<T> ();
			if (result && predicate != null) {
				GetAffects<T>(who, predicate);
			}
			return result;
		}

		public static void RemoveAffect<T>(Race who) where T: Affect {
			var affectModule = who.GetModule<AffectModule> ();
			affectModule.GetAffects<T> (x => Destroy(x.gameObject));
			affectModule.RefreshAffect ();
		}

		public static void RemoveAffect(Race who, Affect affect){
			var affectModule = who.GetModule<AffectModule> ();
			Destroy(affect.gameObject);
			affectModule.RefreshAffect ();
		}

		public static T[] GetAffects<T>(Race who, Action<T> predicate = null) where T: Affect{
			T[] result = new T[0];
			who.GetModule<AffectModule> ((a) => {
				result = a.GetAffects<T>(predicate);
			});
			return result;
		}

		public static T[] GetSubAffects<T>(Race who, Action<T> predicate = null) {
			T[] result = new T[0];
			who.GetModule<AffectModule> ((a) => {
				result = a.GetSubAffects<T>(predicate);
			});
			return result;
		}

		public static void Create<T>(string resource, Race own, Race target, Action<T> predicate = null) where T : Affect {
			Create<T> (resource, own, new Race[]{ target }, predicate);
		}

		public static void Create<T>(string resource, Race own, Race[] targets, Action<T> predicate = null) where T : Affect {
			var a = GetMonoResource<T> (resource);
			Create<T> (a, own, targets, predicate);
		}

		public static void Create<T>(T affect, Race own, Race[] targets, Action<T> predicate = null) where T : Affect {
			var a = Instantiate<T>(affect);
			a.Init ();
			a.own = own;
			a.targets = targets;
			if (predicate != null) {
				predicate.Invoke (a);
			}
			foreach (var target in a.targets) {
				target.GetModule<AffectModule> ((am) => {
					am.AddAffect(a);
				});
			}
		}

		public static void CreatePrimitiveAndUse(Type affectType, Race own, Race[] targets, Action<object> predicate = null) {
			var a = CreatePrimitive (affectType, own, targets, predicate);
			Use (own, a);
		}

		public static void CreatePrimitiveAndUse<T>(Race own, Race[] targets, Action<T> predicate = null) where T: Affect {
			CreatePrimitiveAndUse (typeof(T), own, targets, predicate.Convert());
		}

		public static void Use(Race own, Affect affect){
			var a = affect;
			foreach (var target in a.targets) {
				target.GetModule<AffectModule> ((am) => {
					am.AddAffect(a);
				});
				var effectValueTransfer = own.GetModule<EffectValueTransferModule> ();
				if (typeof(IPhysicalAttackingEventHandler).IsAssignableFrom (a.GetType ())) {
					var physicalAttacking = ((IPhysicalAttackingEventHandler)a);
					var stat = own.GetModule<StatModule> ();
					var targetStat = target.GetModule<StatModule> ();
					var isHit = AccuracyCalculator.IsHit (stat.attackRating, targetStat.attackRating);
					isHit = !isHit ? AccuracyCalculator.IsDodgeable(own, target) : isHit;
					isHit = !isHit ? AccuracyCalculator.MakeSureHit(own) : isHit;
					effectValueTransfer.Add ("isHit", isHit);
					if (isHit) {
						var isCritHit = AccuracyCalculator.IsCriticalHit (own, stat.criticalRating);
						isCritHit = !isCritHit ? AccuracyCalculator.MakeSureCritical (own) : isCritHit;
						// optional Damage
						var outputDamage = AttackPowerCalculator.TakeDamage (physicalAttacking.bonusDamage, targetStat.physicalDefend, isCritHit);
						AccuracyCalculator.HandleCriticalDamage (ref outputDamage, own, target);
						AttackPowerCalculator.HandleDamage (ref outputDamage, own, target);

						target.GetModule<HealthPowerModule> (x => x.SubtractHp (outputDamage));

						effectValueTransfer.Add ("damage", outputDamage);
						effectValueTransfer.Add ("targetHp", target.GetModule<HealthPowerModule> ().hp);
						effectValueTransfer.Add ("isCritHit", isCritHit);
						effectValueTransfer.Add ("targetNetId", target.netId.Value);

						AttackPowerCalculator.OccurAffectChange(own, target);
						AttackPowerCalculator.ExtraAttackableAffect (own, target);
						physicalAttacking.HandleAttack (target);
					} else {
						var isCritHit = AccuracyCalculator.IsCriticalHit (own, stat.criticalRating);
						var damage = AttackPowerCalculator.TakeDamage(physicalAttacking.bonusDamage, targetStat.physicalDefend, isCritHit);

						effectValueTransfer.Add ("damage", damage);
						effectValueTransfer.Add ("targetHp", target.GetModule<HealthPowerModule> ().hp);
						effectValueTransfer.Add ("isCritHit", isCritHit);
						effectValueTransfer.Add ("targetNetId", target.netId.Value);

						physicalAttacking.HandleAttack (target);
					}
				} else if(typeof(IMagicalAttackingEventHandler).IsAssignableFrom(a.GetType())){
					var magicalAttacking = ((IMagicalAttackingEventHandler)a);
					var stat = own.GetModule<StatModule> ();
					var targetStat = target.GetModule<StatModule> ();
					var isHit = AccuracyCalculator.IsHit (stat.attackRating, targetStat.attackRating);
					isHit = !isHit ? AccuracyCalculator.IsDodgeable(own, target) : isHit;
					isHit = !isHit ? AccuracyCalculator.MakeSureHit(own) : isHit;
					effectValueTransfer.Add ("isHit", isHit);
					if (isHit) {
						var isCritHit = AccuracyCalculator.IsCriticalHit (own, stat.criticalRating);
						isCritHit = !isCritHit ? AccuracyCalculator.MakeSureCritical (own) : isCritHit;
						// optional Damage
						var outputDamage = AttackPowerCalculator.TakeDamage (magicalAttacking.bonusDamage, targetStat.magicResist, isCritHit);
						AccuracyCalculator.HandleCriticalDamage (ref outputDamage, own, target);
						AttackPowerCalculator.HandleDamage (ref outputDamage, own, target);

						target.GetModule<HealthPowerModule> (x => x.SubtractHp (outputDamage));

						effectValueTransfer.Add ("damage", outputDamage);
						effectValueTransfer.Add ("targetHp", target.GetModule<HealthPowerModule> ().hp);
						effectValueTransfer.Add ("isCritHit", isCritHit);
						effectValueTransfer.Add ("targetNetId", target.netId.Value);

						AttackPowerCalculator.OccurAffectChange(own, target);
						AttackPowerCalculator.ExtraAttackableAffect (own, target);	
						magicalAttacking.HandleAttack (target);
					} else {
						var isCritHit = AccuracyCalculator.IsCriticalHit (own, stat.criticalRating);
						var damage = AttackPowerCalculator.TakeDamage(magicalAttacking.bonusDamage, targetStat.physicalDefend, isCritHit);

						effectValueTransfer.Add ("damage", damage);
						effectValueTransfer.Add ("targetHp", target.GetModule<HealthPowerModule> ().hp);
						effectValueTransfer.Add ("isCritHit", isCritHit);
						effectValueTransfer.Add ("targetNetId", target.netId.Value);
						magicalAttacking.HandleAttack (target);

					}
				} else {
					a.Execute (target);
				}

				EmitAffects (own, new EmitAffectArgs {
					affect = a,
					target = target
				});
			}
			a.Execute ();
			a.AddGainPoint ();
			a.HandlePlugins ();
			a.FlushAll ();
		}

		public static T CreatePrimitive<T>(Race own, Race[] targets, Action<T> predicate = null) where T: Affect{
			return (T)CreatePrimitive (typeof(T), own, targets, predicate.Convert());
		}

		public static Affect CreatePrimitive(Type affectType, Race own, Race[] targets, Action<object> predicate = null){
			var go = new GameObject (affectType.Name, affectType);
			var a = (Affect) go.GetComponent (affectType);
			a.own = own;
			a.targets = targets;
			if (predicate != null) {
				predicate.Invoke (a);
			}
			a.transform.SetParent (own.transform);
			a.Init ();
			a.StartCoroutine (a.Interacting (a.gameObject));
			return a;
		}

		public static void EmitAffects(Race own, EmitAffectArgs args){
			var a = new Action<Race> (r => {
				var affectModule = r.GetModule<AffectModule> ();
				foreach (var affect in affectModule.affects) {
					affect.EmitAffect (args);
				}	
			});
			a.Invoke (own);
			a.Invoke (args.target);
		}
	}

	public class EmitAffectArgs{
		public Affect affect {get;set;}
		public Race target {get;set;}
	}

	public class EffectArgs{
		
	}
}

