using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mob
{
	public class IconHelper
	{
		static IconHelper _instance;

		public static IconHelper instance{
			get { return _instance ?? (_instance = new IconHelper()); }
		}

		public Dictionary<string, Sprite> icons;

		public IconHelper(){
			icons = new Dictionary<string, Sprite> ();
		}

		public Sprite GetIcon(string name){
			var icon = icons.ContainsKey(name) ? icons [name] : null;
			if (icon != null)
				return icon;
			var prefabCond = name.Split (new [] { "=>" }, StringSplitOptions.RemoveEmptyEntries);
			var n1 = prefabCond [0].Trim();
			if (prefabCond.Length == 1) {
				icons.Add (name, Resources.Load<Sprite> (n1));
				return GetIcon (name);
			}
			if (prefabCond.Length == 2) {
				var n2 = prefabCond [1].Trim();
				icons.Add (name, Resources.LoadAll<Sprite>(n1).FirstOrDefault(x => x.name == n2));
				return GetIcon (name);
			}
			return null;
		}
	}
}

