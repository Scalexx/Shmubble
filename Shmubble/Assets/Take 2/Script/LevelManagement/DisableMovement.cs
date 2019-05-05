using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMovement : MonoBehaviour {

    [Tooltip("The amount of time that the player can't move.")]
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
            playerScript.disableJump = true;
            playerScript.disableDash = true;
            entered = true;
        }

        if (introPeriod <= 0)
        {
            playerScript.allowDisable = false;
            playerScript.disableShoot = false;
            playerScript.disableJump = false;
            playerScript.disableDash = false;
        }
        else
        {
            introPeriod -= Time.deltaTime;
        }
    }
}

// no bugs plz
