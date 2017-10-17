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
    }
}