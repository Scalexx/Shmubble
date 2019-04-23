using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointTrigger : MonoBehaviour {

    public int spawnPointNumber;

	void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("Player"))
        {
            LevelManager.Instance.spawnPointNumber = spawnPointNumber;
        }
    }
}

// no bugs plz
