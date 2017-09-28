using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mob
{
	public class GearStatController : MobBehaviour
	{
		public Text attackDamage;
		public Text magicDamage;
		public Text defend;
		public Text magicResist;
		public Text hp;
		public Text statPoint;

		public Race _character;
		GearModule _gearModule;

		void Start(){
			EventManager.StartListening (Constants.EVENT_RETURN_STAT_VALUE, new Action<string, float, uint> ((name, statVal, ownNetId) => {
				if(!TryToConnect())
					return;
				if(!_character.netId.Value.Equals(ownNetId))
					return;
				switch (name) {
				case "damage":
					attackDamage.text = Mathf.Floor (statVal).ToString ();
					break;
				case "magicAttack":
					magicDamage.text = Mathf.Floor (statVal).ToString ();
					break;
				case "defend":
					defend.text = Mathf.Floor (statVal).ToString ();
					break;
				case "magicResist":
					magicResist.text = Mathf.Floor (statVal).ToString ();
					break;
				case "hp":
					hp.text = Mathf.Floor (statVal).ToString ();
					break;
				case "point":
					statPoint.text = Mathf.Floor (statVal).ToString ();
					break;
				default:
					break;
				}
			}));
		}

		void Update(){
			if (!TryToConnect ())
				return;
		}

		bool TryToConnect(){
			return NetworkHelper.instance.TryToConnect (() => {
				if(_character != null && _gearModule != null)
					return true;
				_character = Race.GetLocalCharacter();
				if(_character == null)
					return false;
				_gearModule = _character.GetModule<GearModule>();
				return false;
			});
		}
	}
}

