using System;

namespace Mob
{
	public interface IMissingHandler
	{
		void HandleMissing(float damage, Race target);
	}
}

