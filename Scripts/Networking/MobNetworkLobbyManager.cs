using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class MobNetworkLobbyManager : NetworkLobbyManager
    {
        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer){
            var mobLobbyPlayer = lobbyPlayer.GetComponent<MobNetworkLobbyPlayer>();
            
            return true;
        }
    }
}