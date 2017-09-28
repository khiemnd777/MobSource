using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Mob
{
	public struct SyncGold {
		public float gold;
	}

	public class SyncGoldField : SyncListStruct<SyncGold> { }

	public class GoldModule : RaceModule
	{
		public float maxGold;

//		[SyncVar(hook="OnChangeGold")]
		public float gold;

		public float goldLabel;

		public SyncGoldField syncGoldField = new SyncGoldField ();

		public override void Init ()
		{
			syncGoldField.Add (new SyncGold {
				gold = gold
			});
		}

		void RefreshSyncGoldField(){
			syncGoldField [0] = new SyncGold {
				gold = gold
			};
		}

		public void AddGold (float e)
		{
			e *= 10f;
			gold = Mathf.Min (gold + e, maxGold);
//			While ((inc, step) => {
//				goldLabel = Mathf.Min(goldLabel + inc, maxGold);
//			}, e, 1f);
			RefreshSyncGoldField();
		}

		public void SubtractGold (float e)
		{
			gold = Mathf.Max (gold - e, 0f);
			RefreshSyncGoldField ();
//			While ((inc, step) => {
//				goldLabel = Mathf.Max(goldLabel - e, 0f);
//			}, e, 1f);
//
//			var goldValue = GetMonoComponent<Text> (Constants.ATTACKER_GOLD_LABEL);
//			JumpEffect (goldValue.transform, Vector3.one);
//			ShowSubLabel (Constants.DECREASE_LABEL, goldValue.transform, e);
		}

		void OnChangeGold(float currentGold){
			EventManager.TriggerEvent(Constants.EVENT_REFRESH_SYNC_GOLD, new { gold = currentGold, ownNetId = _race.netId.Value });
		}
	}
}

