using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePlayer : MonoBehaviour {
    // Use this for initialization
    public GameObject animatedPanel;
    List<GameObject> guns = new List<GameObject>();
    PlayerController controller;
    Health playerHealth;
    UpgradeData data;

    void Start() {
        controller = GetComponent<PlayerController>();
        playerHealth = GetComponent<Health>();
    }
    
    public void Upgrade(GameObject upgrade)
    {
        animatedPanel.GetComponent<UpdateMenuScript>().DeAnimateMenu();
        Debug.Log("Clicked");

        //set the upgrade to the same position as the player, set the scale of the player to the same size as the upgrade
        upgrade.transform.position = transform.position;
        transform.localScale = upgrade.transform.localScale;

        Transform upgradeTransform = upgrade.transform; //Get the transform for the upgrade object
        data = upgrade.GetComponent<UpgradeData>(); //Get the data for the upgrade object

        foreach (Transform childTransform in upgradeTransform) {
            guns.Add(childTransform.gameObject); //List for the guns of the upgrade
        }

        SetGuns();
        SetData();
    }

    private void SetGuns() {
        //Clear the previous bulletwpawns
        controller.bulletSpawns.Clear();

        foreach (Transform gun in transform) {
            if (gun.gameObject.GetComponent<Canvas>() == null)
            {
                Destroy(gun.gameObject); //Destroy the previous guns
            }
        }

        foreach (GameObject gun in guns) {
            GameObject inGameGun = Instantiate(gun, transform); //Instantiate the new gun from the gun list
            controller.bulletSpawns.Add(inGameGun.transform.GetChild(0)); //Add the new bullet spawns to the gun list
        }
    }

    //Set the data from the player data object
    private void SetData() {
        controller.fireRate = data.fireRate;
        controller.bulletPrefab = data.bulletPrefab;
        playerHealth.maxHealth = data.health;
        playerHealth.currentHealth = data.health;
    }

     
}
