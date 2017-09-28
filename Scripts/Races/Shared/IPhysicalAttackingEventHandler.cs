using UnityEngine;
using System;
using System.Collections;

namespace Mob
{
	public interface IPhysicalAttackingEventHandler {
		float bonusDamage { get; }
		void HandleAttack(Race target);
	}
}

