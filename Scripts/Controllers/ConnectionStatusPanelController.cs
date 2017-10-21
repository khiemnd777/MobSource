using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class ConnectionStatusPanelController : MobNetworkBehaviour
    {
        [SerializeField]
        Button exitConnectionBtn;

        void Start(){
            gameObject.SetActive(false);
            
            
        }
    }
}