using System;
using System.Linq;
using UnityEngine;

namespace Mob
{
	public class BurstStrength : Affect, ICritical
	{
		public override void Init ()
		{
			gainPoint = 6f;
		}

		public override void Execute ()
		{
			
		}

		public override void EmitAffect (EmitAffectArgs args)
		{
			if (typeof(IPhysicalAttackingEventHandler).IsAssignableFrom (args.affect.GetType()) 
				|| typeof(IMagicalAttackingEventHandler).IsAssignableFrom (args.affect.GetType()) ) {
				Destroy (gameObject, 2f);
			}
		}
	}

	// Item
	public class BurstStrengthItem: Item, ISelfUsable
	{	
		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<BurstStrength> (own, targets);
			return true;
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		protected override bool Interact ()
		{
			return !Affect.HasAffect<BurstStrength> (own) && EnoughEnergy () && EnoughLevel () && EnoughCooldown ();
		}
	}

	public class BurstStrengthBoughtItem: BoughtItem 
	{
		public override void Init ()
		{
			title = "Burst strength";
			price = 40f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/items => burst_strength");
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<BurstStrengthItem> (who, price, quantity, x=>{
				x.title = title;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			BuyAndUseImmediately<BurstStrengthItem> (who, targets, price, x => {
				timeToDestroy = 5f;
				x.title = title;
				x.timeToDestroy = 2f;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}
	}
}

