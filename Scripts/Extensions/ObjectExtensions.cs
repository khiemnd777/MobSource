using System;

namespace Mob
{
	public static class ObjectExtensions
	{
		public static bool IsNull(this object target){
			return target == null || target is UnityEngine.Object && target.Equals (null);
		}
	}
}