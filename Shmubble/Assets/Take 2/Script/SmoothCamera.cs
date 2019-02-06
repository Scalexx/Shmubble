using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {

    public Transform lookAt;

    private bool smooth = true;
    public bool cameraPanning;
    [Range (0,1)]
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        Vector3 desiredPosition = lookAt.transform.position + offset;

        if(smooth)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
        else
        {
            transform.position = desiredPosition;
        }
        if (cameraPanning)
        {
            transform.LookAt(lookAt);
        }
    }
}
