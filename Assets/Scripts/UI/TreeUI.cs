using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeUI : MonoBehaviour {
    public static TreeUI instance;
    public TreeWidget root;
    public TreeWidget currentWidget;

    private void Awake(){
        instance = this;
    }

    private void Start(){
        var widgets = GetComponentsInChildren<TreeWidget>();
        foreach(var w in widgets){
            if(w.parent){
                w.parent.AddChild(w);
            }
            w.Hide();
        }
        currentWidget = root;
        currentWidget.Show();
    }

    public void Home(){
        print("Home");
        if(root.GetHashCode() == currentWidget.GetHashCode()){
            return;
        }
        currentWidget.Hide();
        currentWidget = root;
        currentWidget.Show();
    }

    public void Back(){
        print("Back");
        if(root.GetHashCode() == currentWidget.GetHashCode()){
            return;
        }

        currentWidget.Hide();
        currentWidget = currentWidget.parent;
        currentWidget.Show();
    }

    public void Navigate(int i){
        print($"Navigate {i}");
        if(currentWidget.childs.Count <= i){
            print("theres no such a child");
            return;
        }

        currentWidget.Hide();
        currentWidget = currentWidget.childs[i];
        currentWidget.Show();
    }

    public int next;
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            Home();
        }

        if(Input.GetKeyDown(KeyCode.Z)){
            Back();
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)){
            Navigate(0);
        }

        if(Input.GetKeyDown(KeyCode.Alpha2)){
            Navigate(1);
        }
    }
}
