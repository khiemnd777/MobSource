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
                EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_STARTING_A_CONNECTION);
                matchMaker.StartMatchMaker();
                matchMaker.GetMatchList(0, 20, 0, 0, matches => {
                    matchMaker.CreateOrJoinMatch(matches, 2, "", 0, 0);
                });
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