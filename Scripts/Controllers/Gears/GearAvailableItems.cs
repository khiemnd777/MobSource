using System.Collections.Generic;

namespace Mob
{
	public class GearAvailableItems
	{
		public static List<GearBoughtItem> list = new List<GearBoughtItem> ();

		public static void Init(){
			list.Add(BoughtItem.CreatePrimitive<HelmBoughtItem> (x => x.inStoreState = InStoreState.Available));
			list.Add(BoughtItem.CreatePrimitive<ArmorBoughtItem> (x => x.inStoreState = InStoreState.Available));
			list.Add(BoughtItem.CreatePrimitive<ClothBoughtItem> (x => x.inStoreState = InStoreState.Available));
			list.Add(BoughtItem.CreatePrimitive<SwordBoughtItem> (x => x.inStoreState = InStoreState.Available));
			list.Add(BoughtItem.CreatePrimitive<StaffBoughtItem> (x => x.inStoreState = InStoreState.Available));
			list.Add(BoughtItem.CreatePrimitive<RingBoughtItem> (x => x.inStoreState = InStoreState.Available));
		}
	}
}

