
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

//Went through Unity Networking tutorial applying code to 2D game objects
public class PlayerController : NetworkBehaviour {

    //Change depending on class
    public GameObject bulletPrefab;
    public List<Transform> bulletSpawns;

    public GameObject experienceCanvas;
    public float fireRate;

    private bool firing;

    void Start ()
    {
        //Set the main camera's position, and have it point towards the local player
        if(!isLocalPlayer)
        {
            experienceCanvas.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update ()
    {

        //Only update the positions for the player being currently controlled
        if (!isLocalPlayer)
        {
            return;
        }

#if UNITY_STANDALONE || UNITY_WEBPLAYER
        //For now the player moves vertically when the 'w' and 's' keys are pressed rotates clockwise or counter clockwise when the 'a' and 'd' keys are pressed
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, 0, x);
        transform.Translate(0, y, 0);

        //Player fires when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!firing)
            {
                StartCoroutine(Fire());
            }
        }

#else
        //Mobile movement control

        //get the values for the movement joystick
        var xm = CrossPlatformInputManager.GetAxis("Horizontal1") * Time.deltaTime * 3.0f;
        var ym = CrossPlatformInputManager.GetAxis("Vertical1") * Time.deltaTime * 3.0f;

        transform.Translate(new Vector2(-xm, ym));

        //get the values for the shooting joystick
        var xr = CrossPlatformInputManager.GetAxis("Horizontal2");
        var yr = CrossPlatformInputManager.GetAxis("Vertical2");

        float rotateAngle = Mathf.Rad2Deg * Mathf.Atan2(yr, xr);

        // (0, 0, 0) when pointed upward
        // (0, 180, 0) when pointed downward
        if (xr != 0 || yr != 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 90-rotateAngle);
            //fire in the direction of right joystick
            if (firing == false)
            {
                StartCoroutine(Fire());
            }
        }
   
#endif
   
	}

    //run the command on the server (only done on the clients player)
    [Command]
    void CmdFire()
    {
        foreach (Transform bulletSpawn in bulletSpawns) {
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);
        

            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

            //bullet must be spawned for all clients to get its position and velocity
            NetworkServer.Spawn(bullet);

            //destroy the bullet after two seconds
            //StartCoroutine runs a function across multiple frames
            StartCoroutine(Destroy(bullet, 2.0f));
        }

    }

    
    public override void OnStartLocalPlayer()
    {
        //colors the client's player blue while other players are white
        GetComponent<SpriteRenderer>().color = Color.blue;

        //Sets the main camera for the local player
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
    }

    //run over multiple frames
    //after specified time destroys the game object go
    public IEnumerator Destroy(GameObject go, float time) {
        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(go);
    }

    public IEnumerator Fire()
    {
        CmdFire();
        firing = true;
        yield return new WaitForSeconds(1/fireRate); //Return from the frame and continue to wait. Wait time decreases with firerate increase
        firing = false;
    }



}
