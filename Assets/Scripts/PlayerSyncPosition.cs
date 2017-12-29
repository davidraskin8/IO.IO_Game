using System.Collections;
using UnityEngine;
using UnityEngine.Networking; 

public class PlayerSyncPosition : NetworkBehaviour {

    [SyncVar]
    private Vector3 syncPos;

  
    public Transform myTransform;
    public float lerpRate = 2;
	
	// Update is called once per frame
	void FixedUpdate () {
        TransmitPosition();
        LerpPosition();
	}

    void LerpPosition() {
        if (!isLocalPlayer)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }
    
    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
    }

    [ClientCallback]
    void TransmitPosition() {
        if (isLocalPlayer) {
            CmdProvidePositionToServer(myTransform.position);
        }
    }
}
