using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Mob
{
	public class AttackPowerCalculator
	{
		public static float TakeDamage(float attackerDamage, float targetResistance, bool isCritHit){
			var baseDamage = attackerDamage * (isCritHit ? 1.5f : 1f);
			return baseDamage * baseDamage / (baseDamage + targetResistance * 1.3f);
		}

		public static void HandleDamage(ref float damage, Race own, Race target){
			var _ = float.MinValue;
			var d = damage;
			own.GetModule<AffectModule>(am => {
				am.GetSubAffects<IDamaged>(a => {
					_ = Mathf.Max(_, a.HandleDamage(d, target));
				});
			});
			damage = Mathf.Max (_, damage);
		}

		public static void ExtraAttackableAffect(Race own, Race target){
			own.GetModule<AffectModule>(am => {
				am.GetSubAffects<IAttackableAffect>(a => {
					a.AssignAttackableAffect(target);
				});
			});
		}

		public struct DefineAffectArgs
		{
			public Affect affect;
			public MethodInfo methodDefineAffect;
		}

		public static void OccurAffectChange(Race own, Race target){
			var occurs = new Dictionary<Type, float> ();
			var defineAffects = new Dictionary<Type, DefineAffectArgs> ();
			var addAffectHandler = new Action<Affect, Type>((affect, t) => {
				var method = affect.GetType().GetMethod("AdditionalAffectChange"
					, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
					, null
					, CallingConventions.Any
					, new Type[]{t, target.GetType()}
					, null);
				var useDefine = affect.GetType().GetMethod("UseDefineAffect"
					, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
					, null
					, CallingConventions.Any
					, new Type[]{t}
					, null);
				if(method != null){
					var value = method.Invoke(affect, new object[]{null, target});
					var useDefineAffect = (bool)useDefine.Invoke(affect, new object[]{null});
					if(value != null){
						if(occurs.ContainsKey(t)){
							occurs[t] += (float)value;
						} else {
							occurs.Add(t, (float)value);
						}
					}
					if(useDefineAffect && !defineAffects.ContainsKey(t)){
						var methodDefineAffect = affect.GetType().GetMethod("DefineAffect"
							, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic
							, null
							, CallingConventions.Any
							, new Type[]{t}
							, null);
						defineAffects.Add(t, new DefineAffectArgs{
							affect = affect,
							methodDefineAffect = methodDefineAffect
						});
					}
				}
			});
			own.GetModule<AffectModule> (am => {
				foreach(var affect in am.affects){
					var interfaceTypes = affect.GetType().GetInterfaces();
					foreach(var interfaceType in interfaceTypes){
						if(interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IAdditionalAffectChange<>)){
							var t = interfaceType.GetGenericArguments();
							if(t.Length == 0)
								continue;
							var tt = t[0];
							addAffectHandler(affect, tt);
						}
					}
				}
			});
			foreach (var occur in occurs) {
				var percent = Mathf.Min (occur.Value * 100f, 100f);
				var probability = Probability.Initialize (new bool[]{ false, true }, new float[] {
					100f - percent,
					percent
				});
				var result = Probability.GetValueInProbability (probability);
				if (!result)
					continue;
				Affect.CreatePrimitiveAndUse (occur.Key, own, new Race[]{ target }, af => {
					foreach(var defineAffect in defineAffects.Where(d => d.Key == occur.Key)){
						var affect = defineAffect.Value.affect;
						var method = defineAffect.Value.methodDefineAffect;
						method.Invoke(affect, new object[]{af});
					}
				});
			}
		}

		public static void AssignDamage(ref float attackDamage, Race own){
			var _ = float.MinValue;
			own.GetModule<AffectModule>(am => {
				am.GetSubAffects<IAssignableDamage>(a => {
					_ = Mathf.Max(_, a.AssignDamage());
				});
			});

			attackDamage = Mathf.Max (_, attackDamage);
		}
	}
}

