using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDenial : MonoBehaviour {

    [Header("Destructable")]
    [Tooltip("Check if the object is destructable.")]
    public bool destructable;
    [Tooltip("Health of the object.")]
    public float startHealth;
    float health;

    [Header("Platform")]
    [Tooltip("Makes the object a platform which pushes and you can jump on and off from. Make sure to freeze position in Rigidbody. Trigger collider on object attaches the player to the object.")]
    public bool solidObject;

    [Header("Shooting")]
    [Tooltip("Check if the object will spawn new projectiles.")]
    public bool projectileShooting;
    [Tooltip("Spawnpoint of the new projectile.")]
    public Transform spawnPoint;
    [Tooltip("Object with the Object pool on it.")]
    public ObjectPooler projectilePool;
    float timeBetweenShots;

	void OnEnable () {
		if (destructable)
        {
            health = startHealth;
        }
	}
	
	void Update () {
        if (projectileShooting)
        {
            HandleShoot();
        }
	}

    void OnTriggerEnter(Collider hit)
    {
        if (destructable)
        {
            if (hit.gameObject.layer == 8)
            {
                health -= hit.gameObject.GetComponent<BulletData>().damage;
                LevelManager.Instance.DamageDealt(hit.gameObject.GetComponent<BulletData>().damage);
                if (health <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        if (hit.gameObject.CompareTag("Player"))
        {
            hit.transform.SetParent(transform);
        }
    }

    void OnTriggerExit (Collider hit)
    {

        if (hit.gameObject.CompareTag("Player"))
        {
            hit.transform.SetParent(null);
        }
    }

    void HandleShoot()
    {
        if (timeBetweenShots <= 0)
        {
            GameObject newProjectile = projectilePool.GetPooledObject();

            newProjectile.transform.position = spawnPoint.position;
            newProjectile.transform.rotation = spawnPoint.rotation;
            newProjectile.SetActive(true);

            timeBetweenShots = newProjectile.GetComponent<BulletData>().timeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}

// no bugs plz