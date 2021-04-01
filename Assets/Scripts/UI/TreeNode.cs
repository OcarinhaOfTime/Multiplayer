using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<T> : IEnumerable<TreeNode<T>>{
    public TreeNode<T> parent;
    public List<TreeNode<T>> childs;

    public T Value;

    public TreeNode(T val){
        Value = val;
        childs = new List<TreeNode<T>>();
        parent = null;
    }

    public void SetParent(TreeNode<T> parent){
        this.parent = parent;
        parent.childs.Add(this);
    }

    public void Traverse(Action<T> onEach){
        onEach(this.Value);
        
        if(childs.Count <= 0){
            return;
        }

        foreach(var c in this){
            c.Traverse(onEach);
        }
    }

    public IEnumerator<TreeNode<T>> GetEnumerator() {
        foreach(var c in childs){
            yield return c;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        foreach(var c in childs){
            yield return c;
        }
    }
    
    public void Remove(){
        parent.childs.Remove(this);
    }
}
