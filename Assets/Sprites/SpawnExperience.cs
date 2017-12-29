using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class Spawner : NetworkBehaviour {
    public GameObject experiencePrefab;
    public GameObject spawn1;
    public GameObject spawn2;

    public int amount = 100;

    private float mapWidth;
    private float mapHeight;

    public override void OnStartServer() {
        
        mapWidth = transform.localScale.x;
        mapHeight = transform.localScale.y;

        spawn1.transform.position = new Vector3(getRandomPos(mapWidth), getRandomPos(mapHeight), 0);
        spawn2.transform.position = new Vector3(getRandomPos(mapWidth), getRandomPos(mapHeight), 0);
        SpawnAll();
    }

    public override void OnStartLocalPlayer()
    {
        spawn1.transform.position = new Vector3(getRandomPos(mapWidth), getRandomPos(mapHeight), 0);
        spawn2.transform.position = new Vector3(getRandomPos(mapWidth), getRandomPos(mapHeight), 0);
    }

    //Function only run on the server spawns the experience in random locations across the map
    [Server]
    private void SpawnAll() {
        for (int i = 0; i < amount; i++) {
            GameObject experience = (GameObject)Instantiate(experiencePrefab, new Vector3(getRandomPos(mapWidth), getRandomPos(mapHeight), 0), transform.rotation);
            NetworkServer.Spawn(experience);
        }
    }

    //Command run on server from client to spawn experience when a collision is detected
    [Command]
    public void CmdSpawnRandom()
    {
        GameObject experience = (GameObject)Instantiate(experiencePrefab, new Vector3(getRandomPos(mapWidth), getRandomPos(mapHeight), 0), transform.rotation);
        NetworkServer.Spawn(experience);
    }

    private float getRandomPos(float length ) {
        return Random.Range(-1 * length / 2, length / 2);
    }

       
}
