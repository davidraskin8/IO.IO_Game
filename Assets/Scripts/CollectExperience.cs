using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectExperience : MonoBehaviour {
    Spawner experienceSpawner;
    bool canSpawn = true;

    void Start() {
        experienceSpawner = GameObject.Find("Map").GetComponent<Spawner>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            ExperienceLevel levelScript = other.gameObject.GetComponent<ExperienceLevel>();

            levelScript.AddExp(); //Player adds to experience bar when collision with experience
            experienceSpawner.CmdSpawnRandom(); //Randomly spawn a new experience when one is destroyed
            
            Destroy(gameObject); //Destroy this experience
        }
    }

}
