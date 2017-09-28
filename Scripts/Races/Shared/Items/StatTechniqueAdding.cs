using UnityEngine;

namespace Mob
{
	public class StatTechniqueAdding : Affect
	{
		public float extraTechnique;

		void Start(){
			own.GetModule<StatModule>(s => s.intelligent+=extraTechnique);
			Destroy (gameObject);
		}
	}

	public class StatTechniqueAddingItem: Item
	{
		public override void Init ()
		{
			title = "+" + extraTechnique + " stat technique";
		}

		public float extraTechnique;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<StatTechniqueAdding> (own, targets, x => x.extraTechnique = extraTechnique);
			return true;
		}	
	}

	public class StatTechniqueAddingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraTechnique + " stat technique";
		}

		public float extraTechnique;

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<StatTechniqueAddingItem> (who, price, quantity, e => e.extraTechnique = extraTechnique);
		}
	}
}

