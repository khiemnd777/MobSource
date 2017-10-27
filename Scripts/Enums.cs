using System;

namespace Mob
{
	[Serializable]
	public enum GearType {
		Helm, Armor, Weapon, Cloth, Ring, Artifact
	}

	public enum InStoreState{
		Bought, Available
	}

	[Serializable]
	public enum CharacterType {
		Swordman, Mage, Berserker
	}

	public enum PlayerState{
		Unknown, FindingAppropriateBattle, WaitingConnection, InBattle, Exiting, Exited, ErrorConnection, Disconnected
	}
}

