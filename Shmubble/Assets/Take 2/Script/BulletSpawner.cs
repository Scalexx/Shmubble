using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

    public Transform spawnPoint;
    float timeBetweenShots;
    int projectileChoice;

    public List<GameObject> projectiles = new List<GameObject>();

	void Start () {
        if(spawnPoint == null)
        {
            spawnPoint = gameObject.transform;
        }
        projectileChoice = 0;
	}
	
	void Update () {
		if (timeBetweenShots <= 0)
        {
            Instantiate(projectiles[projectileChoice], spawnPoint.position, spawnPoint.rotation);
            timeBetweenShots = projectiles[projectileChoice].GetComponent<BulletData>().timeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
	}
}
