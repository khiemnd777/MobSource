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
        public static PlayerMatchMaker instance{
            get{
                return _instance ?? (_instance = new PlayerMatchMaker());
            }
        }

        MobNetworkLobbyManager networkManager;

        public PlayerMatchMaker()
        {
            networkManager = (MobNetworkLobbyManager) NetworkManager.singleton;
        }

        public void StartMatchMaker()
        {
            networkManager.StartMatchMaker();
        }

        public void StopMatchMaker(){
            networkManager.StopMatchMaker();
        }

        Action<List<MatchInfoSnapshot>> _onMatchListCallback;
        bool isMatchListCallbackWaiting;
        public void GetMatchList(int startPageNumber, int resultPageSize, int eloScoreTarget, int requestDomain, Action<List<MatchInfoSnapshot>> callback = null)
        {
            if (isMatchListCallbackWaiting)
                return;
            isMatchListCallbackWaiting = true;
            _onMatchListCallback = callback;
            networkManager.matchMaker.ListMatches(startPageNumber, resultPageSize, "", true, eloScoreTarget, requestDomain, OnMatchList);
        }

        Action<MatchInfo> _onMatchCreateCallback;
        bool isMatchCreateCallbackWaiting;
        public void CreateMatch(uint matchSize, string matchPassword, int eloScoreForMatch, int requestDomain, Action<MatchInfo> callback = null)
        {
            if(isMatchCreateCallbackWaiting)
                return;
            isMatchCreateCallbackWaiting = true;
            _onMatchCreateCallback = callback;
            var matchName = "match_" + Guid.NewGuid().ToString();
            networkManager.matchMaker.CreateMatch(matchName, matchSize, true, matchPassword, "", "", eloScoreForMatch, requestDomain, OnMatchCreate);
        }

        Action<MatchInfo> _onMatchJoinedCallback;
        bool isMatchJoinedCallbackWaiting;
        public void JoinMatch(NetworkID netId, string matchPassword, int eloScoreForClient, int requestDomain, Action<MatchInfo> callback = null)
        {
            if(isMatchJoinedCallbackWaiting)
                return;
            isMatchJoinedCallbackWaiting = true;
            _onMatchJoinedCallback = callback;
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
                if (networkManager.matchInfo != null && lastMatch.networkId.Equals(networkManager.matchInfo.networkId))
                {
                    return;
                }
                JoinMatch(lastMatch.networkId, matchPassword, eloScoreForMatch, requestDomain, callback);
            }
        }

        Action _onMatchDestroyCallback;
        bool isMatchDestroyCallbackWaiting;
        public void DestroyMatch(NetworkID netId, int requestDomain, Action callback) {
            if(isMatchDestroyCallbackWaiting)
                return;
            isMatchDestroyCallbackWaiting = true;
            _onMatchDestroyCallback = callback;
            networkManager.matchMaker.DestroyMatch(netId, requestDomain, OnMatchDestroy);
        }

        public void Exit(){
            if(networkManager.matchInfo != null){
                networkManager.playerState = PlayerState.Exiting;
                DestroyMatch(networkManager.matchInfo.networkId, networkManager.matchInfo.domain, () => {
                    StopMatchMaker();
                    networkManager.playerState = PlayerState.Unknown;
                });
            }
        }

        Action _onMatchDropConnectionCallback;
        bool isMatchDropConnectionCallbackWaiting;
        public void DropConnection(NetworkID netId, int requestDomain, Action callback){
            if(isMatchDropConnectionCallbackWaiting)
                return;
            isMatchDropConnectionCallbackWaiting = true;
            _onMatchDropConnectionCallback = callback;
            networkManager.matchMaker.DropConnection(netId, NodeID.Invalid, requestDomain, OnMatchDropConnection);
        }

        void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            if (!success)
            {
                isMatchListCallbackWaiting = false;
                return;
            }
            isMatchListCallbackWaiting = false;
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
                isMatchCreateCallbackWaiting = false;
                return;
            }
            isMatchCreateCallbackWaiting = false;
            if(_onMatchCreateCallback != null)
            {
                _onMatchCreateCallback.Invoke(matchInfo);
            }
            networkManager.matchInfo = matchInfo;
            MatchInfo hostInfo = matchInfo;
            // networkManager.StartHost(hostInfo);
            networkManager.OnMatchCreate(success, extendedInfo, matchInfo);
        }

        void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (!success)
            {
                isMatchJoinedCallbackWaiting = false;
                return;
            }
            isMatchJoinedCallbackWaiting = false;
            if(_onMatchJoinedCallback != null){
                _onMatchJoinedCallback.Invoke(matchInfo);
            }
            networkManager.matchInfo = matchInfo;
            MatchInfo hostInfo = matchInfo;
            // networkManager.StartClient(hostInfo);
            networkManager.OnMatchJoined(success, extendedInfo, matchInfo);
        }

        void OnMatchDestroy(bool success, string extendedInfo)
        {
            if(!success) 
            {
                isMatchDestroyCallbackWaiting = false;
                return;
            }
            isMatchDestroyCallbackWaiting = false;
            networkManager.matchInfo = null;
            networkManager.StopHost();
            networkManager.StopMatchMaker();
            if(_onMatchDestroyCallback != null){
                _onMatchDestroyCallback.Invoke();
            }
        }

        public void OnMatchDropConnection(bool success, string extendedInfo)
        {
            if(!success) 
            {
                isMatchDropConnectionCallbackWaiting = false;
                return;
            }
            isMatchDropConnectionCallbackWaiting = false;
            networkManager.matchInfo = null;
            if(_onMatchDropConnectionCallback != null){
                _onMatchDropConnectionCallback.Invoke();
            }
        }
    }
}