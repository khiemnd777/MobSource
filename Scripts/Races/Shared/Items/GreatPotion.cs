using System;
using System.Linq;
using UnityEngine;

namespace Mob
{
	public class GreatPotion : Affect
	{
		public float extraHp;

		public override void Init ()
		{
			timeToDestroy = 0f;
			gainPoint = 8f;
		}

		public override void Execute ()
		{
			Affect.CreatePrimitiveAndUse<HealthPowerRestoring> (own, targets, predicate: hp => hp.extraHp = extraHp);
		}
	}

	// Item
	public class GreatPotionItem: Item, ISelfUsable
	{
		public float extraHp;
		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<GreatPotion> (own, targets, x => {
				x.extraHp = extraHp;
			});
			return true;
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		protected override bool Interact ()
		{
			return EnoughEnergy () && EnoughLevel () && EnoughCooldown ();
		}
	}

	public class GreatPotionBoughtItem: BoughtItem 
	{
		public float extraHp;

		public override void Init ()
		{
			title = "Great potion";
			extraHp = 150f;
			price = 80f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/items => great_potion");
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<GreatPotionItem> (who, price, quantity, x=>{
				x.extraHp = extraHp;
				x.title = title;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			timeToDestroy = 5f;
			BuyAndUseImmediately<GreatPotionItem> (who, targets, price, x => {
				x.title = title;
				x.extraHp = extraHp;
				x.timeToDestroy = 2f;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}
	}
}

