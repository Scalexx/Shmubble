using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float moveSpeed = 0.2f ;
	int damage = 1 ;
	Vector3 positionBullet ;

	// Use this for initialization
	void Start () {
        positionBullet = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Move left (change to velocity maybe?)
		positionBullet.x -= moveSpeed ;
		transform.position = new Vector3 (positionBullet.x, positionBullet.y , 0) ;
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "OutOfBounds") {
			// Destroy when off screen
			Destroy (this.gameObject);
		}
	}
}
