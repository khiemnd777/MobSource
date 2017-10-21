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

        void Start(){
            lobbyManager = (MobNetworkLobbyManager)NetworkManager.singleton;
            matchMaker = PlayerMatchMaker.instance;
            exitConnectionTxt = exitConnectionBtn.GetComponentInChildren<Text>();

            VisiblePanel(lobbyManager.playerState == PlayerState.WaitingConnection);

            exitConnectionBtn.onClick.AddListener(() => {
                exitConnectionTxt.text = "Exiting...";
                matchMaker.Exit(() => {
                    VisiblePanel(false);
                    exitConnectionTxt.text = "Exit";
                });
            });

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_STARTING_A_CONNECTION, new Action(()=>{
                connectionStatusTxt.text = "Connecting...";
                VisiblePanel(true);
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_START_HOST, new Action(() => {
                connectionStatusTxt.text = "Looking for an appropriate battle\nPlease wait";
                VisiblePanel(true);
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_CONNECT, new Action(() => {
                connectionStatusTxt.text = "Looking for an appropriate battle\nPlease wait";
                VisiblePanel(true);
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_PLAYER_COUNTDOWN, new Action<int>(countdown => {
                connectionStatusTxt.text = string.Format("The battle is going to start in {0}", (countdown + 1));
                if(countdown < 0){
                    VisiblePanel(false);
                }
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_DISCONNECT, new Action(() => {
                connectionStatusTxt.text = "The connection has occured an error\nPlease to exit and try to connect again";
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_ERROR,  new Action(()=> {
                connectionStatusTxt.text = "The connection has occured an error\nPlease to exit and try to connect again";
            }));

            EventManager.StartListening(Constants.EVENT_CONNECTION_STATUS_ON_DROP_CONNECTION, new Action(() => {
                connectionStatusTxt.text = "The connection has occured an error\nPlease to exit and try to connect again";
            }));
        }

        void VisiblePanel(bool visible){
            overlayPanel.gameObject.SetActive(visible);
            gameObject.SetActive(visible);
        }
    }
}