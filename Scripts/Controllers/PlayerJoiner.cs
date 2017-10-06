using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class PlayerJoiner : MobBehaviour
    {
        NetworkManager manager;
        [SerializeField]
        Button createBattleBtn;

        void Start()
        {
            manager = NetworkManager.singleton;

            createBattleBtn.onClick.AddListener(()=>{
                manager.StartMatchMaker();
                manager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            });
        }
        
        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            Debug.Log(matches.Count);
            if (matches.Count == 0)
            {
                Debug.Log("Create");
                var matchName = "match-" + Guid.NewGuid().ToString();
                manager.matchMaker.CreateMatch(matchName, 2, true, "", "", "", 0, 0, OnMatchCreate);
            }
            else
            {
                Debug.Log("Joining");
                if (matches[0].networkId.Equals(manager.matchInfo.networkId))
                {
                    Debug.Log("Could not be joined");
                    return;
                }
                manager.matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
            }
        }

        public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if(!success){
                return;
            }
            MatchInfo hostInfo = matchInfo;
            // NetworkServer.Listen(hostInfo, 9000);
            manager.StartHost(hostInfo);
            manager.matchInfo = matchInfo;
        }

        public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if(!success){
                return;
            }
            MatchInfo hostInfo = matchInfo;
            manager.StartClient(hostInfo);
            manager.matchInfo = matchInfo;
        }
    }
}