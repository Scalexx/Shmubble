using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksSpawner : MonoBehaviour
{

    public float trackWidth;

    public GameObject generationPoint;

    public ObjectPooler projectilePool;

    void Start()
    {
        if (projectilePool.pooledObject.GetComponent<TrainTracks>().direction.y != 0)
        {
            trackWidth = projectilePool.pooledObject.GetComponent<BoxCollider>().size.y * projectilePool.pooledObject.transform.localScale.y;
        }
        else if (projectilePool.pooledObject.GetComponent<TrainTracks>().direction.z != 0)
        {
            trackWidth = projectilePool.pooledObject.GetComponent<BoxCollider>().size.z * projectilePool.pooledObject.transform.localScale.z;
        }
        else if (projectilePool.pooledObject.GetComponent<TrainTracks>().direction.x != 0)
        {
            trackWidth = projectilePool.pooledObject.GetComponent<BoxCollider>().size.x * projectilePool.pooledObject.transform.localScale.x;
        }
    }

    void Update()
    {
        if (generationPoint.transform.position.x < transform.position.x)
        {
            generationPoint.transform.position = new Vector3(generationPoint.transform.position.x + trackWidth, generationPoint.transform.position.y, generationPoint.transform.position.z);

            generationPoint.transform.parent = null;

            GameObject newProjectile = projectilePool.GetPooledObject();

            newProjectile.transform.position = generationPoint.transform.position;

            generationPoint.transform.parent = newProjectile.transform;

            newProjectile.SetActive(true);
        }
    }
}

// no bugs plz
