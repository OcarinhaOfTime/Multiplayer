using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientCanvas : MonoBehaviour {
    private Lobby lobby;
    public GameObject connectionPanel;
    public GameObject connectedPanel;
    public TMP_Text log;
    public EntryList entryList;
    public Button disconnect;
    public LoginPanel loginPanel;

    private void Start(){
        var widget = GetComponent<TreeWidget>();
        widget.onShow.AddListener(OnShow);
        lobby = Lobby.instance;
        lobby.onClientConnect.AddListener(OnClientConnected);
        lobby.onConnectionChange.AddListener(OnConnectionChange);
        disconnect.onClick.AddListener(Disconnect);
        loginPanel.onLogin = OnLogin;
    }

    private void OnShow(){
        SetConnectedPanel(false);
    }

    private void OnLogin(string nick, int pp_id){
        lobby.Join(nick, pp_id);
        SetConnectedPanel(true);
    }

    private void Disconnect(){
        TreeUI.instance.Home();
        lobby.Disconnect();
    }

    private void SetConnectedPanel(bool b){
        connectionPanel.SetActive(!b);
        connectedPanel.SetActive(b);
        entryList.gameObject.SetActive(false);
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

        var entries = lobby.connectedClients.Select(v => v.nick).ToArray();
        var sprites = lobby.connectedClients.Select(
            v => ProfilePicPicker.instance[v.pic]).ToArray();
        entryList.SetEntries(entries, sprites);
    }
}
