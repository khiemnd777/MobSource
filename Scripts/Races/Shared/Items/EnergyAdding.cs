using UnityEngine;

namespace Mob
{
	public class EnergyAdding : Affect
	{
		public float extraEnergy;

		public override void Init ()
		{
			timeToDestroy = 0f;
		}

		public override void Execute ()
		{
			own.GetModule<EnergyModule>(e => e.AddEnergy(extraEnergy));
		}
	}

	public class EnergyAddingItem: Item
	{
		public float extraEnergy;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<EnergyAdding> (own, targets, e => e.extraEnergy = extraEnergy);
			return true;
		}	
	}

	public class EnergyAddingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraEnergy + " energy";
		}

		public float extraEnergy;

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<EnergyAddingItem> (who, price, quantity, e => {
				e.extraEnergy = extraEnergy;
				e.title = title;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<EnergyAddingItem> (who, targets, price, e => {
				e.extraEnergy = extraEnergy;
				e.title = title;
			});
		}
	}
}

