using UnityEngine;

namespace Mob
{
	public class StatLuckAdding : Affect
	{
		public float extraLuck;

		void Start(){
			own.GetModule<StatModule>(s => s.luck+=extraLuck);
			Destroy (gameObject);
		}
	}

	public class StatLuckAddingItem: Item
	{
		public override void Init ()
		{
			title = "+" + extraLuck + " stat luck";
		}

		public float extraLuck;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<StatLuckAdding> (own, targets, l => l.extraLuck = extraLuck);
			return true;
		}	
	}

	public class StatLuckAddingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraLuck + " stat luck";
		}

		public float extraLuck;

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<StatLuckAddingItem> (who, price, quantity, e => e.extraLuck = extraLuck);
		}
	}
}

