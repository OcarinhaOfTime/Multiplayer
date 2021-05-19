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

public class Lobby : MonoBehaviour {
    public struct ConnectedClientData{
        public string nick;
        public int pic;

        public ConnectedClientData(string nick, int pic){
            this.nick = nick;
            this.pic = pic;
        }

        public ConnectedClientData(string payload){
            var split = payload.Split(',');
            nick = split[0];
            pic = int.Parse(split[1]);
        }

        public static ConnectedClientData[] ParseClients(string payload){
            var split = payload.Split(';');
            ConnectedClientData[] clients = new ConnectedClientData[split.Length];
            for(int i=0; i<split.Length; i++){
                clients[i] = new ConnectedClientData(split[i]);
            }

            return clients;
        }
    }
    public static Lobby instance;
    private NetworkManager manager;
    private Dictionary<ulong, string> connectedClientsDict = new Dictionary<ulong, string>();
    //public UnityEvent<string> onHostConnect;
    public UnityEvent<string> onClientConnect;
    public UnityEvent onConnectionChange;
    public ConnectedClientData[] connectedClients;
    private ConnectedClientData hostData;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        manager = NetworkManager.Singleton;
        manager.OnClientConnectedCallback += OnClientConnected;
    }

    public void Host(string nick, int pic_id) {
        hostData = new ConnectedClientData(nick, pic_id);
        print("starting host");

        manager.ConnectionApprovalCallback += HostApprovalCheck;
        manager.OnClientDisconnectCallback += OnClientDisconnect;
        manager.StartHost();        

        UpdateClients();
    }

    public void Join(string nick, int pp_id) {
        print("starting join");
        var payload = $"{nick},{pp_id}";
        
        manager.NetworkConfig.ConnectionData = 
        System.Text.Encoding.UTF8.GetBytes(payload);  
        RegisterClientMessageHandlers();
        
        manager.StartClient();
    }

    public void Disconnect(){        
        manager.StopClient();
    }

    private void OnClientDisconnect(ulong clientID){
        connectedClientsDict.Remove(clientID);
        UpdateClients();    
        SendConnectionUpdate();
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
        var clients = new List<ConnectedClientData>();
        clients.Add(hostData);

        clients.AddRange(connectedClientsDict.Values.Select(
            s => new ConnectedClientData(s)));
        connectedClients =  clients.ToArray();
        onConnectionChange.Invoke();
    }

    private void SendConnectionUpdate(){
        var str = $"";
        foreach(var client in connectedClients){
            str += $"{client.nick},{client.pic};";
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
        connectedClients = ConnectedClientData.ParseClients(payload);
        onConnectionChange.Invoke();
    }

    private void RegisterClientMessageHandlers() {
        MLAPI.Messaging.CustomMessagingManager.RegisterNamedMessageHandler("BJ_ConnectResult", 
        (senderClientId, stream) => {
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
                MLAPI.Messaging.CustomMessagingManager.SendNamedMessage(
                    "BJ_ConnectResult", null, buffer);
            }
        }
    }

    public void SpawnPlayers(){
        print("here?");
        var prefab = manager.NetworkConfig.NetworkPrefabs[0].Prefab;
        print("maybe here? " + prefab.name);
        // var net_obj = prefab.GetComponent<NetworkObject>();
        // print("perhaps here? " + net_obj.name);
        // foreach(var client_id in connectedClientsDict.Keys){
        //     net_obj.SpawnAsPlayerObject(client_id);
        // }        

        print("or even here?");
        var inst = Instantiate(prefab);
        var net_obj =  inst.GetComponent<NetworkObject>();
        net_obj.SpawnAsPlayerObject(manager.ServerClientId);
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
