using UnityEngine;

namespace Mob
{
    public class ConnectionStateHelper
    {
        ConnectionStateHelper _singleton;

        public ConnectionStateHelper singleton{
            get{
                return _singleton ?? (_singleton = new ConnectionStateHelper());
            }
        }
        public string status;        
    }
}