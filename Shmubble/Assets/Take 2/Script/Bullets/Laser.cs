using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public float beamRotationSpeed = 400.0f;
    public float beamExtendSpeed = 10.0f;

    public float laserTime;
    float laserPeriod;

    public Vector3 size;
    Vector3 currentSize;

    void OnEnable()
    {
        laserPeriod = laserTime;
        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(0, size.y, size.z);
    }

    void Update()
    {
        transform.Rotate((Time.deltaTime * beamRotationSpeed), 0, 0);
        laserPeriod -= Time.deltaTime;

        if (laserPeriod >= 0)
        {
            currentSize = size;
            transform.localScale = Vector3.Lerp(transform.localScale, currentSize, (beamExtendSpeed * Time.deltaTime));
            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(currentSize.x / 2, currentSize.y, currentSize.z), (beamExtendSpeed * Time.deltaTime));
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
            gameObject.SetActive(false);
        }
    }
}

// no bugs plz
