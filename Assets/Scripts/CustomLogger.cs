using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomLogger : MonoBehaviour {
    private static CustomLogger instance;
    public TMP_Text log;

    private void Awake(){
        instance = this;
    }

    private void InternalLog(string msg){
        log.text = msg;
    }
    public static void Log(string msg){
        instance.InternalLog(msg);
    }
}
