using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class MobNetworkLobbyManager : NetworkLobbyManager
    {
        public CharacterType characterType;
        public PlayerState playerState;

        public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer){
            // Debug.Log("OnLobbyServerSceneLoadedForPlayer");
            // var mobLobbyPlayer = lobbyPlayer.GetComponent<MobNetworkLobbyPlayer>();
            // var battlePlayer = gamePlayer.GetComponent<Race>();
            // // battlePlayer.characterType = mobLobbyPlayer.characterType;
            return true;
        }

        #region Starting a Host or a Client 

        public override void OnStartHost(){
            Debug.Log("OnStartHost");
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_START_HOST);
            playerState = PlayerState.WaitingConnection;
            base.OnStartHost();
        }

        // public override void OnLobbyStartHost(){
        //     Debug.Log("OnLobbyStartHost");
        //     base.OnLobbyStartHost();
        // }

        // public override void OnLobbyStartServer(){
        //     Debug.Log("OnLobbyStartServer");
        //     base.OnLobbyStartServer();
        // }

        // public override void OnServerConnect(NetworkConnection conn)
        // {
        //     Debug.Log("OnServerConnect");
        //     base.OnServerConnect(conn);
        // }

        // public override void OnLobbyServerConnect(NetworkConnection conn){
        //     Debug.Log("OnLobbyServerConnect");
        //     base.OnLobbyServerConnect(conn);
        // }

        public override void OnStartClient(NetworkClient lobbyClient)
        {
            Debug.Log("OnStartClient");
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_CONNECT);
            playerState = PlayerState.WaitingConnection;
            base.OnStartClient(lobbyClient);
        }

        // public override void OnLobbyStartClient(NetworkClient lobbyClient){
        //     Debug.Log("OnLobbyStartClient");
        //     base.OnLobbyStartClient(lobbyClient);
        // }

        // public override void OnClientConnect(NetworkConnection conn){
        //     Debug.Log("OnClientConnect");
        //     base.OnClientConnect(conn);
        // }

        // public override void OnLobbyClientConnect(NetworkConnection conn){
        //     Debug.Log("OnLobbyClientConnect");
        //     base.OnLobbyClientConnect(conn);
        // }

        // public override void OnLobbyClientEnter(){
        //     Debug.Log("OnLobbyClientEnter");
        //     base.OnLobbyClientEnter();
        // }

        // public override void OnServerReady(NetworkConnection conn){
        //     Debug.Log("OnServerReady");
        //     base.OnServerReady(conn);
        // }

        // public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        // {
        //     Debug.Log("OnServerAddPlayer");
        //     base.OnServerAddPlayer(conn, playerControllerId);
        // }

        public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
        {
            Debug.Log("OnLobbyServerCreateLobbyPlayer");
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_LOBBY_SERVER_CREATE_LOBBY_PLAYER);
            var lobbyGo = GameObject.Instantiate(lobbyPlayerPrefab, Vector3.zero, Quaternion.identity);
            var lobbyPlayer = lobbyGo.GetComponent<MobNetworkLobbyPlayer>();
            lobbyPlayer.characterType = characterType;
            return lobbyGo.gameObject;
        }

        #endregion

        public override void OnDropConnection(bool success, string extendedInfo){
            Debug.Log("OnDropConnection");
            switch(playerState){
                case PlayerState.Unknown:
                case PlayerState.Exiting:
                case PlayerState.InBattle:
                    EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_DROP_CONNECTION);
                    base.OnDropConnection(success, extendedInfo);
                break;
                default:
                break;
            }
        }

        // public override void OnClientNotReady(NetworkConnection conn){
        //     Debug.Log("OnClientNotReady");
        //     base.OnClientNotReady(conn);
        // }

        // public override void OnLobbyClientAddPlayerFailed(){
        //     Debug.Log("OnLobbyClientAddPlayerFailed");
        //     base.OnLobbyClientAddPlayerFailed();
        // }

        // public override void OnStopHost(){
        //     Debug.Log("OnStopHost");
        //     base.OnStopHost();
        // }

        // public override void OnLobbyClientExit(){
        //     Debug.Log("OnLobbyClientExit");
        //     base.OnLobbyClientExit();
        // }
    
        public override void OnServerDisconnect(NetworkConnection conn){
            Debug.Log("OnServerDisconnect");
            switch(playerState){
                case PlayerState.InBattle:
                    playerState = PlayerState.Unknown;
                    PlayerMatchMaker.instance.Exit();
                    break;
                default:
                    base.OnServerDisconnect(conn);
                    break;
            }
        }

        public override void OnClientDisconnect(NetworkConnection conn){
            Debug.Log("OnClientDisconnect");
            switch(playerState){
                case PlayerState.InBattle:
                    EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_DISCONNECT_IN_BATTLE, new { conn = conn });
                    StopClient();
                    playerState = PlayerState.Unknown;
                    break;
                case PlayerState.Unknown:
                case PlayerState.Exiting:
                    EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_DISCONNECT);
                    playerState = PlayerState.Unknown;
                    base.OnClientDisconnect(conn);
                break;
                case PlayerState.WaitingConnection:
                case PlayerState.FindingAppropriateBattle:
                    PlayerMatchMaker.instance.StartMatchMaker();
                    PlayerMatchMaker.instance.GetMatchList(0, 20, 0, 0, matches => {
                        PlayerMatchMaker.instance.CreateOrJoinMatch(matches, 2, "", 0, 0);
                    });
                break;
                default:
                    base.OnClientDisconnect(conn);
                break;
            }
        }

        public override void OnClientError(NetworkConnection conn, int errorCode){
            Debug.Log("OnClientError");
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_CLIENT_ERROR);
            playerState = PlayerState.Unknown;
            base.OnClientError(conn, errorCode);
        }

        // public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId){
        //     Debug.Log("OnLobbyServerPlayerRemoved");
        //     base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
        // }

        public override void OnLobbyStopHost(){
            Debug.Log("OnLobbyStopHost");
            EventManager.TriggerEvent(Constants.EVENT_CONNECTION_STATUS_ON_LOBBY_STOP_HOST);
            playerState = PlayerState.Unknown;
            base.OnLobbyStopHost();
        }

        // public override void OnLobbyClientDisconnect(NetworkConnection conn){
        //     Debug.Log("OnLobbyClientDisconnect");
        //     base.OnLobbyClientDisconnect(conn);
        // }

        // public override void OnLobbyStopClient(){
        //     Debug.Log("OnLobbyStopClient");
        //     base.OnLobbyStopClient();
        // }

        // public override void OnStopServer(){
        //     Debug.Log("OnStopServer");
        //     base.OnStopServer();
        // }

        public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
        {
            switch(characterType){
                default:
                case CharacterType.Swordman:
                case CharacterType.Mage:
                case CharacterType.Berserker:
                    return CreateCharacter<Swordman>(c => 
                    {
                        c.DefaultValue();
                        c.GetModule<GoldModule>(x =>
                        {
                            x.AddGold(10);
                        });
                        c.GetModule<EnergyModule>(x =>
                        {
                            x.maxEnergy = 12;
                            x.AddEnergy(0);
                        });
                        c.GetModule<StatModule>(x =>
                        {
                            x.point = 20;
                        });
                    });
            }
        }

        public override void OnLobbyServerPlayersReady(){
            var allready = lobbySlots.Count(x => !x.IsNull() && x.readyToBegin) == maxPlayers;
            if (allready){
                playerState = PlayerState.InBattle;
                for (int i = 0; i < lobbySlots.Length; ++i)
                {
                    if (lobbySlots[i] != null)
                    {
                        var lobbyPlayer = (MobNetworkLobbyPlayer) lobbySlots[i];
                        lobbyPlayer.TargetPlayerHasAlready(lobbyPlayer.connectionToClient);
                    }
                }
                StartCoroutine(ServerCountdownCoroutine());
            }
        }

        public override void OnClientSceneChanged(NetworkConnection conn){
            playerState = PlayerState.InBattle;
            base.OnClientSceneChanged(conn);
        }

        public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo){
            StartCoroutine(MobUtility.WaitInWhile(7f, () => {
                Debug.Log("OnMatchCreateInWhile");
                return matchMaker == null || !lobbySlots.Any(x => x.IsNull());
            }, () => {
                PlayerMatchMaker.instance.DestroyMatch(matchInfo.networkId, matchInfo.domain, () => {
                    playerState = PlayerState.WaitingConnection;
                    PlayerMatchMaker.instance.StartMatchMaker();
                    PlayerMatchMaker.instance.GetMatchList(0, 20, 0, 0, matches => {
                        PlayerMatchMaker.instance.CreateOrJoinMatch(matches, 2, "", 0, 0);
                    });
                });
            }));
            base.OnMatchCreate(success, extendedInfo, matchInfo);
        }

        GameObject CreateCharacter<T>(Action<T> predicate = null) where T : Race
		{
			var prefabObj = spawnPrefabs.SingleOrDefault(x => x.GetComponent<T>() != null);
			if(prefabObj.IsNull())
				return null;
            var go = GameObject.Instantiate(spawnPrefabs[0], Vector3.zero,
                Quaternion.identity);
            var character = go.GetComponent<T>();
            if(predicate != null)
                predicate.Invoke(character);
            return go;
		}

        IEnumerator ServerCountdownCoroutine()
        {
            var remainingTime = 3f;
            var floorTime = Mathf.FloorToInt(remainingTime);

            while (remainingTime > 0)
            {
                yield return null;

                remainingTime -= Time.deltaTime;
                var newFloorTime = Mathf.FloorToInt(remainingTime);

                if (newFloorTime != floorTime)
                {
                    floorTime = newFloorTime;

                    for (int i = 0; i < lobbySlots.Length; ++i)
                    {
                        if (lobbySlots[i] != null)
                        {
                            var lobbyPlayer = (MobNetworkLobbyPlayer) lobbySlots[i];
                            lobbyPlayer.TargetUpdateCountdown(lobbyPlayer.connectionToClient, floorTime);
                        }
                    }
                }
            }
            ServerChangeScene(playScene);
        }
        // End of function
    }
}