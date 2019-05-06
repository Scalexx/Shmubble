using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour {

    [Header("Target")]
    [Tooltip("Target to follow.")]
    public Transform lookAt;

    [Header("Movement options")]
    [Tooltip("Makes camera movement smooth.")]
    public bool smooth = true;
    [Tooltip("Makes camera look at target.")]
    public bool cameraPanning;
    [Tooltip("How fast the camera reaches its target position.")]
    [Range(0, 1)]
    public float smoothSpeed = 0.125f;

    [Space(10)]
    [Tooltip("How fast the camera rotates to its target rotation.")]
    public float smoothSpeedRotation = 3f;

    [Header("Movement restrictions")]
    [Tooltip("Gives camera bounds.")]
    public bool bounds;
    [Tooltip("Stops the camera panning effect when the camera reaches the bounds of the screen.")]
    public bool stopPanningWhenBound;
    [Tooltip("Minimum x, y, and z values for the camera. (Check for offset)")]
    public Vector3 minBounds;
    [Tooltip("Maximum x, y, and z values for the camera. (Check for offset)")]
    public Vector3 maxBounds;

    [Space(20)]
    [Tooltip("Offset position.")]
    public Vector3 offset;
    [Tooltip("Offset rotation")]
    public Vector3 offsetRotation;

    private Vector3 velocity;

    void Update()
    {
        Vector3 desiredPosition = Vector3.zero;
        if (lookAt != null)
        {
            desiredPosition = lookAt.transform.position + offset;
        }
        else
        {
            desiredPosition = offset;
        }

        if(smooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
        else
        {
            transform.position = desiredPosition;
        }
        if (cameraPanning)
        {
            Vector3 relativePos = lookAt.position - transform.position + offsetRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(relativePos), Time.deltaTime * smoothSpeedRotation);
        }
        if (bounds)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x), Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y), Mathf.Clamp(transform.position.z, minBounds.z, maxBounds.z));
            if (stopPanningWhenBound)
            {
                if (transform.position.x == minBounds.x || transform.position.x == maxBounds.x)
                {
                    cameraPanning = false;
                }
                else
                {
                    cameraPanning = true;
                }
            }
            else
            {
                cameraPanning = true;
            }
        }
    }
}
