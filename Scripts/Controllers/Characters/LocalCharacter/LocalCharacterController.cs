using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mob
{
	public class LocalCharacterController : MobBehaviour
	{
		public Text className;
		public Text level;
		public Text gainPoint;

		Race _character;
		LevelModule _levelModule;
		HealthPowerModule _hpModule;
		GoldModule _goldModule;
		EnergyModule _energyModule;

		void Start(){
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_LEVEL, new Action<int, uint>(RefreshLevel));
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_UP_LEVEL, new Action<int, uint>(RefreshUpLevel));
		}

		void Update(){
			if (!TryToConnect ()) {
				isInitCharacterInfo = false;
				return;
			}
			InitCharacterInfo ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _levelModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if(_character == null)
					return false;
				_levelModule = _character.GetModule<LevelModule>();
				return false;
			});
		}

		bool isInitCharacterInfo;
		void InitCharacterInfo(){
			if (isInitCharacterInfo)
				return;
			className.text = _character.className;
			RefreshLevel (_levelModule.level, _character.netId.Value);
			level.text = "Lv. " + _levelModule.level;
//			gainPoint.text = _character.gainPoint + "/" + LevelCalculator.GetMaxPointAt (_levelModule.level);
			isInitCharacterInfo = true;
		}

		void RefreshLevel(int level, uint ownNetId){
			if (_levelModule == null)
				return;
			if (_character.netId.Value != ownNetId)
				return;
			this.level.text = "Lv. " + _levelModule.level;

		}

		void RefreshUpLevel(int upLevel, uint ownNetId){
			if (_levelModule == null)
				return;
			if (_character.netId.Value != ownNetId)
				return;
			ShowSubLabel (Constants.INCREASE_LABEL, this.level.transform, _levelModule.upLevel, deltaTime: 0.5f);
		}
	}
}

