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

public partial class Lobby : MonoBehaviour {
    public static Lobby instance;
    private NetworkManager manager;
    private Dictionary<ulong, string> connectedClientsDict = new Dictionary<ulong, string>();
    public UnityEvent<string> onClientConnect;
    public ConnectedClientData[] connectedClients;
    private ConnectedClientData hostData;

    public UnityEvent onConnectionChange;
    public UnityEvent onGameStart;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        manager = NetworkManager.Singleton;
    }

    public void Host(string nick, int pic_id) {
        hostData = new ConnectedClientData(nick, pic_id);
        print("starting host");

        manager.ConnectionApprovalCallback += HostApprovalCheck;
        manager.OnClientDisconnectCallback += OnClientDisconnect;
        manager.StartHost();        

        UpdateClients();
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
        SendNetMessage("BJ_ConnectResult", str);
    }

    private void SendNetMessage(string msg_name, string msg) {
        print("Sending net message");
        using (var buffer = PooledNetworkBuffer.Get()) {
            using (var writer = PooledNetworkWriter.Get(buffer)) {
                writer.WriteString(msg);
                MLAPI.Messaging.CustomMessagingManager.SendNamedMessage(
                    msg_name, null, buffer);
            }
        }
    }

    public void SpawnPlayers(){        
        var prefab = manager.NetworkConfig.NetworkPrefabs[0].Prefab;

        var inst = Instantiate(prefab);
        var net_obj =  inst.GetComponent<NetworkObject>();
        net_obj.SpawnAsPlayerObject(manager.ServerClientId);
        
        foreach(var client_id in connectedClientsDict.Keys){
            inst = Instantiate(prefab);
            var v = Random.insideUnitCircle * 5;
            inst.transform.position = new Vector3(v.x, 1, v.y);
            net_obj =  inst.GetComponent<NetworkObject>();
            net_obj.SpawnAsPlayerObject(client_id);
        }

        SendNetMessage("BJ_StartGame", "14");
        onGameStart.Invoke();
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
