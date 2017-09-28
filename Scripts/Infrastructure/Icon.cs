using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mob
{
	public sealed class Icon
	{
		public Dictionary<string, Sprite> instances;
		public Dictionary<string, string> prefabs;

		public Icon(){
			instances = new Dictionary<string, Sprite> ();
			prefabs = new Dictionary<string, string> ();
		}

		public string GetIconName(string key){
			return prefabs.ContainsKey (key) ? prefabs [key] : null;
		}

		public Sprite GetIconFromInstance(string key, Func<bool> predicate){
			return predicate != null && predicate.Invoke () ? instances [key] : null;
		}

		public Sprite GetIconFromInstance(string key){
			return instances.ContainsKey(key) ? instances [key] : null;
		}

		public Sprite GetIconFromInstance(){
			return instances.Count > 0 ? instances.FirstOrDefault().Value : null;
		}

		public Sprite GetIconFromPrefab(string key, Func<bool> predicate){
			var iconFromInstance = GetIconFromInstance (key, predicate);
			if (iconFromInstance != null)
				return iconFromInstance;
			var prefabStr = predicate != null && predicate.Invoke () ? prefabs [key] : null;
			if (prefabStr == null)
				return null;
			var prefabCond = prefabStr.Split (new[]{ "=>" }, StringSplitOptions.RemoveEmptyEntries);
			var name = prefabCond [0].Trim();
			if (prefabCond.Length == 1) {
				instances.Add (key, Resources.Load<Sprite> (name));
				return GetIconFromInstance (key, predicate);
			}
			if (prefabCond.Length == 2) {
				var subName = prefabCond [1].Trim();
				instances.Add (key, Resources.LoadAll<Sprite>(name).FirstOrDefault(x => x.name == subName));
				return GetIconFromInstance (key, predicate);
			}
			return null;
		}

		public Sprite GetIconFromPrefab(string key){
			var iconFromInstance = GetIconFromInstance (key);
			if (iconFromInstance != null)
				return iconFromInstance;
			var prefabStr = prefabs.ContainsKey(key) ? prefabs [key] : null;
			if (prefabStr == null)
				return null;
			var prefabCond = prefabStr.Split (new[]{ "=>" }, StringSplitOptions.RemoveEmptyEntries);
			var name = prefabCond [0].Trim();
			if (prefabCond.Length == 1) {
				instances.Add (key, Resources.Load<Sprite> (name));
				return GetIconFromInstance (key);
			}
			if (prefabCond.Length == 2) {
				var subName = prefabCond [1].Trim();
				instances.Add (key, Resources.LoadAll<Sprite>(name).FirstOrDefault(x => x.name == subName));
				return GetIconFromInstance (key);
			}
			return null;
		}

		public Sprite GetIconFromPrefab(){
			var iconFromInstance = GetIconFromInstance ();
			if (iconFromInstance != null)
				return iconFromInstance;
			var prefabStr = prefabs.Count > 0 ? prefabs.FirstOrDefault().Value : null;
			if (prefabStr == null)
				return null;
			var prefabCond = prefabStr.Split (new[]{ "=>" }, StringSplitOptions.RemoveEmptyEntries);
			var name = prefabCond [0].Trim();
			if (prefabCond.Length == 1) {
				instances.Add (prefabs.FirstOrDefault().Key, Resources.Load<Sprite> (name));
				return GetIconFromInstance ();
			}
			if (prefabCond.Length == 2) {
				var subName = prefabCond [1].Trim();
				instances.Add (prefabs.FirstOrDefault().Key, Resources.LoadAll<Sprite>(name).FirstOrDefault(x => x.name == subName));
				return GetIconFromInstance ();
			}
			return null;
		}
	}
}