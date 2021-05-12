using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryList : MonoBehaviour {
    public RectTransform content;
    public GameObject prefab;
    private List<GameObject> entries = new List<GameObject>();

    public void SetEntries(string[] entries, Sprite[] sprites){
        Clear();
        for(int i=0; i<entries.Length; i++){
            Add(entries[i], sprites[i]);
        }
    }

    public void Add(string label, Sprite sprite){
        var entry = Instantiate(prefab);
        var txt = entry.GetComponentInChildren<TMP_Text>();
        txt.text = label;
        var im = entry.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        im.sprite = sprite;
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
