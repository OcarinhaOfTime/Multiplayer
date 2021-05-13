using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HostCanvas : MonoBehaviour {
    public GameObject host_login;
    public GameObject host_menu;
    public EntryList players;
    private TreeWidget widget;
    private Lobby lobby;
    public LoginPanel loginPanel;

    private bool logged{
        set{
            host_login.SetActive(!value);
            host_menu.SetActive(value);
        }
    }

    private void Start(){
        widget = GetComponent<TreeWidget>();
        widget.onShow.AddListener(OnShow);
        lobby = Lobby.instance;
        lobby.onConnectionChange.AddListener(UpdateUI);
        loginPanel.onLogin = OnLogin;
    }

    private void OnLogin(string nick, int pic_id){
        logged = true;

        lobby.Host(nick, pic_id);
    }

    private void OnShow(){
        logged = false;
    }

    public void UpdateUI(){
        var entries = lobby.connectedClients.Select(v => v.nick).ToArray();
        var sprites = lobby.connectedClients.Select(
            v => ProfilePicPicker.instance[v.pic]).ToArray();
        players.SetEntries(entries, sprites);
    }    
}
