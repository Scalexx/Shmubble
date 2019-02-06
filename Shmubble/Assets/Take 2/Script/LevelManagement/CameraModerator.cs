using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModerator : MonoBehaviour {

    public Transform lookAt;

    public Vector3 offset;

	void Start () {
		
	}
	
	void LateUpdate () {
        transform.position = lookAt.transform.position + offset;
	}
}
