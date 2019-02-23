using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLR : MonoBehaviour {

    LineRenderer line;

    public GameObject player;

    public float damage;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        line.enabled = false;

        player = GameObject.Find("PlayerCollider");
    }
	
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("FireLaser");
            StartCoroutine("FireLaser");
        }
	}

    IEnumerator FireLaser()
    {
        line.enabled = true;

        while (Input.GetButton("Fire1"))
        {
            Ray ray = new Ray(transform.position, transform.right);
            RaycastHit hit;

            line.SetPosition(0, ray.origin);
            line.SetPosition(1, ray.GetPoint(100));

            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.CompareTag("Player"))
                {
                    player.GetComponentInParent<Player>().GetDamaged(transform);
                }
            }

            yield return null;
        }

        line.enabled = false;
    }
}
