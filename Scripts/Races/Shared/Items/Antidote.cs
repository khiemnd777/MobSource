using System;
using System.Linq;
using UnityEngine;

namespace Mob
{
	public class Antidote : Affect
	{
		public override void Init ()
		{
			timeToDestroy = 0f;
			gainPoint = 4f;
		}

		public override void Execute ()
		{
			own.GetModule<AffectModule>((a) => {
				a.RemoveAffect(m => typeof(INegativeAffect).IsAssignableFrom(m.GetType()));
			});
		}
	}

	// Item
	public class AntidoteItem: Item, ISelfUsable {
		
		public override bool Use (Race[] targets)
		{
			Affect.CreatePrimitiveAndUse<Antidote> (own, targets);
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

	public class AntidoteBoughtItem: BoughtItem 
	{
		public override void Init ()
		{
			title = "Antidote";
			price = 30f;

			icon.prefabs.Add ("none", "Sprites/icon");
			icon.prefabs.Add ("default", "Sprites/items => antidote");
		}

		public override string GetSyncIcon ()
		{
			return icon.prefabs.ContainsKey ("default") ? icon.prefabs ["default"] : icon.prefabs ["none"];
		}

		public override void Buy (Race who, float price, int quantity)
		{
			Buy<AntidoteItem> (who, price, quantity, x => {
				x.title = title;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}

		public override void BuyAndUseImmediately (Race who, Race[] targets, float price = 0)
		{
			timeToDestroy = 5f;
			BuyAndUseImmediately<AntidoteItem> (who, targets, price, x => {
				x.title = title;
				x.timeToDestroy = 2f;
				x.icon = icon;
			}, () => {
				this.price *= Constants.PRICE_UP_TO;
			});
		}
	}
}

