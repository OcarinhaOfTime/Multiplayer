using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class HelloWorldPlayer : NetworkBehaviour {
    public NetworkVariable<Vector3> position = new NetworkVariable<Vector3>(
        new NetworkVariableSettings{
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

    public override void NetworkStart() {
        Move();
    }

    public void Move(){
        if(NetworkManager.Singleton.IsServer){
            var randPos = GetRandomPosOnPlane();
            transform.position = randPos;
            position.Value = randPos;
        }else{
            SubmitPositionReqServerRpc();
        }
    }

    static Vector3 GetRandomPosOnPlane() {
        return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
    }

    [ServerRpc]
    private void SubmitPositionReqServerRpc(){
        position.Value = GetRandomPosOnPlane();
    }

    private void Update(){
        transform.position = position.Value;
    }
}
