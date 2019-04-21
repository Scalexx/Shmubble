using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovement : MonoBehaviour {

    public float introTimer;
    Player playerScript;

    void Start()
    {
        playerScript = GetComponent<Player>();
        playerScript.enabled = false;
    }
    void Update()
    {
        if (introTimer <= 0)
        {
            playerScript.enabled = true;
        }
        else
        {
            introTimer -= Time.deltaTime;
        }
    }

}
