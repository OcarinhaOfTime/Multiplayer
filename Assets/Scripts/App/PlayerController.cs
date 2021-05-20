using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour {
    public float speed = 10;
    public float mouse_sensibility = 10;
    public Vector2 move;
    public Vector2 look;
    ControlMap map;
    CharacterController controller;
    private Transform pivot;
    private Transform cam;
    private float vrot;
    private void Awake(){
        //cam = GetComponentInChildren<Camera>().transform;
        pivot = transform.GetChild(0);
        map = new ControlMap();
        map.Enable();
        controller = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        print("say my name");
    }

    private void Start(){
        if(!IsLocalPlayer){
            pivot.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Update(){
        if(IsLocalPlayer){
            Move();
            //Look();
        }
    }

    private void Move(){
        move = map.Player.Move.ReadValue<Vector2>();
       
        var v = pivot.right * move.x + pivot.forward * move.y;
        v *= speed;
        v.y = -10;
        controller.Move(v * Time.deltaTime);
    }

    private void Look(){
        var nlook = map.Player.Look.ReadValue<Vector2>();
        var delta = nlook - look;
        if(delta.sqrMagnitude > 10000){
            delta = Vector2.zero;
        }
        pivot.localRotation *= Quaternion.Euler(
            0, delta.x * Time.deltaTime * mouse_sensibility, 0);
        
        vrot += delta.y * Time.deltaTime * mouse_sensibility * Screen.height / (float)Screen.width;
        vrot = Mathf.Clamp(vrot, -45, 45);
        cam.localRotation = Quaternion.Euler(-vrot, 0, 0);
        look = nlook;
    }
}
