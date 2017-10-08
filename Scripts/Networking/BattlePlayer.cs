using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mob
{
    public class BattlePlayer : MobNetworkBehaviour
    {
        public CharacterType characterType;
		[SerializeField]
		Race[] prefabs;
		Race character;

        void Start()
        {
            InitPlayer();
            InitCharacter();
        }

        void InitPlayer()
        {
            if (isServer)
            {
                tag = Constants.SERVER_PLAYER;
                //name = Constants.SERVER_PLAYER + netId;
                return;
            }
            if (isLocalPlayer)
            {
                tag = Constants.LOCAL_PLAYER;
                //name = Constants.LOCAL_PLAYER;
            }
            else if (isClient)
            {
                tag = Constants.OPPONENT_PLAYER;
                //name = Constants.OPPONENT_PLAYER;
            }
        }
		public override bool OnCheckObserver(NetworkConnection conn)
		{
			SpawnWithClientAuthority(character, gameObject);
			return true;
		}
        // Todo: For the testing purpose, we should use the default character
        void InitCharacter()
        {
            if (isServer)
            {
                switch (characterType)
                {
                    default:
                    case CharacterType.Swordman:
						CreateCharacter<Swordman>(p => {
							p.DefaultValue();
							p.playerNetId = netId.Value;
							p.GetModule<GoldModule>(x =>
							{
								x.AddGold(10);
							});
							p.GetModule<EnergyModule>(x =>
							{
								x.maxEnergy = 12;
								x.AddEnergy(0);
							});
							p.GetModule<StatModule>(x =>
							{
								x.point = 20;
							});
						});
                        break;
                    case CharacterType.Mage:
                        break;
                }
            }
        }

		void CreateCharacter<T>(Action<T> predicate) where T : Race
		{
			var prefabObj = prefabs.SingleOrDefault(x => typeof(T).IsEqual(x.GetType()));
			if(prefabObj.IsNull())
				return;
			character = Race.Create<T>((T)prefabObj, predicate);
		}

        void SpawnWithClientAuthority(Race race, GameObject player)
        {
            race.transform.SetParent(transform);
            NetworkServer.SpawnWithClientAuthority(race.gameObject, player);
        }

        public static BattlePlayer GetLocalPlayer()
        {
            var go = GameObject.FindGameObjectWithTag(Constants.LOCAL_PLAYER);
            return go != null ? go.GetComponent<BattlePlayer>() : null;
        }

        public static BattlePlayer GetOpponentPlayer()
        {
            var go = GameObject.FindGameObjectWithTag(Constants.OPPONENT_PLAYER);
            return go != null ? go.GetComponent<BattlePlayer>() : null;
        }

        public static BattlePlayer[] GetOpponentPlayers()
        {
            var go = GameObject.FindGameObjectsWithTag(Constants.OPPONENT_PLAYER);
            return go.Length > 0 ? go.Select(x => x.GetComponent<BattlePlayer>()).ToArray() : new BattlePlayer[0];
        }

        // Todo: This function below will be considered to remove.
        public static BattlePlayer[] GetOpponentsOf(BattlePlayer battlePlayer, NetworkConnection networkConnection)
        {
            var connections = NetworkServer.connections.ToArray();
            var connection = connections
                .FirstOrDefault(x => x.connectionId != networkConnection.connectionId);
            if (connection == null)
                return new BattlePlayer[0];
            return connection.playerControllers
                .Where(x => x.playerControllerId == battlePlayer.playerControllerId)
                .Select(x => x.gameObject.GetComponent<BattlePlayer>())
                .ToArray();
        }
    }
}