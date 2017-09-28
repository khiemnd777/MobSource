using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Mob
{
	public class StatCalculator
	{
		public static float Add(float value, int point, float percent){
			var delta = point * percent / 100f;
			var addingPoint = value + delta;
			return addingPoint;
		}

		public static int GeneratePoint(int upLevel, int point){
			return point * upLevel;
		}

		public static int[] InitProbability(params float[] percentStats) {
			return Probability.Initialize (percentStats);
		}

		public static IEnumerable<int?> GetStatWithProbability(int point, int[] arr, params float[] percentStats){
			for (var x = 0; x < point; x++) {	
				var index = Random.Range (0, arr.Length - 1);
				yield return arr [index];
			}

			yield return null;
		}

		public static IEnumerable<int?> GetStatWithProbability(int point , params float[] percentStats){
			//var arr = InitProbability (percentStats);
			var arr = Probability.Initialize(percentStats);
			for (var x = 0; x < point; x++) {	
				var index = Random.Range (0, arr.Length - 1);
				yield return arr [index];
			}

			yield return null;	
		}
	}
}