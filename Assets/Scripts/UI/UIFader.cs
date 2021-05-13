using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VUtils;

[RequireComponent(typeof(CanvasGroup))]
public class UIFader : MonoBehaviour {
    private CanvasGroup canvasGroup;
    private Vector3 originalPos;
    private Vector3 downPos;
    public bool hideOnStart;

    private Coroutine appear;
    private Coroutine disappear;


    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        originalPos = transform.position;
        downPos = transform.position + Vector3.up * Screen.height * .05f;

        if (!hideOnStart)
            return;

        canvasGroup.alpha = 0;
        transform.position = downPos;
    }

    [ContextMenu("Appear")]
    public void Appear() {
        Appear(() => { });
    }

    [ContextMenu("Disappear")]
    public void Disappear() {
        Disappear(() => { });
    }

    public void Appear(Action onEnd) {
        if (disappear != null)
            StopCoroutine(disappear);

        appear = this.LerpRoutine(.5f, CoTween.SmoothStop2, (t) => {
            canvasGroup.alpha = t;
            transform.position = Vector3.Lerp(downPos, originalPos, t);
        }, onEnd);
    }

    public void Disappear(Action onEnd) {
        if (appear != null)
            StopCoroutine(appear);

        disappear = this.LerpRoutine(.25f, (t) => {
            canvasGroup.alpha = 1-t;
            transform.position = Vector3.Lerp(downPos, originalPos, 1-t);
        }, onEnd);
    }
}
