using UnityEngine;
using UnityEngine.UI;
using System;

namespace Mob
{
	public class EnergyLocalCharacterController : MobBehaviour
	{
		Text text;
		float energy;
		float currentEnergy;
		Race _character;
		EnergyModule _energyModule;

		void Start(){
			text = GetComponent<Text> ();
			text.text = "12e";
			EventManager.StartListening(Constants.EVENT_ENERGY_SUBTRACTING_EFFECT, new Action<EffectValueTransferModule>(SubtractEnergyEffect));
			EventManager.StartListening(Constants.EVENT_ENERGY_ADDING_EFFECT, new Action<EffectValueTransferModule>(AddEnergyEffect));
		}

		void Update(){
			if (!TryToConnect ())
				return;
			Init ();
			Draw ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _energyModule != null)
					return true;
				isInit = false;
				_character = Race.GetLocalCharacter ();
				if(_character == null)
					return false;
				_energyModule = _character.GetModule<EnergyModule>();
				return false;
			});
		}

		bool isInit;
		void Init(){
			if (isInit)
				return;
			energy = _energyModule.syncEnergyField [0].energy;
			isInit = true;
		}

		void Draw(){
			text.text = Mathf.FloorToInt(energy) + "e";
		}

		void SubtractEnergyEffect(EffectValueTransferModule evt){
			MathfLerp (energy, _energyModule.syncEnergyField [0].energy, (r) => {
				energy = r;
				Draw ();
			}, 0.5f);
			ShowSubLabel (Constants.DECREASE_LABEL, transform, evt.GetValue<float>("energy"), deltaTime: 0.5f);
		}

		void AddEnergyEffect(EffectValueTransferModule evt){
			MathfLerp (energy, _energyModule.syncEnergyField [0].energy, (r) => {
				energy = r;
				Draw ();
			}, 0.5f);
			ShowSubLabel (Constants.INCREASE_LABEL, transform, evt.GetValue<float>("energy"), deltaTime: 0.5f);
		}
	}
}

