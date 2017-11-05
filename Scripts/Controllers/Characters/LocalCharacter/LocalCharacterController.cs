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
		public HealthBar hpBar;
		public HealthBar epBar;
		public HealthBar expBar;

		Race _character;
		LevelModule _levelModule;
		HealthPowerModule _hpModule;
		GoldModule _goldModule;
		EnergyModule _energyModule;

		void Start(){
			// EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_LEVEL, new Action<int, uint>(RefreshLevel));
			// EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_UP_LEVEL, new Action<int, uint>(RefreshUpLevel));

			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_HP, new Action<float, float, uint>((hp, maxHp, ownNetId) => {
				if(!TryToConnect())
					return;
				if (_character.netId.Value != ownNetId)
				return;
				SetHpBar(hp, maxHp);
			}));

			EventManager.StartListening(Constants.EVENT_ENERGY_CHANGED, new Action<float, uint>((energy, ownNetId) =>
            {
                if (!TryToConnect())
                    return;
                if (_character.netId.Value != ownNetId)
                    return;
                SetEpBar(energy);
            }));

			EventManager.StartListening(Constants.EVENT_GAIN_POINT_CHANGED, new Action<float, int, uint>((gainPoint, level, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				SetExpBar(gainPoint, level);
			}));
		}

		void Update(){
			if (!TryToConnect ()) {
				return;
			}
			Init();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (!_character.IsNull() && !_levelModule.IsNull() && !_hpModule.IsNull() && !_energyModule.IsNull())
					return true;
				_character = Race.GetLocalCharacter ();
				if(_character.IsNull())
					return false;
				_levelModule = _character.GetModule<LevelModule>();
				_hpModule = _character.GetModule<HealthPowerModule>();
				_energyModule = _character.GetModule<EnergyModule>();
				return false;
			});
		}

		bool isInit;
		void Init(){
			if(isInit)
				return;

			RefreshLevel(_levelModule.level, _character.netId.Value);

			var syncHpField = _hpModule.syncHpField[0];
			SetHpBar(syncHpField.hp, syncHpField.maxHp);

			var syncEpField = _energyModule.syncEnergyField[0];
			SetEpBar(syncEpField.energy);

			isInit = true;
		}

		void SetHpBar(float hp, float maxHp){
			hpBar.SetValue(hp, maxHp);
		}

		void SetEpBar(float energy){
			epBar.SetValue(energy, 12f);
		}

		void SetExpBar(float gainPoint, int level){
			var maxPointAtLevel = LevelCalculator.GetMaxPointAt(level);
			var minPointAtLevel = LevelCalculator.GetMinPointAt(level);
			if(gainPoint > maxPointAtLevel){
				maxPointAtLevel = LevelCalculator.GetMaxPointAt(level + 1);
				minPointAtLevel = LevelCalculator.GetMinPointAt(level + 1);
			}
			var realRangeInBar = maxPointAtLevel - minPointAtLevel;
			var expPoint = gainPoint - minPointAtLevel;
			expBar.SetValue(expPoint, realRangeInBar, false);
			expBar.SetLabel(gainPoint, maxPointAtLevel);
		}


		void RefreshLevel(int level, uint ownNetId){
			if(!TryToConnect())
				return;
			if (_levelModule.IsNull())
				return;
			if (_character.netId.Value != ownNetId)
				return;
			if(this.level.IsNull())
				return;
			this.level.text = "Lv. " + _levelModule.level;
		}

		void RefreshUpLevel(int upLevel, uint ownNetId){
			if(!TryToConnect())
				return;
			if (_levelModule.IsNull())
				return;
			if (_character.netId.Value != ownNetId)
				return;
			ShowSubLabel (Constants.INCREASE_LABEL, this.level.transform, _levelModule.upLevel, deltaTime: 0.5f);
		}
	}
}

