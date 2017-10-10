using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;
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

            // NetworkServer.AddPlayerForConnection(conn, _temp, playerControllerId);
            return _temp;
        }

        public override void OnLobbyServerPlayersReady(){
            var allready = lobbySlots.Count(x => !x.IsNull() && x.readyToBegin) == maxPlayers;
            if (allready)
                StartCoroutine(ServerCountdownCoroutine());
        }

        IEnumerator ServerCountdownCoroutine()
        {
            var remainingTime = 5f;
            var floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                int newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {//to avoid flooding the network of message, we only send a notice to client when the number of plain seconds change.
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {//there is maxPlayer slots, so some could be == null, need to test it before accessing!
                            (lobbySlots[i] as MobNetworkLobbyPlayer).RpcUpdateCountdown(floorTime);
                        }
                    }
                }
            }

            for (int i = 0; i < lobbySlots.Length; ++i)
            {
                if (lobbySlots[i] != null)
                {
                    (lobbySlots[i] as MobNetworkLobbyPlayer).RpcUpdateCountdown(0);
                }
            }

            ServerChangeScene(playScene);
        }

    }
}