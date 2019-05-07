using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    public Boss boss;
    public DisableMovement playerDisable;

    public float introTimerFull;

    void Awake()
    {
        introTimerFull = boss.lengthAnimationIntro;
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            boss.triggeredNewBoss = true;
        }
    }

    void Update()
    {
        if (boss.triggeredNewBoss)
        {
            playerDisable.entered = false;
            playerDisable.introTimer = introTimerFull;
        }
    }

}

// no bugs plz
