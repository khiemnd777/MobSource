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
            var battlePlayer = gamePlayer.GetComponent<BattlePlayer>();
            battlePlayer.characterType = mobLobbyPlayer.characterType;
            return true;
        }

        public override void OnClientSceneChanged(NetworkConnection conn){
            if(client.isConnected){
                OnLobbyClientEnter();
                foreach (var player in lobbySlots)
                {
                    if (player.IsNull())
                        continue;
                    // player.readyToBegin = true;
                    // player.OnClientEnterLobby();
                    base.OnClientSceneChanged(conn);
                    return;
                }
            }
            OnClientSceneChanged(conn);
        }
    }
}