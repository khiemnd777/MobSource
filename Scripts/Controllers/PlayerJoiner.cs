using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class PlayerJoiner : MobBehaviour
    {
        NetworkManager manager;

        void Start()
        {
            manager = NetworkManager.singleton;
            manager.StartMatchMaker();
            manager.matchMaker.ListMatches(0, 20, "", true, 0, 0, manager.OnMatchList);
            StartCoroutine (joinOrCreate ());
        }

        public IEnumerator joinOrCreate() {
            //waits for response from list matches
            yield return new WaitForSeconds (5f);
            Debug.Log("Done waiting");
            if (manager.matchInfo == null) {
                Debug.Log("Join or create?");
                if (manager.matches.Count == 0) {
                    Debug.Log("Create");
                    manager.matchMaker.CreateMatch (manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                } else {
                    Debug.Log ("Joining");
                    manager.matchMaker.JoinMatch (manager.matches[0].networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                }
            }
        }
    }
}