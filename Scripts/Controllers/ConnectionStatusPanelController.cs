using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class ConnectionStatusPanelController : MobBehaviour
    {
        [SerializeField]
        Button exitConnectionBtn;
        [SerializeField]
        Text connectionStatusTxt;
        [SerializeField]
        RectTransform overlayPanel;

        Text exitConnectionTxt;
        MobNetworkLobbyManager lobbyManager;
        PlayerMatchMaker matchMaker;

        const string LOOKING_AN_APPROPRIATE_BATTLE = "Looking for an appropriate battle\nPlease wait";
        const string CONNECTING = "Connecting...";
        const string EXIT = "Exit";
        const string EXITING = "Exiting...";
        const string BATTLE_STAT_IN_SECONDS = "The battle is going to start in {0}";
        const string PLAYER_DISCONNECT = "Your opponent has been disconnected\nPlease to exit and try to connect again";
        const string CONNECTION_OCCURS_ERROR = "The connection has occured an error\nPlease to exit and try to connect again";

        void Start(){
            lobbyManager = (MobNetworkLobbyManager)NetworkManager.singleton;
            matchMaker = PlayerMatchMaker.instance;
            exitConnectionTxt = exitConnectionBtn.GetComponentInChildren<Text>();

            VisiblePanel(false);

            switch(lobbyManager.playerState){
                case PlayerState.WaitingConnection:
                    VisiblePanel(true);
                    connectionStatusTxt.text = LOOKING_AN_APPROPRIATE_BATTLE;
                    break;
                case PlayerState.Disconnected:
                    VisiblePanel(true);
                    connectionStatusTxt.text = PLAYER_DISCONNECT;
                    break;
                default:
                break;
            }

            exitConnectionBtn.onClick.AddListener(() => {
                exitConnectionTxt.text = EXITING;
                matchMaker.Exit(() => {
                    VisiblePanel(false);
                    exitConnectionTxt.text = EXIT;
                });
            });

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_STARTING_A_CONNECTION, new Action(()=>{
                connectionStatusTxt.text = CONNECTING;
                VisiblePanel(true);
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_START_HOST, new Action(() => {
                connectionStatusTxt.text = LOOKING_AN_APPROPRIATE_BATTLE;
                VisiblePanel(true);
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_CONNECT, new Action(() => {
                connectionStatusTxt.text = LOOKING_AN_APPROPRIATE_BATTLE;
                VisiblePanel(true);
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_PLAYER_COUNTDOWN, new Action<int>(countdown => {
                connectionStatusTxt.text = string.Format(BATTLE_STAT_IN_SECONDS, (countdown + 1));
                if(countdown < 0){
                    exitConnectionBtn.interactable = true;
                    VisiblePanel(false);
                }
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_SERVER_DISCONNECT, new Action(() => {
                connectionStatusTxt.text = PLAYER_DISCONNECT;
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_LOBBY_STOP_CLIENT, new Action(() => {
                connectionStatusTxt.text = PLAYER_DISCONNECT;
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_DISCONNECT, new Action(() => {
                connectionStatusTxt.text = CONNECTION_OCCURS_ERROR;
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_ERROR,  new Action(()=> {
                connectionStatusTxt.text = CONNECTION_OCCURS_ERROR;
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_DROP_CONNECTION, new Action(() => {
                connectionStatusTxt.text = PLAYER_DISCONNECT;
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_PLAYER_HAS_ALREADY, new Action(() => {
                exitConnectionBtn.interactable = false;
            }));
        }

        void VisiblePanel(bool visible){
            overlayPanel.gameObject.SetActive(visible);
            gameObject.SetActive(visible);
        }
    }
}