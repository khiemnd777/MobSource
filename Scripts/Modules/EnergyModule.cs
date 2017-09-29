using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Mob
{
	public struct SyncEnergy {
		public float energy;
	}

	public class SyncEnergyField : SyncListStruct<SyncEnergy> { }

	public class EnergyModule : RaceModule
	{
		public float maxEnergy;

		[SyncVar(hook="OnChangeEnergy")]
		public float energy;

		[SyncVar]
		public SyncEnergyField syncEnergyField = new SyncEnergyField();

		public int dice;

		public float energyLabel;

		public override void Init ()
		{
			syncEnergyField.Add (new SyncEnergy {
				energy = energy
			});
		}

		void RefreshSyncEnergyField(){
			syncEnergyField [0] = new SyncEnergy {
				energy = energy
			};
		}

		[Command]
        public void CmdRollDice()
        {
			RollDice();
			RpcRollDiceCallback();
        }

		[ClientRpc]
		void RpcRollDiceCallback(){
			EventManager.TriggerEvent(Constants.EVENT_ENERGY_DICE_ROLLED, new { dice = dice, ownNetId = _race.netId.Value });
		}

        public void RollDice()
        {
			dice = Random.Range(1, 10);
			AddEnergy(dice);
        }

		public void AddEnergy (float e)
		{
			energy = Mathf.Min (energy + e, maxEnergy);
//			While ((inc, step) => {
//				energyLabel = Mathf.Min(energyLabel + inc, maxEnergy);
//			}, e, 1f);
			RefreshSyncEnergyField ();
		}

		public void SubtractEnergy (float e)
		{
			energy = Mathf.Max (energy - e, 0f);
			RefreshSyncEnergyField ();
//			While ((inc, step) => {
//				energyLabel = Mathf.Max(energyLabel - inc, 0f);
//			}, e, 1f);

//			var energyValue = GetMonoComponent<Text> (Constants.ATTACKER_ENERGY_LABEL);
//			if (energyValue != null) {
//				JumpEffectAndShowSubLabel (energyValue.transform, Constants.DECREASE_LABEL, e);
//			}
		}

		void OnChangeEnergy(float currentEnergy){
			EventManager.TriggerEvent (Constants.EVENT_ENERGY_CHANGED, new { energy = currentEnergy, ownNetId = _race.netId.Value });
		}
	}
}

