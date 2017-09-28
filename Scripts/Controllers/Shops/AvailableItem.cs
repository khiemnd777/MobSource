using System.Collections.Generic;

namespace Mob
{
	public class AvailableItem
	{
		static AvailableItem _instance;

		public static AvailableItem instance{
			get {
				return _instance ?? (_instance = new AvailableItem());
			}
		}

		List<BoughtItem> _shopItems;

		public List<BoughtItem> shopItems {
			get {
				if (_shopItems != null)
					return _shopItems;
				
				_shopItems = new List<BoughtItem> ();
				_shopItems.Add(BoughtItem.CreatePrimitive<PotionBoughtItem>(x => x.quantity = 99));
				_shopItems.Add(BoughtItem.CreatePrimitive<GreatPotionBoughtItem>(x => x.quantity = 99));
				_shopItems.Add(BoughtItem.CreatePrimitive<AntidoteBoughtItem>(x => x.quantity = 99));
				_shopItems.Add(BoughtItem.CreatePrimitive<BurstStrengthBoughtItem>(x => x.quantity = 99));
				_shopItems.Add(BoughtItem.CreatePrimitive<SpeedyBoughtItem>(x => x.quantity = 99));

				return _shopItems;
			}
		}
	}
}

