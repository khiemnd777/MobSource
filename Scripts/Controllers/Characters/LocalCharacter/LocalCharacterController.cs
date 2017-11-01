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
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_LEVEL, new Action<int, uint>(RefreshLevel));
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_UP_LEVEL, new Action<int, uint>(RefreshUpLevel));
			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_HP, new Action<float, float, uint>((hp, maxHp, ownNetId) => {
				if(!TryToConnect())
					return;
				if (_character.netId.Value != ownNetId)
				return;
				AddHpBar(hp, maxHp);
			}));

			EventManager.StartListening (Constants.EVENT_CLIENT_ADD_HP, new Action<float, float, uint>((hp, maxHp, ownNetId) => {
				if(!TryToConnect())
					return;
				if (_character.netId.Value != ownNetId)
				return;
				AddHpBar(hp, maxHp);
			}));

			EventManager.StartListening (Constants.EVENT_CLIENT_SUBTRACT_HP, new Action<float, float, uint>((hp, maxHp, ownNetId) => {
				if(!TryToConnect())
					return;
				if (_character.netId.Value != ownNetId)
				return;
				SubtractHpBar(hp, maxHp);
			}));
		}

		void Update(){
			if (!TryToConnect ()) {
				return;
			}
			InitCharacterInfo ();
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

		bool isInitCharacterInfo;
		void InitCharacterInfo(){
			if (isInitCharacterInfo)
				return;
			// className.text = _character.className;
			RefreshLevel (_levelModule.level, _character.netId.Value);
//			gainPoint.text = _character.gainPoint + "/" + LevelCalculator.GetMaxPointAt (_levelModule.level);
			InitHpBar();
			InitEpBar();
			InitGainPoint();
			isInitCharacterInfo = true;
		}

		void InitGainPoint(){

		}

		void InitHpBar(){
			var syncField = _hpModule.syncHpField[0];
			hpBar.SetLabel(Mathf.FloorToInt(syncField.hp), Mathf.FloorToInt(syncField.maxHp));
			hpBar.Add(syncField.hp / syncField.maxHp, true);
		}

		void AddHpBar(float hp, float maxHp){
			hpBar.SetLabel(Mathf.FloorToInt(hp), Mathf.FloorToInt(maxHp));
			hpBar.Add(hp / maxHp, true);
		}

		void SubtractHpBar(float hp, float maxHp){
			hpBar.SetLabel(Mathf.FloorToInt(hp), Mathf.FloorToInt(maxHp));
			hpBar.Subtract(hp / maxHp, true);
		}

		void InitEpBar(){
			var syncField = _energyModule.syncEnergyField[0];
			epBar.SetLabel(Mathf.FloorToInt(syncField.energy), 12f);
			epBar.Add(syncField.energy / 12f, true);
		}

		void SetEpBar(float energy){
			epBar.SetLabel(energy, 12f);
			epBar.Add(energy / 12, true);
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

