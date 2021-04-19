using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeWidget : MonoBehaviour {
    public List<TreeWidget> childs = new List<TreeWidget>();
    public TreeWidget parent;
    private CanvasGroup canvasGroup;
    public UnityEvent onShow;
    public UnityEvent onHide;

    public void AddChild(TreeWidget child){
        childs.Add(child);
    }

    private void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(){
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);
        onShow.Invoke();
    }

    public void Hide(){
        canvasGroup.alpha = .2f;
        gameObject.SetActive(false);
        onHide.Invoke();
    }
}
