using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public float damage;

    public float beamRotationSpeed = 400.0f;
    public float beamExtendSpeed = 10.0f;

    public float timeBetweenShots;

    public float laserTime;
    float laserPeriod;

    public Vector3 size;
    Vector3 currentSize;

    public GameObject effects;
    GameObject effectActive;

    void OnEnable()
    {
        laserPeriod = laserTime;
        transform.localScale = new Vector3(0, size.y, size.z);

        effectActive = Instantiate(effects, gameObject.transform.parent.transform.position, gameObject.transform.parent.transform.rotation);
    }

    void Update()
    {
        transform.Rotate((Time.deltaTime * beamRotationSpeed), 0, 0);
        laserPeriod -= Time.deltaTime;

        if (laserPeriod >= 0)
        {
            currentSize = size;
            transform.localScale = Vector3.Lerp(transform.localScale, currentSize, (beamExtendSpeed * Time.deltaTime));
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(currentSize.x / 2, 0, 0), (beamExtendSpeed * Time.deltaTime));
        }
        else
        {
            Vector3 moveOut = new Vector3(size.x * 2, 0, 0);
            transform.Translate(moveOut * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("OutOfBoundsFar"))
        {
            DestroyMe();
        }
    }

    void DestroyMe()
    {
        float timer;
        var ps = effectActive.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
            timer = ps.main.duration + ps.main.startLifetime.constantMax;
            Destroy(ps.gameObject, timer);
        }
        else
        {
            Transform par = effectActive.transform;
            int childCount = par.childCount - 1;
            ParticleSystem psChild;

            for (int i = childCount; i > -1; i--)
            {
                Transform childX = par.GetChild(i);

                childX.transform.parent = null;
                psChild = childX.GetComponent<ParticleSystem>();
                if (psChild != null)
                {
                    psChild.Stop();
                    Destroy(psChild.gameObject, psChild.main.duration + psChild.main.startLifetime.constantMax);
                }
                else
                {
                    Destroy(childX.gameObject);
                }
            }
        }

        Destroy(effectActive);
        effectActive = null;

        gameObject.SetActive(false);
        gameObject.transform.parent.transform.gameObject.SetActive(false);
    }
}

// no bugs plz
