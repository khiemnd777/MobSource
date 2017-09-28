using System;

namespace Mob
{
	public class SwordmanPattern : RacePattern
	{
		public override void Pick (string playerId)
		{
			Pick<Swordman> (playerId);
		}
	}
}

