using System;
using System.Linq;
using UnityEngine;

namespace Mob
{
	public class Potion : Affect
	{
		public float extraHp;

		public override void Init ()
		{
			timeToDestroy = 0f;
			gainPoint = 5f;
		}

		public override void Execute ()
		{
			Affect.CreatePrimitiveAndUse<HealthPowerRestoring> (own, targets, hp => hp.extraHp = extraHp);
		}
	}

	// Item
	public class PotionItem: Item, ISelfUsable {
		
		public float extraHp;

		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<Potion> (own, targets, x => {
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

	public class PotionBoughtItem: BoughtItem 
	{
		public float extraHp;

		public override void Init ()
		{
			title = "Potion";
			extraHp = 50f;
			price = 40f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/items => potion");
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<PotionItem> (who, price, quantity, x => {
				x.title = title;
				x.extraHp = extraHp;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			timeToDestroy = 5f;
			BuyAndUseImmediately<PotionItem> (who, targets, price, x => {
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

