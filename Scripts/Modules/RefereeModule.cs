using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mob
{
    public class RefereeModule : RaceModule
    {
        public int playerNumber;
        public Race[] characters;
        public Race turningCharacter;
        public uint turningCharacterNetId;
        public bool isPlaying;
        public bool isInTurn;

        void Start()
        {
            characters = new Race[2];
            EventManager.StartListening(Constants.EVENT_REFEREE_SERVER_ENDTURNED, new Action<Race>((turningCharacter) => {
                this.turningCharacter = turningCharacter;
                this.turningCharacterNetId = turningCharacter.netId.Value;
                isInTurn = _race.netId.Value == turningCharacter.netId.Value;
            }));
            EventManager.StartListening(Constants.EVENT_REFEREE_CLIENT_ENDTURNED, new Action<uint>((turningCharacterNetId) => {
                this.turningCharacterNetId = turningCharacterNetId;
                isInTurn = _race.netId.Value == turningCharacterNetId;
            }));
            EventManager.StartListening(Constants.EVENT_REFEREE_SERVER_JOINT, new Action<Race, uint>((character, ownNetId) =>{
                if(ownNetId == _race.netId.Value)
                    return;
                Join(character);
            }));
            EventManager.StartListening(Constants.EVENT_REFEREE_SERVER_PLAYED, new Action<Race>((turningCharacter) => {
                isPlaying = true;
                isInTurn = _race.netId.Value == turningCharacter.netId.Value;
            }));
            EventManager.StartListening(Constants.EVENT_REFEREE_CLIENT_PLAYED, new Action<uint>((turningCharacterNetId) => {
                isPlaying = true;
                isInTurn = _race.netId.Value == turningCharacterNetId;
            }));
        }

        public void EndTurn()
        {
            if(!isServer)
                return;
            for(var i = 0; i < characters.Length - 1; i++){
                var character = characters[i];
                if(turningCharacter.GetInstanceID() == character.GetInstanceID()){
                    var nextCharacter = characters[i == characters.Length - 1 ? 0 : i + 1];
                    turningCharacter = nextCharacter;
                    turningCharacterNetId = nextCharacter.netId.Value;
                    break;
                }
            }
            // Stop countdown
            _race.GetModule<CountdownNetworkModule>(x => {
                x.Stop();
            });
            // Callback end-turning in server
            EventManager.TriggerEvent(Constants.EVENT_REFEREE_SERVER_ENDTURNED, new { turningCharacter = this.turningCharacter });
            // Callback end-turning to client
            RpcEndTurnCallback(turningCharacter.netId.Value);
        }

        [Command]
        public void CmdEndTurn(){
            EndTurn();
        }

        [ClientRpc]
        void RpcEndTurnCallback(uint turningCharacterNetId){
            EventManager.TriggerEvent(Constants.EVENT_REFEREE_CLIENT_ENDTURNED, new { turningCharacterNetId = turningCharacterNetId});
        }

        public void AddCharacter(Race character)
        {
            if(characters.Length >= playerNumber)
                return;
            for(var i = 0; i < characters.Length - 1; i++){
                if(characters[i].IsNull())
                    characters[i] = character;
            }
        }

        public void Play(){
            if(characters.Length == 0 || characters.Length < playerNumber)
                return;
            var randomIndex = UnityEngine.Random.Range(0, characters.Length - 1); 
            var turningCharacter = characters[randomIndex];
            this.turningCharacter = turningCharacter;
            this.turningCharacterNetId = turningCharacter.netId.Value;
            EventManager.TriggerEvent(Constants.EVENT_REFEREE_SERVER_PLAYED, new { turningCharacter = turningCharacter });
            RpcPlayCallback(this.turningCharacterNetId);
        }

        [ClientRpc]
        void RpcPlayCallback(uint turningCharacterNetId){
            EventManager.TriggerEvent(Constants.EVENT_REFEREE_CLIENT_PLAYED, new { turningCharacterNetId = turningCharacterNetId });
        }

        [Command]
        public void CmdPlay(uint characterNetId){
            Play();
        }

        public void Join(Race character){
            if(!isServer)
                return;
            if(characters.Length >= playerNumber)
                return;
            if(character.IsNull())
                return;
            AddCharacter(character);
            EventManager.TriggerEvent(Constants.EVENT_REFEREE_SERVER_JOINT, new { character = character, ownNetId = _race.netId.Value });
            RpcJoinCallback(characters.Length);
        }

        [ClientRpc]
        void RpcJoinCallback(int jointNumber){
            EventManager.TriggerEvent(Constants.EVENT_REFEREE_CLIENT_JOINT, new { jointNumber = jointNumber });
        }

        [Command]
        public void CmdJoin(uint characterNetId){
            var character = Race.GetCharacterByNetId(characterNetId);
            Join(character);
        }
    }
}