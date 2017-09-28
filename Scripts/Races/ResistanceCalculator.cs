using UnityEngine;

namespace Mob
{
	public class ResistanceCalculator
	{
		public static void HandleResistance(ref float extraResistance, Race own, Race target){
			var _ = float.MinValue;
			own.GetModule<AffectModule>(am => {
				am.GetSubAffects<IResistant>(a => {
					_ = Mathf.Max(_, a.HandleResistance(target));
				});
			});
			extraResistance = Mathf.Max (_, extraResistance);
		}
	}
}

