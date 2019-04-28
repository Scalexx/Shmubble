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

    }

    void Update()
    {
        if (generationPoint.transform.position.x < transform.position.x)
        {
            generationPoint.transform.position = new Vector3(generationPoint.transform.position.x + trackWidth, generationPoint.transform.position.y, generationPoint.transform.position.z);

            generationPoint.transform.parent = null;

            GameObject newProjectile = projectilePool.GetPooledObject();

            newProjectile.transform.position = generationPoint.transform.position;
            newProjectile.transform.rotation = generationPoint.transform.rotation;

            generationPoint.transform.parent = newProjectile.transform;

            newProjectile.SetActive(true);
        }
    }
}

// no bugs plz
