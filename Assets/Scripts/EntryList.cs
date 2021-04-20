using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntryList : MonoBehaviour {
    public RectTransform content;
    public GameObject prefab;
    private List<GameObject> entries = new List<GameObject>();

    private void Start(){
        //Add("A Player");
        //Add("Another Player");
        //Add("Another Player2");
    }

    public void SetEntries(string[] entrieList){
        Clear();
        foreach(var entry in entrieList){
            Add(entry);
        }
    }

    public void Add(string label){
        var entry = Instantiate(prefab);
        var txt = entry.GetComponentInChildren<TMP_Text>();
        txt.text = label;
        entry.transform.SetParent(content, false);
        entries.Add(entry);
        entry.SetActive(true);
    }

    private void Clear(){
        foreach(var entry in entries){
            Destroy(entry);
        }

        entries.Clear();
    }
}
