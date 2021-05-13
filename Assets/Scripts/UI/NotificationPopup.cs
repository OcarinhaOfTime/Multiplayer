using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour {
    [SerializeField] private Button ok;
    [SerializeField] private TMP_Text text;

    public UIFader fader { get; private set; }

    private void Awake() {
        fader = GetComponent<UIFader>();
    }
    public void Activate(string text, Action onEnd) {
        gameObject.SetActive(true);
        this.text.text = text;
        ok.onClick.RemoveAllListeners();
        ok.onClick.AddListener(() => fader.Disappear(() => gameObject.SetActive(false)));
        ok.onClick.AddListener(() => onEnd());
        fader.Appear();
    }
}
