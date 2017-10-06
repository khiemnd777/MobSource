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
                manager.StartMatchMaker();
                manager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
                // StartCoroutine (joinOrCreate ());
            });
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            Debug.Log("Done waiting");
            if (manager.matchInfo == null) {
                if (matches.Count == 0) {
                    Debug.Log("Create");
                    manager.matchMaker.CreateMatch (manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
                } else {
                    Debug.Log ("Joining");
                    manager.matchMaker.JoinMatch (matches[0].networkId, "", "", "", 0, 0, manager.OnMatchJoined);
                }
            }
        }

        public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            manager.matchInfo = matchInfo;
        }

        public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            manager.matchInfo = matchInfo;
        }
    }
}