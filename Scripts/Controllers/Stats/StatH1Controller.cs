using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Mob
{
	public class StatH1Controller : MobBehaviour
	{
		public Text pointValue;
		public Button autoPointBtn;
		public ScalableGridView list;
		public StatH1ListItem statListItemResource;

		Race _character;
		StatModule _statModule;

		void Start() {
			list.ClearAll ();

//			autoPointBtn.gameObject.SetActive(false);

			autoPointBtn.onClick.AddListener (() => {
				_statModule.CmdAutoAddPoint();
			});

			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_UP_LEVEL, new Action<int, uint> ((upLevel, ownNetId) => {
				if(_character.netId.Value != ownNetId)
					return;
			}));

			EventManager.StartListening (Constants.EVENT_STAT_AUTO_POINT_ADDED, new Action<uint> ((ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				autoPointBtn.gameObject.SetActive(false); 
			}));

			EventManager.StartListening (Constants.EVENT_REFRESH_SYNC_LEVEL, new Action<int, uint> ((level, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(level > 2){
					autoPointBtn.gameObject.SetActive(true);
				}
			}));

			EventManager.StartListening(Constants.EVENT_STAT_POINT_CHANGED, new Action<int, uint>((point, ownNetId) => {
				if(!TryToConnect())
					return;
				if(_character.netId.Value != ownNetId)
					return;
				if(point <=0){
					autoPointBtn.gameObject.SetActive(false);
				}
				pointValue.text = point.ToString();
			}));
		}

		void Update(){
			if (!TryToConnect())
				return;
			
			Init ();
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if (!_character.IsNull () && !_statModule.IsNull ())
					return true;
				_character = Race.GetLocalCharacter ();
				if (_character.IsNull ())
					return false;
				_statModule = _character.GetModule<StatModule> ();
				return false;
			});
		}

		bool isInit;
		void Init(){
			if (isInit)
				return;
			
			isInit = true;
			// Strength
			PrepareItem (StatType.Strength);
			// Dexterity
			PrepareItem (StatType.Dexterity);
			// Intelligent
			PrepareItem (StatType.Intelligent);
			// Vitality
			PrepareItem (StatType.Vitality);
			// Luck
			PrepareItem (StatType.Luck);

			pointValue.text = _statModule.point.ToString();
		}

		void PrepareItem(StatType statType){
			var ui = Instantiate<StatH1ListItem> (statListItemResource, list.transform);
			ui.statType = statType;
		}
	}
}

