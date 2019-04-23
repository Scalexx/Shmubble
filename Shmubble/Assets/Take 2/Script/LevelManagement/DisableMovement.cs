using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovement : MonoBehaviour {

    public float introTimer;
    private float introPeriod;
    Player playerScript;

    [HideInInspector]
    public bool entered;

    void Update()
    {
        if (!entered)
        {
            playerScript = GetComponent<Player>();
            introPeriod = introTimer;
            playerScript.allowDisable = true;
            playerScript.disableShoot = true;
            entered = true;
        }

        if (introPeriod <= 0)
        {
            playerScript.allowDisable = false;
            playerScript.disableShoot = false;
        }
        else
        {
            introPeriod -= Time.deltaTime;
        }
    }
}

// no bugs plz
