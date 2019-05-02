using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIntro : MonoBehaviour {

    [Header("Basic parameters")]
    [Tooltip("The time it takes to play the whole intro.")]
    public float introTimerFull;
    [Tooltip("The time at which the boss spawns in with the intro particle.")]
    public float queueBossTimer;

    [Space(10)]
    [Tooltip("The position the camera needs to move to during the boss spawn.")]
    public Vector3 moveTo;

    [Header("Components")]
    [Tooltip("The smooth camera script on the Main Camera.")]
    public SmoothCamera smoothCamera;
    [Tooltip("The multiple targets script on the Main Camera.")]
    public MultipleTargets multipleTargets;
    [Tooltip("The disable movement script on the Player.")]
    public DisableMovement playerDisable;

    [Space(10)]
    [Tooltip("The animation to let the HUD fade in at the start of the fight.")]
    public Animation HUDAnim;
    [Tooltip("The gameobject which handles the intro particle.")]
    public GameObject introParticle;

    bool entered;
    float introPeriod;
    bool smooth;
    private Vector3 velocity;

	void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            entered = true;
            AudioManager.instance.PlayAmbientSound("BackgroundMusic");
        }
    }
    void Update()
    {
        if (entered)
        {
            playerDisable.entered = false;
            playerDisable.introTimer = introTimerFull;

            introPeriod = introTimerFull;
            smoothCamera.enabled = false;
            smooth = true;
            entered = false;
        }

        if (smooth)
        {
            smoothCamera.gameObject.transform.position = Vector3.SmoothDamp(smoothCamera.gameObject.transform.position, moveTo, ref velocity, 1f);
            smoothCamera.gameObject.transform.rotation = Quaternion.RotateTowards(smoothCamera.gameObject.transform.rotation, Quaternion.Euler(Vector3.zero), 0.1f);

            if (introPeriod <= queueBossTimer)
            {
                AudioManager.instance.PlayBossSound("BossIntro");
                introParticle.SetActive(true);
            }

            if (introPeriod <= 0)
            {
                HUDAnim.Play("HUDin");
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
