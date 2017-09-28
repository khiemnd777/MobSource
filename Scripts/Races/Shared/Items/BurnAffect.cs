using UnityEngine;

namespace Mob
{
	public class BurnAffect : Affect, IMagicalAttackingEventHandler, INegativeAffect
	{
		public float subtractHp = 0f;
		public int turnNumber = 3;

		#region IMagicalAttackingEventHandler implementation

		public float bonusDamage {
			get {
				return subtractHp;
			}
		}

		public void HandleAttack(Race target){
			
		}

		#endregion

		void Update(){
			foreach (var target in targets) {
				ExecuteInTurn(target, () => {
					Affect.Use(own, this);
					if(turn == turnNumber){
						Destroy(gameObject);
					}
				});	
			}
		}
	}

	public class BurnAffectItem: Item
	{
		public float subtractHp = 15f;
		public int turnNumber = 3; 

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<BurnAffect> (own, targets, x => {
				x.subtractHp = subtractHp;
				x.turnNumber = turnNumber;
			});
			return true;
		}
	}

	public class BurnAffectBoughtItem: BoughtItem
	{
		public override void Init ()
		{
			title = "Burn affect";
		}

		public float subtractHp = 15f;
		public int turnNumber = 3; 

		public override void Buy (Race who, float price = 0, int quantity = 0)
		{
			Buy<BurnAffectItem> (who, price, quantity, x => {
				x.subtractHp = subtractHp;
				x.turnNumber = turnNumber;
				x.title = title;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<BurnAffectItem> (who, targets, price, x => {
				x.subtractHp = subtractHp;
				x.turnNumber = turnNumber;
				x.title = title;
			});
		}
	}
}