using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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