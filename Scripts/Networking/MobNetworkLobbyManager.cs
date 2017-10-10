using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class MobNetworkLobbyManager : NetworkLobbyManager
    {
        static Dictionary<int, int> currentPlayers = new Dictionary<int, int>();

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer){
            // Debug.Log("OnLobbyServerSceneLoadedForPlayer");
            // var mobLobbyPlayer = lobbyPlayer.GetComponent<MobNetworkLobbyPlayer>();
            // var battlePlayer = gamePlayer.GetComponent<Race>();
            // // battlePlayer.characterType = mobLobbyPlayer.characterType;
            return true;
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            base.OnServerAddPlayer(conn, playerControllerId);
        }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            if (!currentPlayers.ContainsKey(conn.connectionId))
                currentPlayers.Add(conn.connectionId, 0);
            
            return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
        }

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            int index = currentPlayers[conn.connectionId];

            var _temp = (GameObject)GameObject.Instantiate(spawnPrefabs[0], Vector3.zero,
                Quaternion.identity);
            var swordmand = _temp.GetComponent<Swordman>();
            swordmand.DefaultValue();
            swordmand.GetModule<GoldModule>(x =>
            {
                x.AddGold(10);
            });
            swordmand.GetModule<EnergyModule>(x =>
            {
                x.maxEnergy = 12;
                x.AddEnergy(0);
            });
            swordmand.GetModule<StatModule>(x =>
            {
                x.point = 20;
            });
            // NetworkServer.SpawnWithClientAuthority(_temp, conn);
            // NetworkServer.AddPlayerForConnection(conn, _temp, playerControllerId);
            return _temp;
        }

        public override void OnClientSceneChanged(NetworkConnection conn){
            if(client.isConnected){
                foreach (var player in lobbySlots)
                {
                    if (player.IsNull())
                        continue;
                }
                return;
            }
            OnClientSceneChanged(conn);
        }
    }
}