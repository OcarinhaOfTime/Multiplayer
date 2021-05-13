using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour {
    public ProfilePic pic;
    public Button submit_btn;
    public TMP_InputField submit_if;
    public Action<string, int> onLogin = (a, b) => {};
    private int pic_id;

    private void Awake(){
        pic.onClick.AddListener(() => ProfilePicPicker.ShowPicker(OnPicPicked));
        submit_btn.onClick.AddListener(() => onLogin(submit_if.text, pic_id));
    }

    private void OnPicPicked(int i){
        pic.SetSprite(ProfilePicPicker.instance.lastSpriteSelected);  
        pic_id = i;
    }
}
