using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
	public class LevelModule : RaceModule 
	{
		// Level
		[SyncVar(hook="OnChangeLevel")]
		public int level;

		// Up level
		[SyncVar(hook="OnChangeUpLevel")]
		public int upLevel;

		// Max level
		public int maxLevel;

		// Seed to generate next level
		public float seed;

		int _dynamicLevel;
		
		void Update(){
			int up;
			LevelCalculator.maxLevel = maxLevel;
			LevelCalculator.seed = seed;
			_dynamicLevel = LevelCalculator.Up (_race.gainPoint, level, out up);
			if (_dynamicLevel > level) {
				level = _dynamicLevel;
				upLevel = up;

				GetModules<ILevelUpEventHandler> (x => {
					x.OnLevelUp(level, upLevel);
				});
			}
		}

		void OnChangeLevel(int currentLevel) {
			level = currentLevel;
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_LEVEL, new {level = currentLevel, ownNetId = _race.netId.Value});
		}

		void OnChangeUpLevel(int currentUpLevel) {
			upLevel = currentUpLevel;
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_UP_LEVEL, new {upLevel = currentUpLevel, ownNetId = _race.netId.Value});
		}
	}

	public interface ILevelUpEventHandler
	{
		void OnLevelUp(int level, int levelUp);
	}
}