using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCanvas : MonoBehaviour {
    public GameObject block;
    public static PopupCanvas instance;

    public NotificationPopup notificationPopup;
    public OptionPopup optionPopup;

    private void Awake() {
        instance = this;
    }

    private Action Combine(params Action[] actions){
        return () => {
            foreach(var act in actions)
                act();
        };
    }

    public void ActivateNotificationPopup(string text, Action onEnd) {
        block.SetActive(true);
        var conEnd = Combine(() => block.SetActive(false), onEnd);

        notificationPopup.Activate(text, conEnd);
        notificationPopup.fader.Appear();
    }

    public void ActivateNotificationPopup(string text) {
        block.SetActive(true);
        var conEnd = Combine(() => block.SetActive(false));

        notificationPopup.Activate(text, conEnd);
        notificationPopup.fader.Appear();
    }

    public void ActivateOptionPopup(string text, string tb1, string tb2, Action action1, Action action2) {
        block.SetActive(true);
        var caction1 = Combine(() => block.SetActive(false), action1);
        var caction2 = Combine(() => block.SetActive(false), action2);

        optionPopup.Activate(text, tb1, tb2, caction1, caction2);
    }
}
