using UnityEngine;

namespace Mob
{
	public abstract class Skill : Item
	{
		protected override bool Interact ()
		{
			return EnoughEnergy () && EnoughLevel () && EnoughCooldown ();
		}
	}
}

