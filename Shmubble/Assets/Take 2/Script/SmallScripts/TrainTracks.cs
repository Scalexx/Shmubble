using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks : MonoBehaviour {

    public float speed;

	void Update () {
        transform.Translate(-speed * Time.deltaTime, 0, 0);
	}

    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.CompareTag("OutOfBoundsFar"))
        {
            gameObject.SetActive(false);
        }
    }
}

// no bugs plz
