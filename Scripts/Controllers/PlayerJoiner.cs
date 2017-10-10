using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class PlayerJoiner : MobBehaviour
    {
        PlayerMatchMaker matchMaker;
        [SerializeField]
        Button createBattleBtn;

        void Start()
        {
            matchMaker = PlayerMatchMaker.instance;

            createBattleBtn.onClick.AddListener(()=>{
                matchMaker.StartMatchMaker();
                matchMaker.GetMatchList(0, 20, 0, 0, matches => {
                    matchMaker.CreateOrJoinMatch(matches, 2, "", 0, 0);
                });
                // matchMaker.CreateMatch(2, "", 0, 0);
            });
        }
    }
}