using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class PlayerMatchMaker
    {
        static PlayerMatchMaker _instance;
        public static PlayerMatchMaker instance
        {
            get
            {
                return _instance ?? (_instance = new PlayerMatchMaker());
            }
        }

        MobNetworkLobbyManager networkManager;

        public PlayerMatchMaker()
        {
            networkManager = (MobNetworkLobbyManager)NetworkManager.singleton;
        }

        public void StartMatchMaker()
        {
            networkManager.StartMatchMaker();
        }

        public void StopMatchMaker()
        {
            networkManager.StopMatchMaker();
        }

        Action<List<MatchInfoSnapshot>> _onMatchListCallback;
        bool isMatchListCallbackWaiting;
        public void GetMatchList(int startPageNumber, int resultPageSize, int eloScoreTarget, int requestDomain, Action<List<MatchInfoSnapshot>> callback = null)
        {
            // if (isMatchListCallbackWaiting)
            //     return;
            // isMatchListCallbackWaiting = true;
            _onMatchListCallback = callback;
            if (networkManager.matchMaker == null) return;
            networkManager.matchMaker.ListMatches(startPageNumber, resultPageSize, "", true, eloScoreTarget, requestDomain, OnMatchList);
        }

        Action<MatchInfo> _onMatchCreateCallback;
        bool isMatchCreateCallbackWaiting;
        public void CreateMatch(uint matchSize, string matchPassword, int eloScoreForMatch, int requestDomain, Action<MatchInfo> callback = null)
        {
            // if (isMatchCreateCallbackWaiting)
            //     return;
            // isMatchCreateCallbackWaiting = true;
            _onMatchCreateCallback = callback;
            var matchName = "match" + Guid.NewGuid().ToString();
            if (networkManager.matchMaker == null) return;
            networkManager.maxPlayers = Convert.ToInt32(matchSize);
            networkManager.matchMaker.CreateMatch(matchName, matchSize, true, matchPassword, "", "", eloScoreForMatch, requestDomain, OnMatchCreate);
        }

        Action<MatchInfo> _onMatchJoinedCallback;
        bool isMatchJoinedCallbackWaiting;
        public void JoinMatch(NetworkID netId, string matchPassword, int eloScoreForClient, int requestDomain, Action<MatchInfo> callback = null)
        {
            // if (isMatchJoinedCallbackWaiting)
            //     return;
            // isMatchJoinedCallbackWaiting = true;
            _onMatchJoinedCallback = callback;
            if (networkManager.matchMaker == null) return;
            networkManager.matchMaker.JoinMatch(netId, matchPassword, "", "", eloScoreForClient, requestDomain, OnMatchJoined);
        }

        public void CreateOrJoinMatch(List<MatchInfoSnapshot> matches, uint matchSize, string matchPassword, int eloScoreForMatch, int requestDomain, Action<MatchInfo> callback = null)
        {
            if (matches.Count == 0)
            {
                CreateMatch(matchSize, matchPassword, eloScoreForMatch, requestDomain, callback);
            }
            else
            {
                var lastMatch = matches[matches.Count - 1];
                JoinMatch(lastMatch.networkId, matchPassword, eloScoreForMatch, requestDomain, callback);
                // foreach (var match in matches)
                // {
                //     if (networkManager.matchInfo != null && match.networkId.Equals(networkManager.matchInfo.networkId))
                //     {
                //         continue;
                //     }
                //     JoinMatch(match.networkId, matchPassword, eloScoreForMatch, requestDomain, callback);
                // }
            }
        }

        Action _onMatchDestroyCallback;
        bool isMatchDestroyCallbackWaiting;
        public void DestroyMatch(NetworkID netId, int requestDomain, Action callback = null)
        {
            // if (isMatchDestroyCallbackWaiting)
            //     return;
            // isMatchDestroyCallbackWaiting = true;
             _onMatchDestroyCallback = callback;
            // networkManager.StopHost();
            if (networkManager.matchMaker == null) return;
            networkManager.matchMaker.DestroyMatch(netId, requestDomain, OnDestroyMatch);
        }

        public void Exit(Action actionExit = null)
        {
            networkManager.playerState = PlayerState.Exiting;
            // networkManager.StopHost();
            // networkManager.StopClient();
            var currentMatchInfo = networkManager.matchInfo;
            if(currentMatchInfo == null){
                if(actionExit != null){
                    actionExit.Invoke();
                }
                return;
            }
            DropConnection(currentMatchInfo.networkId, currentMatchInfo.nodeId, currentMatchInfo.domain, actionExit);
            // StartMatchMaker();
            // GetMatchList(0, 20, 0, 0, matches =>
            // {
            //     // if (networkManager.matchInfo != null)
            //     // {
            //     //     if (matches.Count > 0)
            //     //     {
            //     //         foreach (var match in matches)
            //     //         {
            //     //             if (match.networkId == networkManager.matchInfo.networkId)
            //     //             {
            //     //                 DestroyMatch(networkManager.matchInfo.networkId, networkManager.matchInfo.domain, actionExit);
            //     //             }
            //     //             // else
            //     //             // {
            //     //             //     networkManager.StopClient();
            //     //             //     if (actionExit != null)
            //     //             //     {
            //     //             //         actionExit.Invoke();
            //     //             //     }
            //     //             //     // DropConnection(networkManager.matchInfo.networkId, networkManager.matchInfo.domain, actionExit);
            //     //             // }
            //     //         }
            //     //     }
            //     //     else
            //     //     {
            //     //         networkManager.StopClient();
            //     //         if (actionExit != null)
            //     //         {
            //     //             actionExit.Invoke();
            //     //         }
            //     //         // DropConnection(networkManager.matchInfo.networkId, networkManager.matchInfo.domain, actionExit);
            //     //     }
            //     // }
            //     networkManager.playerState = PlayerState.Unknown;
            // });
            // if (networkManager.matchMaker == null)
            // {
            //     networkManager.StopClient();
            // }
        }

        Action _onMatchDropConnectionCallback;
        bool isMatchDropConnectionCallbackWaiting;
        public void DropConnection(NetworkID netId, NodeID dropNodeId, int requestDomain, Action callback = null)
        {
            // if (isMatchDropConnectionCallbackWaiting)
            //     return;
            //isMatchDropConnectionCallbackWaiting = true;
            _onMatchDropConnectionCallback = callback;
            //networkManager.StopClient();
            networkManager.matchMaker.DropConnection(netId, dropNodeId, requestDomain, OnMatchDropConnection);
        }

        void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            if (!success)
            {
                Debug.Log("List match failed");
                // isMatchListCallbackWaiting = false;
                return;
            }
            // isMatchListCallbackWaiting = false;
            var wrappedMatches = matches.Where(x => x.currentSize > 0).ToList();
            if (_onMatchListCallback != null)
            {
                _onMatchListCallback.Invoke(wrappedMatches);
            }
        }

        void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (!success)
            {
                Debug.Log("Create match failed");
                //isMatchCreateCallbackWaiting = false;
                return;
            }
            //isMatchCreateCallbackWaiting = false;
            if (_onMatchCreateCallback != null)
            {
                _onMatchCreateCallback.Invoke(matchInfo);
            }
            MatchInfo hostInfo = matchInfo;
            NetworkServer.Listen(hostInfo, 9000);
            networkManager.StartHost(hostInfo);
            // networkManager.matchInfo = matchInfo;
            // networkManager.OnMatchCreate(success, extendedInfo, matchInfo);
        }

        void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (!success)
            {
                Debug.Log("Join match failed");
                // isMatchJoinedCallbackWaiting = false;
                return;
            }
            // isMatchJoinedCallbackWaiting = false;
            if (_onMatchJoinedCallback != null)
            {
                _onMatchJoinedCallback.Invoke(matchInfo);
            }
            MatchInfo hostInfo = matchInfo;
            networkManager.StartClient(hostInfo);
            // networkManager.matchInfo = matchInfo;
            // networkManager.OnMatchJoined(success, extendedInfo, matchInfo);
        }

        void OnDestroyMatch(bool success, string extendedInfo)
        {
            if (!success)
            {
                Debug.Log("Destroy match failed");
                //isMatchDestroyCallbackWaiting = false;
                return;
            }
            //isMatchDestroyCallbackWaiting = false;
            // networkManager.matchInfo = null;
            networkManager.OnDestroyMatch(success, extendedInfo);
            if (_onMatchDestroyCallback != null)
            {
                _onMatchDestroyCallback.Invoke();
            }
        }

        public void OnMatchDropConnection(bool success, string extendedInfo)
        {
            // if (!success)
            // {
            //     Debug.Log("Drop connection failed");
            //     //isMatchDropConnectionCallbackWaiting = false;
            //     return;
            // }
            //isMatchDropConnectionCallbackWaiting = false;
            // networkManager.matchInfo = null;
            // networkManager.StopHost();
            networkManager.StopClient();
            networkManager.playerState = PlayerState.Exited;
            if (_onMatchDropConnectionCallback != null)
            {
                _onMatchDropConnectionCallback.Invoke();
            }
        }
    }
}