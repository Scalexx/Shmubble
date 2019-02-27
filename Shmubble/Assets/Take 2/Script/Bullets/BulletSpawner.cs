using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour {

    public List<Transform> spawnPoints = new List<Transform>();
    float timeBetweenShots;
    int projectileChoice;

    public ObjectPooler projectilePool;

	void Start () {
        if(spawnPoints.Count == 0)
        {
            spawnPoints.Add (gameObject.transform);
        }
	}
	
	void Update () {
		if (timeBetweenShots <= 0)
        {
            GameObject newProjectile = projectilePool.GetPooledObject();

            newProjectile.transform.position = spawnPoints[projectileChoice].position;
            newProjectile.transform.rotation = spawnPoints[projectileChoice].rotation;
            newProjectile.SetActive(true);

            timeBetweenShots = newProjectile.GetComponent<BulletData>().timeBetweenShots;
            projectileChoice = Random.Range(0, spawnPoints.Count);
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
	}
}
