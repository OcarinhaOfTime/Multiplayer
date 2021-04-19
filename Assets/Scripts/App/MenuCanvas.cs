using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour {
    public Button host;
    public Button client;

    private void Start(){
        host.onClick.AddListener(() => TreeUI.instance.Navigate(0));
        client.onClick.AddListener(() => TreeUI.instance.Navigate(1));
    }
}
