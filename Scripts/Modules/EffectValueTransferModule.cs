using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Collections.Generic;

namespace Mob
{
	public class EffectValueTransferModule : RaceModule
	{
		[SyncVar]
		public EffectValueNetworkTransfer effectValueTransfer = new EffectValueNetworkTransfer();

		public void Add(string key, object value){
			var syncVar = new EffectValueNetworkDelivery {
				key = key,
				value = value.ToString()
			};
			if (!effectValueTransfer.Any (x => x.key == key)) {
				effectValueTransfer.Add (syncVar);
				return;
			}
			var effectSyncVar = effectValueTransfer.FirstOrDefault (x => x.key == key);
			var effectSynVarIndex = effectValueTransfer.IndexOf (effectSyncVar);
			effectValueTransfer [effectSynVarIndex] = syncVar;
		}

		public T GetValue<T>(string key) {
			if (!effectValueTransfer.Any (x => x.key == key)) {
				return default(T);
			}
			var effectSyncVar = effectValueTransfer.FirstOrDefault (x => x.key == key);
			return (T)System.Convert.ChangeType(effectSyncVar.value, typeof(T));
		}

		public Dictionary<string, object> ToDictionary(){
			return effectValueTransfer.ToDictionary (x => x.key, y => System.Convert.ChangeType (y.value, typeof(object)));
		}
	}
}

