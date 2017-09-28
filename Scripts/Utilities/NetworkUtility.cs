using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
	public class NetworkHelper
	{
		public NetworkHelper(){
			
		}

		static NetworkHelper _instance;

		public static NetworkHelper instance{
			get { return _instance ?? (_instance = new NetworkHelper ()); }
		}

		public bool TryToConnect(Func<bool> predicate = null){
			if (NetworkManager.singleton == null)
				return false;
			if (NetworkManager.singleton.client == null)
				return false;
			if (!NetworkManager.singleton.client.isConnected)
				return false;
			if (predicate != null)
				return predicate.Invoke ();
			return false;
		}
	}
}

