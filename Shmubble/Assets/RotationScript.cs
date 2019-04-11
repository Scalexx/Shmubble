using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    public float randX;
    public float randY;
    public float randZ;

    void OnEnable()
    {
        this.transform.localRotation = Quaternion.Euler (Random.Range(0, randX), Random.Range(0, randY), Random.Range(0, randZ));
    }

}
