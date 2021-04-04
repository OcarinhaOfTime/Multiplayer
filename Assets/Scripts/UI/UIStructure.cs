using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStructure : MonoBehaviour {
    
    public int currentIndex = 0;
    private SimpleWidget[] widgets;

    private void Start(){
        widgets = GetComponentsInChildren<SimpleWidget>();
    }
    

    public void Navigate(int i){
        
    }
}
