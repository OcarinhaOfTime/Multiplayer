using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        var entries = lobby.connectedClients.Select(v => v.nick).ToArray();
        var sprites = lobby.connectedClients.Select(
            v => ProfileFicPicker.instance[v.pic]).ToArray();
        players.SetEntries(entries, sprites);
    }
    
}
