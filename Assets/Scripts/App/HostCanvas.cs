using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostCanvas : MonoBehaviour {
    public EntryList players;
    private TreeWidget widget;
    private Lobby lobby;

    private void Start(){
        widget = GetComponent<TreeWidget>();
        widget.onShow.AddListener(OnShow);
        lobby = Lobby.instance;
        lobby.onConnectionChange.AddListener(UpdateUI);
    }

    private void OnShow(){
        print("hosting");
        lobby.Host();
    }

    public void UpdateUI(){
        players.SetEntries(lobby.connectedClients);
    }
    
}
