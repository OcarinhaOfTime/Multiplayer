using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientCanvas : MonoBehaviour {
    public TMP_InputField nameIF;
    public TMP_InputField ipIF;

    public Button submit;
    private Lobby lobby;
    public GameObject connectionPanel;
    public GameObject connectedPanel;
    public TMP_Text log;
    public EntryList entryList;

    private void Start(){
        submit.onClick.AddListener(Submit);
        lobby = Lobby.instance;
        lobby.onClientConnect.AddListener(OnClientConnected);
        lobby.onConnectionChange.AddListener(OnConnectionChange);
    }

    private void SetConnectedPanel(bool b){
        connectionPanel.SetActive(!b);
        connectedPanel.SetActive(b);
    }

    private void Submit(){
        var payload = $"{nameIF.text};{ipIF.text}";
        lobby.Join(nameIF.text, ipIF.text);
        SetConnectedPanel(true);
    }

    private void OnClientConnected(string status){
        print("Client Canvas: OnClientConnected");
        log.text = status;
    }

    private void OnConnectionChange(){
        print("Client Canvas: OnConnectionChange");
        connectionPanel.SetActive(false);
        connectedPanel.SetActive(false);
        entryList.gameObject.SetActive(true);
        entryList.SetEntries(lobby.connectedClients);
    }
}
