using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class StatHListItem : MobBehaviour
	{
		public StatType statType;
		public Text statText;
		public Text statValue;
		public Button addBtn;

		Race _character;
		StatModule _statModule;

		void Start(){
			addBtn.onClick.AddListener (() => {
				_statModule.CmdAddPoint(statType);
			});

			EventManager.StartListening(Constants.EVENT_STAT_STRENGTH_CHANGED, new Action<float, uint>((strength, ownNetId) => {
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				if(statType == StatType.Strength){
					PrepareItems ("Strength", strength);
				}
			}));

			EventManager.StartListening(Constants.EVENT_STAT_DEXTERITY_CHANGED, new Action<float, uint>((dexterity, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(statType == StatType.Dexterity){
					PrepareItems ("Dexterity", dexterity);
				}
			}));

			EventManager.StartListening(Constants.EVENT_STAT_INTELLIGENT_CHANGED, new Action<float, uint>((intelligent, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(statType == StatType.Intelligent){
					PrepareItems ("Intelligent", intelligent);
				}
			}));

			EventManager.StartListening(Constants.EVENT_STAT_VITALITY_CHANGED, new Action<float, uint>((vitality, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(statType == StatType.Vitality){
					PrepareItems ("Vitality", vitality);
				}
			}));

			EventManager.StartListening(Constants.EVENT_STAT_LUCK_CHANGED, new Action<float, uint>((luck, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(statType == StatType.Luck){
					PrepareItems ("Luck", luck);
				}
			}));
		}

		void Update(){
			if (!TryToConnect())
				return;
			InitItems ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (_character != null && _statModule != null)
					return true;
				_character = Race.GetLocalCharacter ();
				if (_character == null)
					return false;
				_statModule = _character.GetModule<StatModule> ();
				return false;
			});
		}

		bool _isInitItem;

		public void InitItems(){
			if (_isInitItem)
				return;
			_isInitItem = true;
			switch (statType) {
			case StatType.Strength:
				PrepareItems ("Strength", _statModule.strength);
				break;
			case StatType.Dexterity:
				PrepareItems ("Dexterity", _statModule.dexterity);
				break;
			case StatType.Intelligent:
				PrepareItems ("Intelligent", _statModule.intelligent);
				break;
			case StatType.Vitality:
				PrepareItems ("Vitality", _statModule.vitality);
				break;
			case StatType.Luck:
				PrepareItems ("Luck", _statModule.luck);
				break;
			default:
				break;
			}
		}

		void PrepareItems(string name, float value){
			statText.text = name + ":";
			statValue.text = Mathf.Floor(value).ToString();
		}
	}
}

