using System.Collections;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Serialization.Pooled;
using MLAPI.Transports;
using MLAPI.Transports.UNET;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {
    public static Lobby instance;
    private NetworkManager manager;
    private Dictionary<ulong, string> connectedClientsDict = new Dictionary<ulong, string>();
    //public UnityEvent<string> onHostConnect;
    public UnityEvent<string> onClientConnect;
    public UnityEvent onConnectionChange;
    public string[] connectedClients;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        manager = NetworkManager.Singleton;
        manager.OnClientConnectedCallback += OnClientConnected;
    }

    public void Host() {
        print("starting host");

        manager.ConnectionApprovalCallback += HostApprovalCheck;
        manager.StartHost();

        UpdateClients();
    }

    public void Join(string nick, string ip) {
        print("starting join");
        
        manager.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(nick);  
        RegisterClientMessageHandlers();
        
        manager.StartClient();
    }

    private void HostApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback) {
        print("approving...");

        var payload = System.Text.Encoding.UTF8.GetString(connectionData);
        print($"connected {payload} \nClient: {clientId}");
        if (connectedClientsDict.ContainsKey(clientId)) {
            print(clientId + " is already in");
            return;
        }

        connectedClientsDict.Add(clientId, payload);

        callback(false, 0, true, Vector3.zero, Quaternion.identity);
        UpdateClients();    
        SendConnectionUpdate();
    }

    private void UpdateClients(){
        var list = new List<string>(new string[]{"Host"});
        list.AddRange(connectedClientsDict.Values);
        connectedClients = list.ToArray();
        onConnectionChange.Invoke();
    }

    private void SendConnectionUpdate(){
        var str = "";
        foreach(var client in connectedClients){
            str += $"{client},";
        }
        str = str.Remove(str.Length - 1);

        print($"updating clients {str}");
        SendNetMessage(str);
    }

    private void OnClientConnected(ulong serverId) {
        print("we are OnClientConnected");
        onClientConnect.Invoke($"we are connected on {serverId}");
    }

    private void OnClientMessage(string payload) {
        print($"we are OnClientMessage {payload}");
        onClientConnect.Invoke($"Custom message {payload}");
        connectedClients = payload.Split(',');
        onConnectionChange.Invoke();
    }

    private void RegisterClientMessageHandlers() {
        MLAPI.Messaging.CustomMessagingManager.RegisterNamedMessageHandler("BJ_ConnectResult", (senderClientId, stream) => {
            print("receiving message from server");
            using (var reader = PooledNetworkReader.Get(stream)) {
                var status = reader.ReadString();
                OnClientMessage(status.ToString());
            }
        });
    }

    private void SendNetMessage(string msg) {
        print("Sending net message");
        using (var buffer = PooledNetworkBuffer.Get()) {
            using (var writer = PooledNetworkWriter.Get(buffer)) {
                writer.WriteString(msg);
                MLAPI.Messaging.CustomMessagingManager.SendNamedMessage("BJ_ConnectResult", null, buffer);
            }
        }
    }

    // private void RegisterServerMessageHandlers()
    // {
    //     MLAPI.Messaging.CustomMessagingManager.RegisterNamedMessageHandler("C2S_SceneChanged", (senderClientId, stream) =>
    //     {
    //         using (var reader = PooledNetworkReader.Get(stream))
    //         {
    //             int sceneIndex = reader.ReadInt32();

    //             ClientSceneChanged?.Invoke(senderClientId, sceneIndex);
    //         }

    //     });
    // }
}
