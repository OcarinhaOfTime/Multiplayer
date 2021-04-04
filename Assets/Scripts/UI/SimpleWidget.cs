using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VUtils;

public class SimpleWidget : MonoBehaviour, ISimpleWidget {
    protected CanvasGroup canvasGroup;
    protected virtual void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();        
    }
    public IEnumerator Hide() {
        yield return this.LerpRoutine(.25f, t => canvasGroup.alpha = 1-t);
    }

    public IEnumerator Show() {
        yield return this.LerpRoutine(.5f, t => canvasGroup.alpha = t);
    }

    public void ShowForced() {
        canvasGroup.alpha = 1;
    }
}