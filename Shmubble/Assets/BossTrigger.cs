using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    public SmoothCamera smoothCamera;
    public Vector3 moveTo;
    bool smooth;
    private Vector3 velocity;

	void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            smoothCamera.enabled = false;
            smooth = true;
        }
    }
    void Update()
    {
        if (smooth)
        {
            smoothCamera.gameObject.transform.position = Vector3.SmoothDamp(smoothCamera.gameObject.transform.position, moveTo, ref velocity, 1f);
            smoothCamera.gameObject.transform.rotation = Quaternion.RotateTowards(smoothCamera.gameObject.transform.rotation, Quaternion.Euler(Vector3.zero), 0.1f);
        }   

        if (smoothCamera.gameObject.transform.position == moveTo)
            smooth = false;
    }
}
