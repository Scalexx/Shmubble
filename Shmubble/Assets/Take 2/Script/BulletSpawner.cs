using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

    public List<Transform> spawnPoints = new List<Transform>();
    Vector3 direction;
    float timeBetweenShots;
    int projectileChoice;

    public List<GameObject> projectiles = new List<GameObject>();

	void Start () {
        if(spawnPoints.Count == 0)
        {
            spawnPoints.Add (gameObject.transform);
        }
	}
	
	void Update () {
		if (timeBetweenShots <= 0)
        {
            Instantiate(projectiles[projectileChoice], spawnPoints[0].position, spawnPoints[0].rotation);
            timeBetweenShots = projectiles[projectileChoice].GetComponent<BulletData>().timeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
	}
}
