using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace Mob
{
	public struct SyncHp {
		public float hp;
		public float maxHp;
	}

	public class SyncHpField : SyncListStruct<SyncHp> { }
	
	public class HealthPowerModule : RaceModule
	{
//		[SyncVar(hook="OnChangeHp")]
		public float hp;

		public float hpEffect;

//		[SyncVar(hook="OnChangeMaxHp")]
		public float maxHp;

		[SyncVar]
		public SyncHpField syncHpField = new SyncHpField();

		public float maxHpEffect;

		public float hpPercent;

		public override void Init ()
		{
//			hpEffect = hp;
//			maxHpEffect = maxHp;
			syncHpField.Add (new SyncHp {
				hp = hp,
				maxHp = maxHp
			});
		}

		void RefreshSyncHpField(){
			syncHpField[0] = new SyncHp {
				hp = hp,
				maxHp = maxHp
			};
		}

		public void AddHp(float p){
			hp = Mathf.Min(hp + p, maxHp);
			RefreshSyncHpField ();
		}

		public void AddHpEffect(){
//			MathfLerp (hpEffect, hp, p => hpEffect = p);
//			While ((inc, step) => {
//				hpEffect = Mathf.Clamp(hpEffect + inc, hp, maxHp);
//			}, p);
		}

		public void SubtractHp(float p){
			hp = Mathf.Max(hp - p, 0f);
			RefreshSyncHpField ();
		}

		public void SubtractHpEffect(){
//			var p = hpEffect - hp;
//			MathfLerp (hpEffect, hp, (p) => hpEffect = p);
//			While ((inc, step) => {
//				hpEffect = Mathf.Max(hpEffect - inc, 0f);
//			}, p);
		}

		public void SetFullHp(){
			AddHp (maxHp);
		}

		public void SetFullHpEffect(){
//			AddHpEffect ();
		}

		public void SetMaxHp(float time = 1f, bool setFullHp = true){
			var upMaxHp = 1f;
			_race.GetModule<StatModule> (s => upMaxHp = s.maxHp);
			while (time > 0f) {
				maxHp += Mathf.Max(maxHp, upMaxHp);
				time--;
			}
			if(setFullHp)
				SetFullHp();
		}

		public void SetMaxHpEffect(bool setFullHp = true){
			While ((inc, step) => {
				maxHpEffect = Mathf.Min(maxHpEffect + inc, maxHp);
			}, maxHp - maxHpEffect);
			if(setFullHp)
				SetFullHpEffect();
		}

		void OnChangeHp(float currentHp){
			EventManager.TriggerEvent (Constants.EVENT_REFRESH_SYNC_HP, new { hp = currentHp, maxHp = maxHp, ownNetId = _race.netId.Value });
		}

		void OnChangeMaxHp(float currentMaxHp){
			EventManager.TriggerEvent(Constants.EVENT_REFRESH_SYNC_HP, new { hp = hp, maxHp = currentMaxHp, ownNetId = _race.netId.Value });
		}
	}
}

