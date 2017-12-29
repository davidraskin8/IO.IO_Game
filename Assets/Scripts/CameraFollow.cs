using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform playerTransform;
    public int depth = -10;

    // Update is called once per frame
    void Update()
    {
        if (playerTransform != null)
        {
            //follow the players position
            transform.position = playerTransform.position + new Vector3(0, 0, depth);
            //look at the player
            transform.LookAt(playerTransform);
        }
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
