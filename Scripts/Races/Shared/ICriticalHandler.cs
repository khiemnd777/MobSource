using UnityEngine;

namespace Mob
{
	public interface ICriticalHandler
	{
		float HandleCriticalDamage(float damage, Race target);	
	}
}

