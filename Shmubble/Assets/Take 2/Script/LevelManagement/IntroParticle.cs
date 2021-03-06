﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroParticle : MonoBehaviour {

    [Tooltip("The vfx prefab.")]
    public GameObject vfx;
    [Tooltip("The gameobject the vfx prefab spawns from.")]
    public Transform startPoint;
    [Tooltip("The gameobject the vfx prefab goes to.")]
    public Transform endPoint;

    void OnEnable()
    {
        var startPos = startPoint.position;
        GameObject vfxObj = Instantiate(vfx, startPos, Quaternion.identity) as GameObject;

        var endPos = endPoint.position;

        RotateTo(vfxObj, endPos);
    }

    void RotateTo(GameObject obj, Vector3 destination)
    {
        var direction = destination - obj.transform.position;
        var rotation = Quaternion.LookRotation(direction);
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.rotation, rotation, 1);
    }

}

// no bugs plz
