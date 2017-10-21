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
        [SerializeField]
        Button createBattleBtn;
        [SerializeField]
        Button exitBattleBtn;
        [SerializeField]
        Button swordmanSelectionBtn;
        [SerializeField]
        Button mageSelectionBtn;
        [SerializeField]
        Button berserkerSelectionBtn;

        MobNetworkLobbyManager lobbyManager;
        PlayerMatchMaker matchMaker;

        void Start()
        {
            matchMaker = PlayerMatchMaker.instance;
            lobbyManager = (MobNetworkLobbyManager) NetworkManager.singleton;
            createBattleBtn.onClick.AddListener(() => {
                matchMaker.StartMatchMaker();
                matchMaker.GetMatchList(0, 20, 0, 0, matches => {
                    matchMaker.CreateOrJoinMatch(matches, 2, "", 0, 0);
                });
            });

            exitBattleBtn.onClick.AddListener(() => {
                if(lobbyManager.matchInfo != null){
                    matchMaker.DestroyMatch(lobbyManager.matchInfo.networkId, lobbyManager.matchInfo.domain, () => {
                        matchMaker.StopMatchMaker();
                    });
                }
            });

            swordmanSelectionBtn.onClick.AddListener(() => {
                lobbyManager.characterType = CharacterType.Swordman;
            });

            mageSelectionBtn.onClick.AddListener(() => {
                lobbyManager.characterType = CharacterType.Mage;
            });
            
            berserkerSelectionBtn.onClick.AddListener(() => {
                lobbyManager.characterType = CharacterType.Berserker;
            });
        }
    }
}