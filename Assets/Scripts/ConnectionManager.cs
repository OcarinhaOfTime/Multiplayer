using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;

public class ConnectionManager : MonoBehaviour {
    private void Setup(){
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, MLAPI.NetworkManager.ConnectionApprovedDelegate callback){
        bool approve = true;
        bool createPlayerObject = true;

        ulong? prefabHash = NetworkSpawnManager.GetPrefabHashFromGenerator("MyPrefab");
        callback(createPlayerObject, prefabHash, approve, Vector3.zero, Quaternion.identity);
    }
}
