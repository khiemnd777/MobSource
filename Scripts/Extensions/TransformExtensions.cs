using UnityEngine;

namespace Mob
{
	public static class TransformExtensions
	{
		public static void MoveInHierarchy(this Transform transform, int delta){
			// delta: positive is forward, negative is backward
			var index = transform.GetSiblingIndex();
			transform.SetSiblingIndex (index + delta);
		}
	}
}

