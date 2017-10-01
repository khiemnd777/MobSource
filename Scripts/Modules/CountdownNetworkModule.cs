using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;

namespace Mob
{
	public class CountdownNetworkModule : RaceModule
	{
		public float timer = 90f;
		public float minutes { get { return _m; } }
		public string minutesString {
			get{
				return FormatMinutes(_m);
			}
		}
		public float seconds { get { return _s; } }
		public string secondsString{
			get{
				return FormatSeconds(_s);
			}
		}
		public bool isEnd { get { return _isEnd; } }

		float _t;
		float _m;
		float _s;
		bool _stop;
		bool _isEnd;

		public string FormatSeconds(float seconds){
			var pre = seconds < 10f ? "0" : "";
			return pre + seconds;
		}

		public string FormatMinutes(float minutes){
			var deltaTime = timer / 60f;
			var pre = deltaTime > 10f && minutes < 10f ? "0" : "";
			return pre + minutes;
		}

		public string Format(float minutes, float seconds){
			return FormatMinutes(minutes) + ":" + FormatSeconds(seconds);
		}

		IEnumerator Countingdown(){
			while (_t > -1f && !_stop) {
				_m = Mathf.Floor (_t / 60f);
				_s = Mathf.RoundToInt (_t % 60f);
				//_t = Mathf.Max(0f, --_t);
				--_t;
				RpcCountingdownCallback(_m, _s);
				yield return new WaitForSeconds (1f);
			}
			_isEnd = _t <= -1f;
			RpcCountdownEndedCallback();
			yield return null;
		}

		public void Run(){
			_isEnd = false;
			_stop = false;
			StartCoroutine (Countingdown ());
		}

		[ClientRpc]
		void RpcCountingdownCallback(float minutes, float seconds){
			EventManager.TriggerEvent(Constants.EVENT_COUNTDOWN_CALLBACK, new { minutes = minutes, seconds = seconds, ownNetId = _race.netId.Value });
		}

		[ClientRpc]
		void RpcCountdownEndedCallback(){
			EventManager.TriggerEvent(Constants.EVENT_COUNTDOWN_ENDED, new { ownNetId = _race.netId.Value });
		}

		public void RefreshAndRun(){
			Stop ();
			_t = timer;
			Run ();
		}

		[Command]
		public void CmdRefreshAndRun(){
			RefreshAndRun();
		}

		public void Stop(){
			StopAllCoroutines ();
			_stop = true;
		}

		[Command]
		public void CmdStop(){
			Stop();
			RpcStopCallback();
		}

		[ClientRpc]
		void RpcStopCallback(){
			EventManager.TriggerEvent(Constants.EVENT_COUNTDOWN_STOPPED, new { ownNetId = _race.netId.Value});
		}
	}
}

