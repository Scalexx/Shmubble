using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovement : MonoBehaviour {

    public float introTimer;
    private float introPeriod;
    Player playerScript;

    [HideInInspector]
    public bool entered;

    void Start()
    {
        
    }
    void Update()
    {
        if (!entered)
        {
            playerScript = GetComponent<Player>();
            introPeriod = introTimer;
            playerScript.enabled = false;
            entered = true;
        }

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
