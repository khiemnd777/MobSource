using UnityEngine;

namespace Mob
{
	public abstract class RaceModule : MobNetworkBehaviour
	{
		protected Race _race 
		{
			get 
			{
				return GetComponent<Race> ();
			}
		}

		public virtual void Init(){
			
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
	}
}

