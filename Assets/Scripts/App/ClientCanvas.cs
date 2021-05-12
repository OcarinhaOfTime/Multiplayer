using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientCanvas : MonoBehaviour {
    public TMP_InputField nameIF;

    public Button submit;
    private Lobby lobby;
    public GameObject connectionPanel;
    public GameObject connectedPanel;
    public TMP_Text log;
    public EntryList entryList;
    public ProfilePic profilePic;
    private int ppic_id;

    private void Start(){
        submit.onClick.AddListener(Submit);
        lobby = Lobby.instance;
        lobby.onClientConnect.AddListener(OnClientConnected);
        lobby.onConnectionChange.AddListener(OnConnectionChange);
        profilePic.onClick.AddListener(() => ProfileFicPicker.ShowPicker(OnPictureChange));
    }

    private void OnPictureChange(int ppic_id){
        this.ppic_id = ppic_id;
        profilePic.SetSprite(ProfileFicPicker.instance.lastSpriteSelected);
    }

    private void SetConnectedPanel(bool b){
        connectionPanel.SetActive(!b);
        connectedPanel.SetActive(b);
    }

    private void Submit(){
        var payload = $"{nameIF.text};{ppic_id}";
        lobby.Join(nameIF.text, ppic_id);
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

        var entries = lobby.connectedClients.Select(v => v.nick).ToArray();
        var sprites = lobby.connectedClients.Select(
            v => ProfileFicPicker.instance[v.pic]).ToArray();
        entryList.SetEntries(entries, sprites);
    }
}
