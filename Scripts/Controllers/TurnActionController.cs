using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
    public class TurnActionController : MobBehaviour
    {
		public Text clockTxt;
		public Text turnNumberTxt;
		public Text turnTxt;
		public Button endTurnBtn;

		Race _character;
		CountdownNetworkModule _countdownModule;
		RefereeModule _refereeModule;

		void Start(){
			endTurnBtn.onClick.AddListener(EndTurn);

			EventManager.StartListening(Constants.EVENT_COUNTDOWN_CALLBACK, new Action<float, float, uint>((minutes, seconds, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
                    return;
				clockTxt.text = _countdownModule.Format(minutes, seconds);
			}));

			EventManager.StartListening(Constants.EVENT_COUNTDOWN_ENDED, new Action<uint>((ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
                    return;
			}));

			EventManager.StartListening(Constants.EVENT_TURN_NUMBER_GETTING, new Action<int, uint>((turnNumber, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
                    return;
				turnNumberTxt.text = turnNumber.ToString();
				if(turnNumber > 1)
					turnTxt.text = "Turns";
			}));
		}

		void Update(){
			if(!TryToConnect())
			return;
			Init();
		}

		bool isInit;
		void Init(){
			if(isInit)
                return;
				
            isInit = true;
		}

		void EndTurn(){
			_refereeModule.CmdEndTurn();
		}

		bool TryToConnect()
        {
            return NetworkHelper.instance.TryToConnect(() =>
            {
                if (!_character.IsNull() && !_countdownModule.IsNull() && !_refereeModule.IsNull())
                    return true;
                _character = Race.GetLocalCharacter();
                if (_character.IsNull())
                    return false;
                _countdownModule = _character.GetModule<CountdownNetworkModule>();
				_refereeModule = _character.GetModule<RefereeModule>();
                return false;
            });
        }
    }
}