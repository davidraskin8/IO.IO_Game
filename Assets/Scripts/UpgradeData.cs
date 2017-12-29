using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object to store data for the different types of upgrades 
public class UpgradeData : MonoBehaviour {
    public float fireRate;
    public GameObject bulletPrefab;
    public float health;

    public UpgradeData(float _fireRate, GameObject _bulletPrefab, float _health)
    {
        fireRate = _fireRate;
        bulletPrefab = _bulletPrefab;
        health = _health;
    }
}
