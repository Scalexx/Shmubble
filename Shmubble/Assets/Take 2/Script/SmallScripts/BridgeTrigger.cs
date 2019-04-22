using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTrigger : MonoBehaviour {

    public Boss boss;

	void OnTriggerExit (Collider hit)
    {
        if (hit.CompareTag("Bounce"))
        {
            boss.bounceTrigger = true;
        }
    }
}

// no bugs plz
