using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParticle : MonoBehaviour {

    [Tooltip("The speed at which the object moves.")]
    public float speed;
    [Tooltip("The prefab of the impact effect.")]
    public GameObject impactPrefab;
    [Tooltip("All the trail particles which will need to stay once the gameobject is destroyed.")]
    public List<GameObject> trails;

    Boss boss;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision hit)
    {
        boss.enabled = true;

        speed = 0;

        ContactPoint contact = hit.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        if (impactPrefab != null)
        {
            var impactVFX = Instantiate(impactPrefab, pos, rot) as GameObject;
            Destroy(impactVFX, 5);
        }

        if(trails.Count > 0)
        {
            for(int i = 0; i < trails.Count; i++)
            {
                trails[i].transform.parent = null;
                var ps = trails[i].GetComponent<ParticleSystem>(); 
                if (ps != null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                } 
            }
        }

        Destroy(gameObject);
    }

}

// no bugs plz