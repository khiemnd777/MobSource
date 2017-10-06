using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class PlayerJoiner : MobBehaviour
    {
        NetworkLobbyManager manager;
        [SerializeField]
        Button createBattleBtn;

        void Start()
        {
            manager = (NetworkLobbyManager)NetworkManager.singleton;

            createBattleBtn.onClick.AddListener(()=>{
                // if(NetworkManager.singleton.matchInfo == null)
                    NetworkManager.singleton.StartMatchMaker();
                NetworkManager.singleton.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
                // StartCoroutine (joinOrCreate ());
            });
        }
        
        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            Debug.Log(matches.Count);
            // if (NetworkManager.singleton.matchInfo == null) {
                if (matches.Count == 0) {
                    Debug.Log("Create");
                    NetworkManager.singleton.matchMaker.CreateMatch (NetworkManager.singleton.matchName, NetworkManager.singleton.matchSize, true, "", "", "", 0, 0, OnMatchCreate);
                } else {
                    Debug.Log ("Joining");
                    NetworkManager.singleton.matchMaker.JoinMatch (matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
                }
            // }
        }

        public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            NetworkManager.singleton.matchInfo = matchInfo;
        }

        public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            NetworkManager.singleton.matchInfo = matchInfo;
        }
    }
}