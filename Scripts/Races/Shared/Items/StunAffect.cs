using System;

namespace Mob
{
	public class StunAffect : Affect, INegativeAffect
	{
		public int turnNumber = 1;

		void Update(){
			foreach(var target in targets){
				ExecuteInTurn (target, () => {
					if(turn <= turnNumber){
						target.AllowAttack(false);
					} else {
						target.AllowAttack(true);
						Destroy(gameObject);
					}
				});	
			}
		}
	}

	public class StunAffectItem: Item
	{
		public override void Init ()
		{
			title = "Stun in" + turnNumber + " turn";
		}

		public int turnNumber = 1;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<StunAffect> (own, targets, x => x.turnNumber = turnNumber);
			return true;
		}
	}

	public class StunAffectBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "Stun in" + turnNumber + " turn";
		}

		public int turnNumber = 1;

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<StunAffectItem> (who, price, quantity, e => e.turnNumber = turnNumber);
		}
	}
}