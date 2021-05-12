using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProfilePic : MonoBehaviour {
    public Image pic;
    public UnityEvent onClick => button.onClick;
    public Button button;
    public void SetSprite(Sprite sprite){
        pic.sprite = sprite;
    }
}
