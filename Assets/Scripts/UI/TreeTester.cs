using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeTester : MonoBehaviour {

    [ContextMenu("Test")]
    private void Test() {
        var tree = new TreeNode<string>("root");

        var node0 = new TreeNode<string>("child0");
        node0.SetParent(tree);

        var node1 = new TreeNode<string>("child1");
        node1.SetParent(tree);

        var node2 = new TreeNode<string>("child2");
        node2.SetParent(tree);

        var node = new TreeNode<string>("child11");
        node.SetParent(node1);

        var node12 = new TreeNode<string>("child12");
        node12.SetParent(node1);

        node = new TreeNode<string>("child13");
        node.SetParent(node1);

        node12.Remove();

        tree.Traverse(print);
    }
}
