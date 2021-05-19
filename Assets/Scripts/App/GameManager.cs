using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        Lobby.instance.onGameStart.AddListener(OnGameStart);
    }

    private void OnGameStart(){
        print("Starting game");
        TreeUI.instance.gameObject.SetActive(false);
    }
}
