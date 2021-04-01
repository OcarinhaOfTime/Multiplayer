using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAPI;
using UnityEngine.UI;
using System;
using TMPro;
using MLAPI.Transports.UNET;
using MLAPI.Spawning;

public class HelloWorldManager : MonoBehaviour {
    //192.168.8.2
    public Button host;
    public Button client;
    public Button server;
    public Button move;
    public TMP_Text status_txt;
    public GameObject setupCanvas;
    public GameObject serverCanvas;
    string[] modes = {"Host", "Client", "Server"};
    private NetworkManager manager;
    public TMP_InputField ip_field;
    public TMP_InputField port_field;
    private UNetTransport transportLayer;

    private void Start(){

        host.onClick.AddListener(() => StartService(0));
        client.onClick.AddListener(() => StartService(1));
        server.onClick.AddListener(() => StartService(2));

        move.onClick.AddListener(SubmitNewPosition);

        manager = NetworkManager.Singleton;
        manager.NetworkConfig.ConnectionApproval = true;
        transportLayer = manager.GetComponent<UNetTransport>();
        print($"ip: {transportLayer.ConnectAddress}");
        print($"port: {transportLayer.ConnectPort}");
    }

    private void StartService(int i){
        //transportLayer.ConnectAddress = ip_field.text;
        //transportLayer.ConnectPort = int.Parse(port_field.text);    

        var services = new Action[]{
            StartHost,
            StartClient,
            () => print("this isn't working yet.")
            };

        services[i]();
        var mode = modes[i];
        print($"We're now {mode}");
        var transp = manager.NetworkConfig.NetworkTransport.GetType().Name;
        status_txt.text = $"Transport: {transp}\nMode: {mode}";
        setupCanvas.gameObject.SetActive(false);
        serverCanvas.SetActive(true);
    }

    private void StartHost(){
        print("starting host");
        print($"ip: {transportLayer.ConnectAddress}");
        print($"port: {transportLayer.ConnectPort}");        
        manager.ConnectionApprovalCallback += ApprovalCheck;
        manager.StartHost();
    }

    private void StartClient(){
        CustomLogger.Log("Requesting host...");
        var payload = "sup fool;pw 1234";
        manager.NetworkConfig.ConnectionData = System.Text.Encoding.UTF8.GetBytes(payload);
        manager.StartClient();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback){
        print("approving...");
        // bool approve = true;
        // bool createPlayerObject = false;

        var payload = System.Text.Encoding.UTF8.GetString(connectionData);
        print($"connected {payload} \nClient: {clientId}");

        ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("MyPrefab");
        //callback(createPlayerObject, prefabHash, approve, Vector3.zero, Quaternion.identity);
    }

    private void SubmitNewPosition(){
        if(manager.ConnectedClients.TryGetValue(
            manager.LocalClientId, out var networkedClient)){
                var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
                if (player) {
                    player.Move();
                }
        }
    }
}
