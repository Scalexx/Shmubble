using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainTracks : MonoBehaviour {

    public float speed;
    public Vector3 direction;

	void FixedUpdate () {
        transform.Translate(direction * speed * Time.deltaTime);
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
