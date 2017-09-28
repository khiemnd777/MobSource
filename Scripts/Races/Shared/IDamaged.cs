using UnityEngine;

namespace Mob
{
	public interface IDamaged
	{
		float HandleDamage(float damage, Race target);
	}
}

