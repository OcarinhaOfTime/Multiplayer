using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Serialization.Pooled;
using MLAPI.Transports;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public partial class Lobby : MonoBehaviour{
    public void Join(string nick, int pp_id) {
        manager.OnClientConnectedCallback += OnClientConnected;
        print("starting join");
        var payload = $"{nick},{pp_id}";
        
        manager.NetworkConfig.ConnectionData = 
        System.Text.Encoding.UTF8.GetBytes(payload);  
        RegisterClientMessageHandlers();
        
        manager.StartClient();
    }
    private void OnClientConnected(ulong serverId) {
        print("we are OnClientConnected");
        onClientConnect.Invoke($"we are connected on {serverId}");
    }
    private void RegisterClientMessageHandlers() {
        RegisterClientMessageHandlers("BJ_ConnectResult", OnClientConnChange);
        RegisterClientMessageHandlers("BJ_StartGame", OnClientStartGame);
    }

    private void RegisterClientMessageHandlers(string msg_name, Action<string> msg_fn) {
        MLAPI.Messaging.CustomMessagingManager.RegisterNamedMessageHandler(msg_name, 
        (senderClientId, stream) => {
            print("receiving message from server");
            using (var reader = PooledNetworkReader.Get(stream)) {
                var msg = reader.ReadString().ToString();
                msg_fn(msg.ToString());
            }
        });
    }

    private void OnClientConnChange(string payload) {
        print($"we are OnClientConnChange {payload}");
        onClientConnect.Invoke($"Custom message {payload}");
        connectedClients = ConnectedClientData.ParseClients(payload);
        onConnectionChange.Invoke();
    }    

    private void OnClientStartGame(string payload){
        print($"we are OnClientStartGame {payload}");
        onGameStart.Invoke();
    }
}