using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Mob
{
	public class Treasure
	{
		public static void Init(){
			Tier1 ();
			Tier2 ();
			Tier3 ();
		}

		public static BoughtItem[] GetFor(Race who) {
			var result = new BoughtItem[0];
			who.GetModule<LevelModule>(lv => {
				var level = lv.level;
				if(Mathf.Clamp(level, 1, 6) == level){
					result = GetTreasuresInTier(Tier1());
					return;
				} else if(Mathf.Clamp(level, 7, 9) == level){
					var p = Probability.Initialize(new float[]{80f, 20f});
					var i = Probability.GetValueInProbability(p);
					if(i == 0){
						result = GetTreasuresInTier(Tier1());
						return;
					}
					if(i == 1){
						result = GetTreasuresInTier(Tier2());
						return;
					}
				} else {
					var p = Probability.Initialize(new float[]{40f, 40f, 20f});
					var i = Probability.GetValueInProbability(p);
					if(i == 0){
						result = GetTreasuresInTier(Tier1());
						return;
					}
					if(i == 1){
						result = GetTreasuresInTier(Tier2());
						return;
					}
					if(i == 2){
						result = GetTreasuresInTier(Tier3());
						return;
					}
				}
			});
			return result;
		}

		static BoughtItem[] GetTreasuresInTier(BoughtItem[] tier){
			var items = new BoughtItem[3];
			var indexes = new int[3];
			for (var i = 0; i < items.Length; i++) {
				while (true) {
					var __index = Random.Range (0, tier.Length - 1);
					if (indexes.Length == 0 || !indexes.Contains (__index)) {
						indexes [i] = __index;
						break;
					}
				}
				items [i] = tier [indexes[i]];
			}
			return items;
		}

		static BoughtItem[] _tier1;
		static BoughtItem[] Tier1(){
			if (_tier1 != null)
				return _tier1;
			var list = new List<BoughtItem> {
				BoughtItem.CreatePrimitive<GoldAddingBoughtItem>(g => g.extraGold = 50f),
				BoughtItem.CreatePrimitive<EnergyAddingBoughtItem>(g => g.extraEnergy = 2f),
				BoughtItem.CreatePrimitive<HealthPowerRestoringBoughtItem>(hp => hp.extraHp = 50f),
				BoughtItem.CreatePrimitive<NegativeAffectDissolvingBoughtItem>(),
				BoughtItem.CreatePrimitive<StunAffectBoughtItem>(x => x.turnNumber = 1),
				BoughtItem.CreatePrimitive<StatDamageAddingBoughtItem>(x => x.extraDamage = 2f),
				BoughtItem.CreatePrimitive<StatResistanceAddingBoughtItem>(x => x.extraResistance = 2f),
				BoughtItem.CreatePrimitive<StatTechniqueAddingBoughtItem>(x => x.extraTechnique = 2f),
				BoughtItem.CreatePrimitive<StatLuckAddingBoughtItem>(x => x.extraLuck = 2f),
			};
			_tier1 = list.ToArray();
			return _tier1;
		}

		static BoughtItem[] _tier2;
		static BoughtItem[] Tier2(){
			if (_tier2 != null)
				return _tier2;
			var list = new List<BoughtItem> {
				BoughtItem.CreatePrimitive<GoldAddingBoughtItem>(g => g.extraGold = 80f),
				BoughtItem.CreatePrimitive<EnergyAddingBoughtItem>(g => g.extraEnergy = 3f),
				BoughtItem.CreatePrimitive<GreatPotionBoughtItem>(),
				BoughtItem.CreatePrimitive<BurstStrengthBoughtItem>(),
				BoughtItem.CreatePrimitive<SpeedyBoughtItem>(),
				BoughtItem.CreatePrimitive<AntidoteBoughtItem>(),
				BoughtItem.CreatePrimitive<HealthPowerRestoringBoughtItem>(hp => hp.extraHp = 100f),
				BoughtItem.CreatePrimitive<NegativeAffectDissolvingBoughtItem>(),
				BoughtItem.CreatePrimitive<StunAffectBoughtItem>(x => x.turnNumber = 1),
				BoughtItem.CreatePrimitive<BurnAffectBoughtItem>(x => {
					x.subtractHp = 15f;
					x.turnNumber = 3;
				}),
				BoughtItem.CreatePrimitive<StatDamageAddingBoughtItem>(x => x.extraDamage = 3f),
				BoughtItem.CreatePrimitive<StatResistanceAddingBoughtItem>(x => x.extraResistance = 3f),
				BoughtItem.CreatePrimitive<StatTechniqueAddingBoughtItem>(x => x.extraTechnique = 3f),
				BoughtItem.CreatePrimitive<StatLuckAddingBoughtItem>(x => x.extraLuck = 3f),
			};
			_tier2 = list.ToArray();
			return _tier2;
		}

		static BoughtItem[] _tier3;
		static BoughtItem[] Tier3(){
			if (_tier3 != null)
				return _tier3;
			var list = new List<BoughtItem> {
				BoughtItem.CreatePrimitive<GoldAddingBoughtItem>(g => g.extraGold = 150f),
				BoughtItem.CreatePrimitive<EnergyAddingBoughtItem>(g => g.extraEnergy = 5f),
				BoughtItem.CreatePrimitive<GreatPotionBoughtItem>(),
				BoughtItem.CreatePrimitive<BurstStrengthBoughtItem>(x => x.quantity = 2),
				BoughtItem.CreatePrimitive<SpeedyBoughtItem>(x => x.quantity = 2),
				BoughtItem.CreatePrimitive<AntidoteBoughtItem>(x => x.quantity = 2),
				BoughtItem.CreatePrimitive<HealthPowerRestoringBoughtItem>(hp => hp.extraHp = 180f),
				BoughtItem.CreatePrimitive<NegativeAffectDissolvingAndHealthPowerRestoringBoughtItem>(x => x.extraHp = 50f),
				BoughtItem.CreatePrimitive<StunAffectAndDamageDealingBoughtItem>(x => {
					x.turnNumber = 1;
					x.damage = 30f;
				}),
				BoughtItem.CreatePrimitive<BurnAffectBoughtItem>(x => {
					x.subtractHp = 30f;
					x.turnNumber = 3;
				}),
				BoughtItem.CreatePrimitive<HeartOfHestiaBoughtItem>(),
				BoughtItem.CreatePrimitive<BlessingOfAmphitriteBoughtItem>(),
				BoughtItem.CreatePrimitive<GaiaCellBoughtItem>(),
				BoughtItem.CreatePrimitive<StatDamageAddingBoughtItem>(x => x.extraDamage = 5f),
				BoughtItem.CreatePrimitive<StatResistanceAddingBoughtItem>(x => x.extraResistance = 5f),
				BoughtItem.CreatePrimitive<StatTechniqueAddingBoughtItem>(x => x.extraTechnique = 5f),
				BoughtItem.CreatePrimitive<StatLuckAddingBoughtItem>(x => x.extraLuck = 5f),
			};
			_tier3 = list.ToArray();
			return _tier3;
		}
	}
}

