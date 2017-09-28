using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Mob
{
	public struct EffectValueNetworkDelivery {
		public string key;
		public string value;
	}

	public class EffectValueNetworkTransfer : SyncListStruct<EffectValueNetworkDelivery> { }

	public abstract class Effect : PluginHandler
	{
		public bool use;
		public Race attacker;
		public Race[] targets;
		public object host;

		public virtual void AssignFor(object host){
			this.host = host;
		}

		public abstract IEnumerator Define (Dictionary<string, object> effectValues);

		public override void HandlePlugin (params object[] args)
		{
			Effect.Use (attacker, this);
		}

		public static Effect CreatePrimitive(Type effectType, object host, Race attacker, Race[] targets, Action<object> predicate = null) {
			var go = new GameObject (effectType.Name, effectType);
			var e = (Effect) go.GetComponent (effectType);
			e.attacker = attacker;
			e.targets = targets;
			if (predicate != null) {
				predicate.Invoke (e);
			}
			e.InitPlugin ();
			e.AssignFor (host);
			if (!e.use) {
				Destroy (e.gameObject);
				return null;
			}
			return e;
		}

		public static T CreatePrimitive<T>(object host, Race attacker, Race[] targets, Action<T> predicate = null) where T: Effect{
			return (T)CreatePrimitive (typeof(T), host, attacker, targets, predicate.Convert());
		}

		public static void Use(Race attacker, Effect effect){
			if (effect.use) {
				Dictionary<string, object> ev = null;

//				var effectValueTransfer = attacker.GetModule<EffectValueTransferModule> ();
//				if (effectValueTransfer != null) {
//					ev = effectValueTransfer.ToDictionary ();
//				}

				if (effect.host != null) {
					var effectValues = effect.host.GetType ().GetField ("effectValues");
					if (effectValues != null) {
						ev = (Dictionary<string, object>)effectValues.GetValue (effect.host);
					}
				} 

				attacker.StartCoroutine(effect.Define (ev));
				Destroy (effect.gameObject, effect.timeToDestroy <= -1f ? Constants.TIME_EFFECT_END_DEFAULT : effect.timeToDestroy);
			}
			else
				Destroy (effect.gameObject);
		}

		public static void CreatePrimitiveAndUse(Type effectType, object host, Race attacker, Race[] targets, Action<object> predicate = null){
			var effect = CreatePrimitive (effectType, host, attacker, targets, predicate);
			if (effect == null)
				return;
			Use (attacker, effect);
		}

		public static void CreatePrimitiveAndUse<T>(object host, Race attacker, Race[] targets, Action<T> predicate = null) where T: Effect{
			var effect = CreatePrimitive<T> (host, attacker, targets, predicate);
			if (effect == null)
				return;
			Use (attacker, effect);
		}
	}
}

