using UnityEngine;

namespace Mob
{
	public class StatDamageAdding : Affect
	{
		public float extraDamage;

		void Start(){
			own.GetModule<StatModule>(s => s.strength+=extraDamage);
			Destroy (gameObject);
		}
	}

	public class StatDamageAddingItem: Item
	{
		public float extraDamage;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<StatDamageAdding> (own, targets, d => d.extraDamage = extraDamage);
			return true;
		}	
	}

	public class StatDamageAddingBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "+" + extraDamage + " stat damage";
		}

		public float extraDamage;

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<StatDamageAddingItem> (who, price, quantity, e => e.extraDamage = extraDamage);
		}
	}
}

