using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovement : MonoBehaviour {

    public float introTimer;
    private float introPeriod;
    Player playerScript;

    void Start()
    {
        playerScript = GetComponent<Player>();
        introPeriod = introTimer;
        playerScript.enabled = false;
    }
    void Update()
    {
        if (introPeriod <= 0)
        {
            playerScript.enabled = true;
        }
        else
        {
            introPeriod -= Time.deltaTime;
        }
    }
}

// no bugs plz
