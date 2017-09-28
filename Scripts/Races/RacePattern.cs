using UnityEngine;
using System;

namespace Mob
{
	public abstract class RacePattern : MobBehaviour
	{
		public abstract void Pick(string playerId);

		public virtual void Pick<T>(string playerId, Action<T> predicate = null) where T: Race{
			Race.CreatePrimitive<T> (p => {
				p.tag = playerId;
				p.name = playerId;
				p.DefaultValue ();
				if(predicate != null) {
					predicate.Invoke(p);
				}
			});
		}
		
		public static T CreatePrimitive<T>(Action<T> predicate = null) where T: RacePattern {
			var go = new GameObject (typeof(T).Name, typeof(T));
			var pattern = go.GetComponent<T> ();
			if (predicate != null) {
				predicate.Invoke (pattern);
			}
			return pattern;
		}
	}
}

