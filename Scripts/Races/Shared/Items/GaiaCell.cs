using UnityEngine;

namespace Mob
{
	public class GaiaCell : Affect, IRestorableHealthPower
	{
		void Start(){
//			HasAffect<GaiaCell> (own, () => {
//				Destroy(gameObject);
//			});
//			own.GetModule<StatModule>(s => s.resistance += 10f);
		}

		#region IRestorableHealthPower implementation

		public float RestoreHealthPower (float hp)
		{
			return hp + hp * .5f;
		}

		#endregion
	}

	public class GaiaCellItem: Item, ISelfUsable
	{
		public override void Init ()
		{
			title = "Gaia's cell";
		}

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitive<GaiaCell> (own, targets);
			return false;
		}
	}

	public class GaiaCellBoughtItem: BoughtItem 
	{
		public override void Init ()
		{
			title = "Gaia's cell";
		}

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<GaiaCellItem> (who, price, quantity);
		}
	}
}

