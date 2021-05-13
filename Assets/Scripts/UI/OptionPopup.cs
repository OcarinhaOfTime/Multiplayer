using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : MonoBehaviour {
    [SerializeField] private Button b1;
    [SerializeField] private Button b2;
    [SerializeField] private TMP_Text t1;
    [SerializeField] private TMP_Text t2;
    [SerializeField] private TMP_Text t;

    public UIFader fader { get; private set; }

    private void Awake() {
        fader = GetComponent<UIFader>();
    }
    public void Activate(string text, string tb1, string tb2, Action action1, Action action2) {
        gameObject.SetActive(true);
        t.text = text;
        ButtonSetup(b1, action1);
        ButtonSetup(b2, action2);
        t1.text = tb1;
        t2.text = tb2;
        fader.Appear();
    }

    private void ButtonSetup(Button b, Action action) {
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => fader.Disappear(() => gameObject.SetActive(false)));
        b.onClick.AddListener(() => action());
    }
}
