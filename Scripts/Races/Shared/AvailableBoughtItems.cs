using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mob
{
	public class AvailableBoughtItem
	{	
		static List<BoughtItem> _list = new List<BoughtItem> ();
		
		public static void Init(){
			// Common items
			InitCommonItems();
			// Gear items
			InitGearItems();
			// Skills
			InitSwordmanSkills();
		}

		public static BoughtItem GetSingleBy(Func<BoughtItem, bool> predicate){
			return _list.SingleOrDefault (predicate);
		}

		public static BoughtItem[] GetManyBy(Func<BoughtItem, bool> predicate){
			return _list.Where (predicate).ToArray();
		}

		static void InitCommonItems(){
			_list.Add(BoughtItem.CreatePrimitive<PotionBoughtItem>(x => x.quantity = 99));
		}

		static void InitGearItems(){
			_list.Add(BoughtItem.CreatePrimitive<HelmBoughtItem> (x => x.inStoreState = InStoreState.Available));
			_list.Add(BoughtItem.CreatePrimitive<ArmorBoughtItem> (x => x.inStoreState = InStoreState.Available));
			_list.Add(BoughtItem.CreatePrimitive<ClothBoughtItem> (x => x.inStoreState = InStoreState.Available));
			_list.Add(BoughtItem.CreatePrimitive<SwordBoughtItem> (x => x.inStoreState = InStoreState.Available));
			_list.Add(BoughtItem.CreatePrimitive<StaffBoughtItem> (x => x.inStoreState = InStoreState.Available));
			_list.Add(BoughtItem.CreatePrimitive<RingBoughtItem> (x => x.inStoreState = InStoreState.Available));
		}

		static void InitSwordmanSkills(){
			_list.Add(BoughtItem.CreatePrimitive<SwordmanA1BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanB1BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanB2BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanB3BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanC1BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanC2BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanD1BoughtSkill>());
			_list.Add(BoughtItem.CreatePrimitive<SwordmanE1BoughtSkill>());	
		}
	}
}

