using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mob
{
	public class GoldOpponentCharacterController : MobBehaviour
	{
		Text text;
		float gold;
		float currentGold;
		Race _character;
		GoldModule _goldModule;

		void Start(){
			text = GetComponent<Text> ();
			text.text = "12e";
			EventManager.StartListening(Constants.EVENT_GOLD_SUBTRACTING_EFFECT, new Action<EffectValueTransferModule>(SubtractGoldEffect));
			EventManager.StartListening(Constants.EVENT_GOLD_ADDING_EFFECT, new Action<EffectValueTransferModule>(AddGoldEffect));
		}

		void Update(){
			if (!TryToConnect ())
				return;
			Init ();
			Draw ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _goldModule != null)
					return true;
				isInit = false;
				_character = Race.GetOpponentCharacter ();
				if(_character == null)
					return false;
				_goldModule = _character.GetModule<GoldModule>();
				return false;
			});
		}

		bool isInit;
		void Init(){
			if (isInit)
				return;
			gold = _goldModule.syncGoldField [0].gold;
			isInit = true;
		}

		void Draw(){
			text.text = Mathf.FloorToInt(gold) + "g";
		}

		void SubtractGoldEffect(EffectValueTransferModule evt){
			MathfLerp (gold, _goldModule.syncGoldField [0].gold, (r) => {
				gold = r;
				Draw ();
			}, 0.5f);
			ShowSubLabel (Constants.DECREASE_LABEL, transform, evt.GetValue<float>("gold"), deltaTime: 0.5f);
		}

		void AddGoldEffect(EffectValueTransferModule evt){
			MathfLerp (gold, _goldModule.syncGoldField [0].gold, (r) => {
				gold = r;
				Draw ();
			}, 0.5f);
			ShowSubLabel (Constants.INCREASE_LABEL, transform, evt.GetValue<float>("gold"), deltaTime: 0.5f);
		}
	}
}