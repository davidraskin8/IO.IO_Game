using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour {

    public int bulletDamage;

    //Destroy the bullet when it collides with another gameObject
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHit = collision.gameObject;
            var playerHealth = playerHit.GetComponent<Health>();

            playerHealth.TakeDamage(bulletDamage); //Decrease player health on collision
        }
        Destroy(gameObject);
    }
}
