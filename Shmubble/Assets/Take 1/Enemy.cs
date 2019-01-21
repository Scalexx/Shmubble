using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public GameObject bullet ;
    public float startSpawnTimer;
    private float spawnTimer;

	public float amplitude = 0.5f;
	public float frequency = 1f;

	// Position Storage Variables
	Vector3 posOffset = new Vector3 ();
	Vector3 tempPos = new Vector3 ();


	// Use this for initialization
	void Start () {
		// Store the starting position & rotation of the object
		posOffset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Move up, down
		tempPos = posOffset;
		tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;

		transform.position = tempPos;

        // Spawn bullet over time
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0 )
        {
            Instantiate(bullet, tempPos, Quaternion.identity);
            spawnTimer = startSpawnTimer;
        }
        
	}
}
