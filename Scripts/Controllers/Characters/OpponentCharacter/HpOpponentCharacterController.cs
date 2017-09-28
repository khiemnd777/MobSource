using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mob
{
	public class HpOpponentCharacterController : MobBehaviour
	{
		Text text;
		float hp;
		float currentHp;
		float maxHp;
		Race _character;
		HealthPowerModule _hpModule;

		void Start(){
			text = GetComponent<Text> ();
			text.text = "9999/9999";
			EventManager.StartListening(Constants.EVENT_HP_SUBTRACTING_EFFECT, new Action<EffectValueTransferModule, uint>(SubtractHpEffect));
			EventManager.StartListening(Constants.EVENT_HP_ADDING_EFFECT, new Action<EffectValueTransferModule, uint>(AddHpEffect));
		}

		void Update(){
			if (!TryToConnect ())
				return;
			Init ();
			Draw ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _hpModule != null)
					return true;
				isInit = false;
				_character = Race.GetOpponentCharacter ();
				if(_character == null)
					return false;
				_hpModule = _character.GetModule<HealthPowerModule>();
				return false;
			});
		}

		bool isInit;
		void Init(){
			if (isInit)
				return;
			hp = _hpModule.syncHpField [0].hp;
			maxHp = _hpModule.syncHpField [0].maxHp;
			isInit = true;
		}

		void Draw(){
			text.text = Mathf.FloorToInt(hp) + "/" + Mathf.FloorToInt(maxHp);
		}

		void SubtractHpEffect(EffectValueTransferModule evt, uint targetNetId){
			if (_character.netId.Value != targetNetId)
				return;
			MathfLerp (hp, _hpModule.syncHpField [0].hp, (r) => {
				hp = r;
				Draw ();
			}, 0.5f);
			ShowSubLabel (Constants.DECREASE_LABEL, transform, evt.GetValue<float>("damage"), deltaTime: 0.5f);
		}

		void AddHpEffect(EffectValueTransferModule evt, uint targetNetId){
			if (_character.netId.Value != targetNetId)
				return;
			MathfLerp (hp, _hpModule.syncHpField [0].hp, (r) => {
				hp = r;
				Draw ();
			}, 0.5f);
			ShowSubLabel (Constants.INCREASE_LABEL, transform, evt.GetValue<float>("damage"), deltaTime: 0.5f);
		}
	}
}

