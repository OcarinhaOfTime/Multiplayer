using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MLAPI;
using UnityEngine.UI;
using System;
using TMPro;
using MLAPI.Transports.UNET;

public class HelloWorldManager : MonoBehaviour {
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
        transportLayer = manager.GetComponent<UNetTransport>();
        print($"ip: {transportLayer.ConnectAddress}");
        print($"ip: {transportLayer.ConnectPort}");
    }

    private void StartService(int i){
        transportLayer.ConnectAddress = ip_field.text;
        transportLayer.ConnectPort = int.Parse(port_field.text);    

        var services = new Action[]{
            () => manager.StartHost(),
            () => manager.StartClient(),
            () => manager.StartServer()
            };

        services[i]();
        var mode = modes[i];
        print($"We're now {mode}");
        var transp = manager.NetworkConfig.NetworkTransport.GetType().Name;
        status_txt.text = $"Transport: {transp}\nMode: {mode}";
        setupCanvas.gameObject.SetActive(false);
        serverCanvas.SetActive(true);
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
