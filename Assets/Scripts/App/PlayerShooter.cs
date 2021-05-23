using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PlayerShooter : NetworkBehaviour {

    private void Start(){

    }    

    private void Update(){
        if(IsLocalPlayer && Input.GetMouseButton(0)){
            Shoot();
        }
    }

    private void Shoot(){
        
    }
}
