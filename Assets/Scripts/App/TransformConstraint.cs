using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformConstraint : MonoBehaviour {
    public Transform target;
    private Quaternion originalRot;
    private Vector3 originalPos;
    public bool position;
    public bool rotation;
    private Action updateAction;

    private void Awake(){
        originalRot = transform.localRotation;
        originalPos = transform.localPosition;

        if(position){
            updateAction += () => transform.localPosition = originalPos + target.localPosition;
        }

        if(rotation){
            updateAction += () => transform.localRotation = originalRot * target.localRotation;
        }
    }

    private void Update(){
        updateAction();
    }

}
