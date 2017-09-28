using UnityEngine;

namespace Mob
{
	public class StatResistanceAdding : Affect
	{
		public float extraResistance;

		void Start(){
			own.GetModule<StatModule>(s => s.dexterity+=extraResistance);
			Destroy (gameObject);
		}
	}

	public class StatResistanceAddingItem: Item
	{
		public override void Init ()
		{
			title = "+" + extraResistance + " stat resistance";
		}

		public float extraResistance;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<StatResistanceAdding> (own, targets, x => x.extraResistance = extraResistance);
			return true;
		}	
	}

	public class StatResistanceAddingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraResistance + " stat resistance";
		}

		public float extraResistance;

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<StatResistanceAddingItem> (who, price, quantity, e => e.extraResistance = extraResistance);
		}
	}
}

