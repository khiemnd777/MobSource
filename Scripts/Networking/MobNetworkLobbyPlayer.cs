using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class MobNetworkLobbyPlayer : NetworkLobbyPlayer
    {
        public CharacterType characterType;

        public override void OnClientEnterLobby()
        {
            readyToBegin = true;
        }

        public override void OnStartLocalPlayer(){
            if(this.isLocalPlayer)
                SendReadyToBeginMessage();
        }

        [ClientRpc]
        public void RpcUpdateCountdown(float timeCountDown){
            Debug.Log(timeCountDown);
        }

        [TargetRpc]
        public void TargetUpdateCountdown(NetworkConnection connection, int timeCountdown){
            Debug.Log(timeCountdown);
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_PLAYER_COUNTDOWN, new { countdown = timeCountdown });
        }

        [TargetRpc]
        public void TargetPlayerHasAlready(NetworkConnection connection){
            Debug.Log("Player has already to start");
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_PLAYER_HAS_ALREADY);
        }
    }
}