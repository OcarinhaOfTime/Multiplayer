using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDataStructure<T> {
    LinkedListNode<T> root;
    LinkedList<T> list;
    public TreeDataStructure(T rootNode){
        root = new LinkedListNode<T>(rootNode);
        list = new LinkedList<T>();
        list.AddFirst(root);
    }

    public void Add(T element, LinkedListNode<T> parent){
        var newNode = new LinkedListNode<T>(element);
        list.AddAfter(parent, newNode);
    }

    public void Traverse(LinkedListNode<T> node, Action onEach){
        //node.List
    }
}
