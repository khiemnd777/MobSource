using UnityEngine;

namespace Mob
{
	public class LevelCalculator
	{
		public static float seed;
		public static int maxLevel;

		public static float GetPointAt(int level){
			var next = seed;
			var point = 0f;
			for (var i = 0; i < level; i++) {
				var temp = point;
				point = next;
				next += temp;
			}
			return point;
		}

		public static float GetMaxPointAt(int level){
			return GetPointAt(level + 1);
		}

		public static float GetMinPointAt(int level){
			return GetPointAt(level);
		}

		public static bool OutOfRangeLeft(float point){
			var min = GetMinPointAt (Constants.INIT_LEVEL);
			var max = GetMaxPointAt (maxLevel);
			return Mathf.Clamp(point + 1, min, max) == min;
		}

		public static bool OutOfRangeRight(float point){
			var min = GetMinPointAt (Constants.INIT_LEVEL);
			var max = GetMaxPointAt (maxLevel);
			return Mathf.Clamp(point, min, max) == max;
		}

		public static int Up(float point, int currentLevel, out int upLevel){
			upLevel = 0;
			if(OutOfRangeLeft(point)){
				return Constants.INIT_LEVEL;
			}
			var level = Constants.MIN_LEVEL;
			while (level < maxLevel) {
				var max = GetMaxPointAt (level);
				var min = GetMinPointAt (level);
				if (point == Mathf.Clamp(point, min, max - 1)) {
					break;
				}
				level++;
			}
			upLevel = level - currentLevel;
			return level;
		}
	}
}

