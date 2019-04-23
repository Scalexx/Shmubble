using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargets : MonoBehaviour {

    [Tooltip("Targets for the camera.")]
    public List<Transform> targets;

    [Space(10)]
    [Tooltip("Offset for the camera.")]
    public Vector3 offset;
    [Tooltip("Amount of time it will take to reach the target position. Lower numbers means it will move faster.")]
    public float smoothTime = .5f;

    [Space(10)]
    [Tooltip("The furthest the camera will be. This value will change the Field of View of the camera.")]
    public float minZoom;
    [Tooltip("The closest the camera will be. This value will change the Field of View of the camera.")]
    public float maxZoom;
    [Tooltip("The greatest distance there can be between the targets (the size of the playing field).")]
    public float zoomLimiter;

    private Camera cam;
    private Vector3 velocity;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    void Zoom ()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = GetBounds();

        return bounds.center;
    }

    float GetGreatestDistance()
    {
        var bounds = GetBounds();

        return bounds.size.x;
    }

    Bounds GetBounds()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds;
    }
}

// no bugs plz
