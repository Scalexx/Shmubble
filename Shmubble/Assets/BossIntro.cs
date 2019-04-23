using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour {

    public SmoothCamera smoothCamera;
    public MultipleTargets multipleTargets;
    public Vector3 moveTo;
    public DisableMovement playerDisable;
    public GameObject introParticle;
    public float introTimerFull;
    public float queueBossTimer;
    public Animation HUDAnim;

    float introPeriod;
    bool smooth;
    private Vector3 velocity;

	void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            playerDisable.entered = false;
            playerDisable.introTimer = introTimerFull;
            playerDisable.enabled = true;

            introPeriod = introTimerFull;
            smoothCamera.enabled = false;
            smooth = true;
        }
    }
    void Update()
    {
        if (smooth)
        {
            smoothCamera.gameObject.transform.position = Vector3.SmoothDamp(smoothCamera.gameObject.transform.position, moveTo, ref velocity, 1f);
            smoothCamera.gameObject.transform.rotation = Quaternion.RotateTowards(smoothCamera.gameObject.transform.rotation, Quaternion.Euler(Vector3.zero), 0.1f);

            if (introPeriod <= queueBossTimer)
            {
                introParticle.SetActive(true);
            }

            if (introPeriod <= 0)
            {
                HUDAnim.Play("HUDin");
                playerDisable.enabled = false;
                multipleTargets.enabled = true;
                smooth = false;
                Destroy(gameObject);
            }
            else
            {
                introPeriod -= Time.deltaTime;
            }
        }     
    }
}

// no bugs plz
