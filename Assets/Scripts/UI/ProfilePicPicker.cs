using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePicPicker : MonoBehaviour {
    public static ProfilePicPicker instance;
    public static Action<int> picSelectionCallback;
    public Sprite[] sprites;
    public ProfilePic profilePicPrefab;
    public RectTransform content;
    private int lastSelected = 0;
    public Sprite lastSpriteSelected => sprites[lastSelected];
    private GameObject root;
    private void Awake(){
        root = transform.GetChild(0).gameObject;
        instance = this;
        for(int i=0; i<sprites.Length; i++){
            var pp = Instantiate(profilePicPrefab);
            pp.transform.SetParent(content, false);
            pp.SetSprite(sprites[i]);
            var _i = i;
            pp.onClick.AddListener(() => OnPicSelected(_i));
            pp.gameObject.SetActive(true);
        }

        root.SetActive(false);
    }

    private void OnPicSelected(int i){
        if(i == 4){
            OptionCheck();
            return;
        }
        
        SelectPic(i);
    }

    private void SelectPic(int i){
        lastSelected = i;
        root.SetActive(false);
        picSelectionCallback.Invoke(i);
    }

    private void OptionCheck(){
        PopupCanvas.instance.ActivateOptionPopup(
            "Are you sure you want pick this terrible picture?", 
            "Yes", "No",
            Really,
            () => SelectPic(lastSelected)
            );
    }

    private void Really(){
        PopupCanvas.instance.ActivateOptionPopup(
            "Really?", 
            "Yes", "No",
            GetOut,
            () => SelectPic(lastSelected)
            );
    }

    private void GetOut(){
        PopupCanvas.instance.ActivateNotificationPopup(
            "Why are you the way that you are?", Application.Quit);
    }

    public static void ShowPicker(Action<int> onSelected){
        instance.root.SetActive(true);
        picSelectionCallback = onSelected;
    }

    public Sprite this[int i]{
        get => instance.sprites[i];
    }
}
