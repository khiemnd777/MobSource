using System;

namespace Mob
{
	public class CalculatorUtility
	{
		public static float AddExtraValueByPercent(float value, float extra, float percent, int upgradeCount){
			return value + (extra - ((percent == 0f ? 0f: extra / (1f + percent)) * (upgradeCount == 0 ? 0 : 1)));
		}
	}
}

