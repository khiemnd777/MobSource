using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
    public class DiceActionController : MobBehaviour
    {
        public Text goldTxt;
        public Text energyTxt;
        public Text goldDiceTxt;
        public Text energyDiceTxt;
        public Button rollBtn;

        Race _character;
        GoldModule _goldModule;
        EnergyModule _energyModule;

        void Start()
        {
            rollBtn.onClick.AddListener(() =>
            {
                if (!TryToConnect())
                    return;
                _goldModule.CmdRollDice();
                _energyModule.CmdRollDice();
            });

            EventManager.StartListening(Constants.EVENT_GOLD_CHANGED, new Action<float, uint>((gold, ownNetId) =>
            {
                if (!TryToConnect())
                    return;
                if (_character.netId.Value != ownNetId)
                    return;
                goldTxt.text = Mathf.RoundToInt(gold).ToString();
            }));

            EventManager.StartListening(Constants.EVENT_ENERGY_CHANGED, new Action<float, uint>((energy, ownNetId) =>
            {
                if (!TryToConnect())
                    return;
                if (_character.netId.Value != ownNetId)
                    return;
                energyTxt.text = Mathf.RoundToInt(energy).ToString();
            }));

            EventManager.StartListening(Constants.EVENT_GOLD_DICE_ROLLED, new Action<int, uint>((dice, ownNetId) =>
            {
                if (!TryToConnect())
                    return;
                if (_character.netId.Value != ownNetId)
                    return;
                goldDiceTxt.text = dice.ToString();
            }));

            EventManager.StartListening(Constants.EVENT_ENERGY_DICE_ROLLED, new Action<int, uint>((dice, ownNetId) =>
            {
                if (!TryToConnect())
                    return;
                if (_character.netId.Value != ownNetId)
                    return;
                energyDiceTxt.text = dice.ToString();
            }));
        }

        void Update()
        {
            if (!TryToConnect())
                return;
            Init();
        }

        bool isInit;
        void Init()
        {
            if (isInit)
                return;
            // goldTxt.text = Mathf.RoundToInt(_goldModule.syncGoldField[0].gold).ToString();
            // energyTxt.text = Mathf.RoundToInt(_energyModule.syncEnergyField[0].energy).ToString();
            isInit = true;
        }

        bool TryToConnect()
        {
            return NetworkHelper.instance.TryToConnect(() =>
            {
                if (!_character.IsNull() && !_goldModule.IsNull() && !_energyModule.IsNull())
                    return true;
                _character = Race.GetLocalCharacter();
                if (_character.IsNull())
                    return false;
                _goldModule = _character.GetModule<GoldModule>();
                _energyModule = _character.GetModule<EnergyModule>();
                return false;
            });
        }
    }
}