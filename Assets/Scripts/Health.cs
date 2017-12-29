using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Unity Networking Help: https://unity3d.com/learn/tutorials/topics/multiplayer-networking/networking-player-health?playlist=29690
public class Health : NetworkBehaviour {

    public float maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public float currentHealth;

    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.value = CalculateHealth(currentHealth);
    }

    //called by bullet prefab when player is hit
    public void TakeDamage(int amount)
    {
        if(!isServer)
        {
            return;
        }

        currentHealth -= amount;
        
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead");
        }

    }

    //Change healthbar GUI when a player takes damage
    void OnChangeHealth(float currentHealth)
    {
        healthBar.value = CalculateHealth(currentHealth);
    }

    float CalculateHealth(float currentHealth)
    {
        return currentHealth / maxHealth;
    }
}
