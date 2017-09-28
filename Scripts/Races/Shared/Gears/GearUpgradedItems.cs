using UnityEngine;
using System.Collections.Generic;

namespace Mob
{
	public class GearUpgradedItems
	{
		public static void Init(){
			Case1 ();
			Case2 ();
		}

		public static BoughtItem GetIn(BoughtItem[] whichCase){
			var index = Random.Range (0, whichCase.Length - 1);
			return whichCase [index];
		}

		static BoughtItem[] _case1;
		public static BoughtItem[] Case1(){
			if(_case1 != null)
				return _case1;
			var i = new List<BoughtItem> {
				BoughtItem.CreatePrimitive<AddDefendChanceBoughtItem> (x => {
					x.chance = .1f;
				})
				, BoughtItem.CreatePrimitive<AddHpChanceBoughtItem> (x => {
					x.chance = .1f;
				})
				, BoughtItem.CreatePrimitive<AddCriticalChangeBoughtItem> (x => x.chance = .1f)
				, BoughtItem.CreatePrimitive<AddMagicResistChanceBoughtItem> (x => x.chance = .1f)
				, BoughtItem.CreatePrimitive<AddDamageChanceBoughtItem> (x => x.chance = .1f)
				, BoughtItem.CreatePrimitive<AddMagicChanceBoughtItem> (x => x.chance = .1f)
				, BoughtItem.CreatePrimitive<AddStrengthPointBoughtItem> (x => x.point = 3f)
				, BoughtItem.CreatePrimitive<AddDexterityPointBoughtItem> (x => x.point = 3f)
				, BoughtItem.CreatePrimitive<AddIntelligentPointBoughtItem> (x => x.point = 3f)
				, BoughtItem.CreatePrimitive<AddVitalityPointBoughtItem> (x => x.point = 3f)
				, BoughtItem.CreatePrimitive<AddLuckPointBoughtItem> (x => x.point = 3f)
			};
			_case1 = i.ToArray();
			return _case1;	
		}

		static BoughtItem[] _case2;
		public static BoughtItem[] Case2(){
			if(_case2 != null)
				return _case2;
			var i = new List<BoughtItem> {
				BoughtItem.CreatePrimitive<AddDefendChanceBoughtItem> (x => x.chance = .2f)
				, BoughtItem.CreatePrimitive<AddHpChanceBoughtItem> (x => x.chance = .2f)
				, BoughtItem.CreatePrimitive<AddCriticalChangeBoughtItem> (x => x.chance = .2f)
				, BoughtItem.CreatePrimitive<AddMagicResistChanceBoughtItem> (x => x.chance = .2f)
				, BoughtItem.CreatePrimitive<AddDamageChanceBoughtItem> (x => x.chance = .2f)
				, BoughtItem.CreatePrimitive<AddMagicChanceBoughtItem> (x => x.chance = .2f)
				, BoughtItem.CreatePrimitive<AddStrengthPointBoughtItem> (x => x.point = 5f)
				, BoughtItem.CreatePrimitive<AddDexterityPointBoughtItem> (x => x.point = 5f)
				, BoughtItem.CreatePrimitive<AddIntelligentPointBoughtItem> (x => x.point = 5f)
				, BoughtItem.CreatePrimitive<AddVitalityPointBoughtItem> (x => x.point = 5f)
				, BoughtItem.CreatePrimitive<AddLuckPointBoughtItem> (x => x.point = 5f)
			};
			_case2 = i.ToArray();
			return _case2;	
		}
	}
}

