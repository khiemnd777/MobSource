using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class Stat2ndHListItem : MobNetworkBehaviour
	{
		public Stat2ndType stat2ndType;
		public Text statText;
		public Text statValue;

		BattlePlayer _player;
		Race _character;
		StatModule _statModule;

		void Start(){
			EventManager.StartListening (Constants.EVENT_STAT_PHYSICAL_ATTACK_CHANGED, new Action<float, uint>((physicalAttack, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.PhysicalAttack){
					PrepareItems ("Physical attack", physicalAttack);	
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_PHYSICAL_DEFEND_CHANGED, new Action<float, uint>((physicalDefend, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.PhysicalDefend){
					PrepareItems ("Physical defend", physicalDefend);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_ATTACK_RATING_CHANGED, new Action<float, uint>((attackRating, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.AttackRating){
					PrepareItems ("Attack rating", attackRating);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_CRITICAL_RATING_CHANGED, new Action<float, uint>((criticalRating, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.CriticalRating){
					PrepareItems ("Critical rating", criticalRating);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_MAGIC_ATTACK_CHANGED, new Action<float, uint>((magicAttack, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.MagicAttack){
					PrepareItems ("Magic attack", magicAttack);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_MAGIC_RESIST_CHANGED, new Action<float, uint>((magicResist, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.MagicResist){
					PrepareItems ("Magic resist", magicResist);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_MAX_HP_CHANGED, new Action<float, uint>((maxHp, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.MaxHp){
					PrepareItems ("Max Hp", maxHp);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_REGENERATE_HP_CHANGED, new Action<float, uint>((regenerateHp, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.RegenerateHp){
					PrepareItems ("Regenerate Hp", regenerateHp);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_LUCK_DICE_CHANGED, new Action<float, uint>((luckDice, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.LuckReward){
					PrepareItems ("Luck dice", luckDice);
				}
			}));

			EventManager.StartListening (Constants.EVENT_STAT_LUCK_REWARD_CHANGED, new Action<float, uint>((luckReward, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(stat2ndType == Stat2ndType.LuckReward){
					PrepareItems ("Luck reward", luckReward);
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
			switch (stat2ndType) {
			case Stat2ndType.PhysicalAttack:
				PrepareItems ("Physical attack", _statModule.physicalAttack);
				break;
			case Stat2ndType.PhysicalDefend:
				PrepareItems ("Physical defend", _statModule.physicalDefend);
				break;
			case Stat2ndType.AttackRating:
				PrepareItems ("Attack rating", _statModule.attackRating);
				break;
			case Stat2ndType.CriticalRating:
				PrepareItems ("Critical rating", _statModule.criticalRating);
				break;
			case Stat2ndType.MagicAttack:
				PrepareItems ("Magic attack", _statModule.magicAttack);
				break;
			case Stat2ndType.MagicResist:
				PrepareItems ("Magic resist", _statModule.magicResist);
				break;
			case Stat2ndType.MaxHp:
				PrepareItems ("Max Hp", _statModule.maxHp);
				break;
			case Stat2ndType.RegenerateHp:
				PrepareItems ("Regenerate Hp", _statModule.regenerateHp);
				break;
			case Stat2ndType.LuckDice:
				PrepareItems ("Luck dice", _statModule.luckDice);
				break;
			case Stat2ndType.LuckReward:
				PrepareItems ("Luck reward", _statModule.luckReward);
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

