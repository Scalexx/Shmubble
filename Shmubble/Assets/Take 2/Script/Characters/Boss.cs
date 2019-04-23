using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Boss : MonoBehaviour {

    public enum State
    {
        INTRO,
        IDLE,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        BOUNCE,
        DEATH
    }

    public State state = State.INTRO;
    public State environmentalState = State.ATTACK_1;

    [HideInInspector]
    public int healthTriggerPhaseFinal;

    [Header("Intro")]
    [Tooltip("Intro duration for phase 1.")]
    public float introDuration1 = 1.0f;
    [Tooltip("Intro duration for phase 2.")]
    public float introDuration2 = 1.0f;

    public GameObject introParticle;

    private float time;
    private float timeEnv;
    private int attacksMax = 3;

    [Header("Idle")]
    [Tooltip("Minimum amount of time in the Idle state.")]
    public float idleMinTime;
    [Tooltip("Maximum amount of time in the Idle state.")]
    public float idleMaxTime;
    private float startIdleMinTime;
    
    [Space(10)]
    [Tooltip("Minimum amount of time in the Idle state for the final phase.")]
    public float idleMinTimeFinal;
    [Tooltip("Maximum amount of time in the Idle state for the final phase.")]
    public float idleMaxTimeFinal;
    private float startIdleMinTimeFinal;

    [Space(10)]
    [Tooltip("Minimum amount of time in the Idle state of the environment attacks in the final phase.")]
    public float idleMinTimeEnv;
    [Tooltip("Maximum amount of time in the Idle state of the environment attacks in the final phase.")]
    public float idleMaxTimeEnv;
    private float startIdleMinTimeEnv;
    private bool entered;

    [Header("Attack Effects")]
    public GameObject attackEffect1;
    public GameObject attackEffect2;
    public GameObject attackEffect3;
    public Transform spawnPointEffect;
    GameObject attackEffectActive;

    [Header("Anticipation")]
    [Tooltip("The effect that plays before shooting during the first attack.")]
    public GameObject anticipationEffectAttack1;
    [Tooltip("The effect that plays before shooting during the second attack.")]
    public GameObject anticipationEffectAttack2;
    [Tooltip("The effect that plays before shooting during the third attack.")]
    public GameObject anticipationEffectAttack3;
    [Tooltip("The effect that plays before shooting during the fourth attack (final phase).")]
    public GameObject anticipationEffectAttack4;
    GameObject anticipationEffectActive;

    [Space(10)]
    [Tooltip("Amount of time the boss waits before it shoots during the first phases.")]
    public float anticipationTimer;
    [Tooltip("Amount of time the boss waits before it shoots during the final phase.")]
    public float anticipationTimerFinal;
    float anticipationPeriod;

    [Space(10)]
    [Tooltip("Gameobject which spawns to indicate the first environmental attack.")]
    public GameObject anticipationEnv1;
    bool anticipationDone;
    public Vector3 effectOffset;
    [Tooltip("The intensity of the shake.")]
    public float cameraShakeMagnitude;
    [Tooltip("How rough the shake is. Lower values are slow and smooth, higher values are fast and jarring.")]
    public float cameraShakeRoughness;
    [Tooltip("The time for the shake to fade in.")]
    public float cameraShakeFadeIn;
    [Tooltip("The time for the shake to fade out. Higher values make the shake last longer.")]
    public float cameraShakeFadeOut;

    [Space(10)]
    [Tooltip("Amount of time the first environmental attack waits before it spawns the objects.")]
    public float anticipationTimerEnv1;
    [Tooltip("Amount of time the second environmental attack waits before it spawns the objects.")]
    public float anticipationTimerEnv2;
    float anticipationEnvPeriod;

    [Header("Attacks")]
    [Tooltip("Amount of shots to fire with the first attack of the projectilePool.")]
    public int shotsToFireAttack1;
    [Tooltip("Amount of shots to fire with the second attack of the projectilePool.")]
    public int shotsToFireAttack2;
    [Tooltip("Amount of shots to fire with the third attack of the projectilePool.")]
    public int shotsToFireAttack3;
    int shotsFired;
    int attackInt;
    private int lastAttackState = -1;
    bool attackStateEntered;
    private bool attackEntered;

    [Space(10)]
    [Tooltip("The GameObjects the bullets can spawn from during the first attack.")]
    public List<Transform> spawnPointsAttack1 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the second attack.")]
    public List<Transform> spawnPointsAttack2 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the third attack.")]
    public List<Transform> spawnPointsAttack3 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the fourth attack (final phase projectile attack).")]
    public List<Transform> spawnPointsAttack4 = new List<Transform>();
    int spawnPointInt;
    Transform spawnPoint;

    [Space(10)]
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the first attack.")]
    public bool randomSpawnAttack1;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the second attack.")]
    public bool randomSpawnAttack2;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the third attack.")]
    public bool randomSpawnAttack3;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the fourth attack (final phase projectile attack).")]
    public bool randomSpawnAttack4;

    [Space(10)]
    [Tooltip("The GameObjects with the Object Pool on it.")]
    public ObjectPooler[] projectilePools;

    [Header("Environmental")]
    [Tooltip("Checks if environmental attacks can be spawned (Phase 2).")]
    public bool queueEnvironmental;
    [Tooltip("How much the boss should wait between starting the environmental attack and the normal attack.")]
    public float waitBeforeAttackTimer;
    private float waitBeforeAttackPeriod;
    private bool environmentalEntered;
    private int lastEnvState = -1;

    [Space(10)]
    [Tooltip("Minimum amount of shots to fire with the first environmental attack of the final phase.")]
    public int shotsToFireEnv1Min;
    [Tooltip("Maximum amount of shots to fire with the first environmental attack of the final phase.")]
    public int shotsToFireEnv1Max;
    [Tooltip("Minimum amount of shots to fire with the second environmental attack of the final phase.")]
    public int shotsToFireEnv2Min;
    [Tooltip("Maximum amount of shots to fire with the first environmental attack of the final phase.")]
    public int shotsToFireEnv2Max;
    [Tooltip("Minumum amount of shots to fire with the third environmental attack of the final phase.")]
    public int shotsToFireEnv3Min;
    [Tooltip("Maximum amount of shots to fire with the first environmental attack of the final phase.")]
    public int shotsToFireEnv3Max;
    private int shotsFiredEnv;

    [Space(10)]
    [Tooltip("The GameObjects the bullets can spawn from during the first environmental attack.")]
    public List<Transform> spawnPointsEnv1 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the second environmental attack.")]
    public List<Transform> spawnPointsEnv2 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the third environmental attack.")]
    public List<Transform> spawnPointsEnv3 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the fourth environmental attack.")]
    public List<Transform> spawnPointsEnv4 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the fifth environmental attack.")]
    public List<Transform> spawnPointsEnv5 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the sixth environmental attack.")]
    public List<Transform> spawnPointsEnv6 = new List<Transform>();
    int spawnPointIntEnv;
    Transform spawnPointEnv;

    [Space(10)]
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the first environmental attack.")]
    public bool randomSpawnEnv1;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the second environmental attack.")]
    public bool randomSpawnEnv2;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the third environmental attack.")]
    public bool randomSpawnEnv3;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the fourth environmental attack.")]
    public bool randomSpawnEnv4;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the fifth environmental attack.")]
    public bool randomSpawnEnv5;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the sixth environmental attack.")]
    public bool randomSpawnEnv6;

    [Space(10)]
    [Tooltip("The Gameobjects with the Object Pool on it.")]
    public List<ObjectPooler> environmentalPools = new List<ObjectPooler>();
    
    [Space(10)]
    [Tooltip("Amount of time between bounce attacks.")]
    public float bounceTimer;
    private float bouncePeriod;
    [Tooltip("The GameObject with the Object Pool on it.")]
    public ObjectPooler bouncePool;
    [Tooltip("The GameObject the bounce object spawns from.")]
    public Transform spawnpointBounce;

    [Tooltip("Checks if the bounce attack has passed the trigger for the boss to respawn.")]
    public bool bounceTrigger;
    [Tooltip("Timer of the animation that plays before the boss disappears.")]
    public float bounceAnimTimer;
    float bounceAnimPeriod;
    [Tooltip("Timer of the animation that plays when the boss returns.")]
    public float bounceAnimTimerReturn;
    float bounceAnimPeriodReturn;

    [Header("Extras")]
    [Tooltip("Animation of the canvas before the final phase.")]
    public Animation canvasAnim;

    [Space(10)]
    [Tooltip("The gameobject of the boss which shows during the first phases.")]
    public GameObject bossPhase1;
    [Tooltip("The gameobject of the boss which shows during the final phase.")]
    public GameObject bossPhaseFinal;
    [Tooltip("The animator attached to the boss during the first phases.")]
    public Animator animatorPhase1;
    [Tooltip("The animator attached to the boss during the final phase.")]
    public Animator animatorPhaseFinal;

    Animator animator;
    float timeBetweenShots;
    float timeBetweenShotsEnv;
    int projectileChoice;
    int environmentalChoice;
    float waitTime;
    bool finalPhaseTrigger;
    bool phase2Entered;
    bool envIdleEntered;

    private int rand;

    void Start () {
        animator = animatorPhase1;
        time = introDuration1;
        bouncePeriod = bounceTimer;
        bounceAnimPeriodReturn = bounceAnimTimerReturn;
        bounceAnimPeriod = bounceAnimTimer;
        startIdleMinTime = idleMinTime;
        startIdleMinTimeFinal = idleMinTimeFinal;
        startIdleMinTimeEnv = idleMinTimeEnv;
	}
	
	void Update () {
        if (!finalPhaseTrigger)
        {
            Phase1();
        }
        else
        {
            if (!phase2Entered)
            {
                bossPhase1.SetActive(false);
                bossPhaseFinal.SetActive(true);
                animator = animatorPhaseFinal;

                canvasAnim.Play("Canvas");
                phase2Entered = true;
            }
            
            Phase2();
        }

        if (LevelManager.Instance.bossHealth <= 0)
        {
            state = State.DEATH;
        }

        if (queueEnvironmental)
        {
            if (bouncePeriod <= 0)
            {
                attacksMax = 4;
            }
            else
            {
                bouncePeriod -= Time.deltaTime;
                attacksMax = 3;
            }
        }
    }

    void Phase1()
    {
        switch (state)
        {
            case State.INTRO:
                // do intro stuff
                HandleIntro();

                animator.SetBool("Idle", true);

                if (time <= 0)
                {
                    
                    state = State.IDLE;
                    time = introDuration2;
                }
                else
                {
                    time -= Time.deltaTime;
                }
                break;

            case State.IDLE:
                if (LevelManager.Instance.bossHealth <= healthTriggerPhaseFinal)
                {
                    finalPhaseTrigger = true;
                    break;
                }
                
                if (bounceTrigger)
                {
                    if (bounceAnimPeriodReturn <= 0)
                    {
                        bounceTrigger = false;
                        bounceAnimPeriodReturn = bounceAnimTimerReturn;
                    }
                    else
                    {
                        bounceAnimPeriodReturn -= Time.deltaTime;
                    }
                } 
                else
                {
                    // do idle stuff

                    // play idle animation
                    animator.SetBool("Idle", true);

                    if (entered == false)
                    {
                        animator.ResetTrigger("Intro");
                        idleMinTime = startIdleMinTime + waitTime;
                        time = Random.Range(idleMinTime, idleMaxTime);
                        entered = true;
                    }
                    else
                    {
                        shotsFired = 0;
                        waitTime = 0;

                        if (environmentalState == State.IDLE)
                        {
                            if (time <= 0)
                            {
                                rand = generateRandomNumber(0, attacksMax, lastAttackState);
                                HandleAttackState(rand);
                            }
                            else
                            {
                                time -= Time.deltaTime;
                            }
                        }
                    }
                }
                          
                break;

            case State.ATTACK_1:
                // do attack stuff
                lastAttackState = 0;

                if (!attackStateEntered)
                {
                    if (queueEnvironmental)
                    {
                        if (!environmentalEntered)
                        {
                            HandleEnvironmentalState();
                            environmentalEntered = true;
                        }
                    }
                    attackStateEntered = true;
                } 

                projectileChoice = 0;
                HandleAttack(shotsToFireAttack1, randomSpawnAttack1, spawnPointsAttack1, anticipationEffectAttack1, anticipationTimer, attackEffect1);
                
                break;

            case State.ATTACK_2:
                // do attack stuff
                lastAttackState = 1;

                if (!attackStateEntered)
                {
                    if (queueEnvironmental)
                    {
                        if (!environmentalEntered)
                        {
                            HandleEnvironmentalState();
                            environmentalEntered = true;
                        }
                    }
                    attackStateEntered = true;
                }

                projectileChoice = 1;
                HandleAttack(shotsToFireAttack2, randomSpawnAttack2, spawnPointsAttack2, anticipationEffectAttack2, anticipationTimer, attackEffect2);
                
                break;

            case State.ATTACK_3:
                // do attack stuff
                lastAttackState = 2;

                if (!attackStateEntered)
                {
                    if (queueEnvironmental)
                    {
                        if (!environmentalEntered)
                        {
                            HandleEnvironmentalState();
                            environmentalEntered = true;
                        }
                    }
                    attackStateEntered = true;
                }

                projectileChoice = 2;
                HandleAttack(shotsToFireAttack3, randomSpawnAttack3, spawnPointsAttack3, anticipationEffectAttack3, anticipationTimer, attackEffect3);
                
                break;

            case State.BOUNCE:
                // play bounce animation
                lastAttackState = 3;

                animator.SetBool("Bounce", true);
                if (bounceAnimPeriod <= 0)
                {
                    bossPhase1.SetActive(false);
                }
                else
                {
                    bounceAnimPeriod -= Time.deltaTime;
                }

                // do bounce stuff
                if (shotsFired < 1)
                {
                    spawnPoint = spawnpointBounce;
                    HandleBounce();
                }

                if (bounceTrigger)
                {
                    bossPhase1.SetActive(true);
                    // play return animation
                    animator.SetTrigger("BounceTrigger");
                    bouncePeriod = bounceTimer;
                    bounceAnimPeriod = bounceAnimTimer;
                    
                    state = State.IDLE;
                    animator.SetBool("Idle", true);
                }
                break;
        }

        switch (environmentalState) {
            case State.IDLE:
                environmentalEntered = false;
                
                if (state == State.IDLE)
                {
                    if (queueEnvironmental && !envIdleEntered)
                    {
                        waitBeforeAttackPeriod = waitBeforeAttackTimer;
                        envIdleEntered = true;
                    }
                }
                
                break;

            case State.ATTACK_1:
                lastEnvState = 0;

                // play environmental animation
                environmentalChoice = 0;
                HandleEnvironmentalAttack(randomSpawnEnv1, spawnPointsEnv1, anticipationTimerEnv1, anticipationEnv1, attackInt);

                break;

            case State.ATTACK_2:
                lastEnvState = 1;

                // play environmental animation
                environmentalChoice = 1;
                HandleEnvironmentalAttack(randomSpawnEnv2, spawnPointsEnv2, 0f, null, attackInt);

                break;

            case State.ATTACK_3:
                lastEnvState = 2;

                // play environmental animation
                environmentalChoice = 2;
                HandleEnvironmentalAttack(randomSpawnEnv3, spawnPointsEnv3, 0f, null, attackInt);

                break;   
            }
    }

    void Phase2()
    {
        switch (state)
        {
            case State.INTRO:
                // do intro stuff


                if (time <= 0)
                {
                    state = State.IDLE;
                }
                else
                {
                    time -= Time.deltaTime;
                }
                break;

            case State.IDLE:
                // do idle stuff
                // play idle animation
                if (entered == false)
                {
                    idleMinTimeFinal = startIdleMinTimeFinal + waitTime;
                    time = Random.Range(idleMinTimeFinal, idleMaxTimeFinal);
                    entered = true;
                }
                else
                {
                    shotsFired = 0;
                    waitTime = 0;

                    if (time <= 0)
                    {
                        // choose attack to do
                        int rand = Random.Range(0, attacksMax);
                        if (rand == 0)
                        {
                            state = State.ATTACK_1;
                            time = Random.Range(3, 5);
                            entered = false;
                        }
                        else if (rand == 1)
                        {
                            state = State.ATTACK_2;
                            time = Random.Range(3, 5);
                            entered = false;
                        }
                        else if (rand == 2)
                        {
                            if (shotsToFireAttack3 > 1)
                            {
                                spawnPointInt = 0;
                            }
                            state = State.ATTACK_3;
                            entered = false;
                        }
                    }
                    else
                    {
                        time -= Time.deltaTime;
                    }
                }
                break;

            case State.ATTACK_1:
                // do animation stuff
                // AnimationClip.length
                
                if (time <= 0)
                {
                    state = State.IDLE;
                }
                else
                {
                    time -= Time.deltaTime;
                }

                break;

            case State.ATTACK_2:
                // do animation stuff
                // AnimationClip.length

                if (time <= 0)
                {
                    state = State.IDLE;
                }
                else
                {
                    time -= Time.deltaTime;
                }

                break;

            case State.ATTACK_3:
                // do attack stuff
                projectileChoice = 3;

                if (shotsFired < 1)
                {
                    if (randomSpawnAttack4)
                    {
                        spawnPointInt = Random.Range(0, spawnPointsAttack4.Count);
                    }

                    if (spawnPointInt < spawnPointsAttack4.Count)
                    {
                        spawnPoint = spawnPointsAttack4[spawnPointInt];
                        HandleShoot();
                    }
                    else
                    {
                        spawnPointInt = 0;
                    }
                }
                else
                {
                    state = State.IDLE;
                }

                break;

            case State.DEATH:
                // death animation
                LevelManager.Instance.Win();
                break;

        }

        switch (environmentalState)
        {
            case State.IDLE:
                if (environmentalEntered == false)
                {
                    shotsFiredEnv = 0;
                    idleMinTimeEnv = startIdleMinTimeEnv;
                    timeEnv = Random.Range(idleMinTimeEnv, idleMaxTimeEnv);
                    environmentalEntered = true;
                }
                else
                {
                    if (timeEnv <= 0)
                    {
                        // choose environment attack to do
                        int envRand = Random.Range(0, 3);
                        if (envRand == 0)
                        {
                            environmentalState = State.ATTACK_1;
                            environmentalEntered = false;
                        }
                        else if (envRand == 1)
                        {
                            environmentalState = State.ATTACK_2;
                            environmentalEntered = false;
                        }
                        else if (envRand == 2)
                        {
                            environmentalState = State.ATTACK_3;
                            environmentalEntered = false;
                        }
                    }
                    else
                    {
                        timeEnv -= Time.deltaTime;
                    }
                }
                break;

            case State.ATTACK_1:
                environmentalChoice = 3;
                int rand1 = Random.Range(shotsToFireEnv1Min, shotsToFireEnv1Max);
                if (shotsFiredEnv < rand1)
                {
                    if (randomSpawnEnv4)
                    {
                        spawnPointIntEnv = Random.Range(0, spawnPointsEnv4.Count);
                    }

                    if (spawnPointIntEnv < spawnPointsEnv4.Count)
                    {
                        spawnPointEnv = spawnPointsEnv4[spawnPointIntEnv];
                        HandleEnvironmental(0f, null);
                    }
                    else
                    {
                        spawnPointIntEnv = 0;
                    }
                }
                else
                {
                    environmentalState = State.IDLE;
                }
                
                break;

            case State.ATTACK_2:
                environmentalChoice = 4;
                int rand2 = Random.Range(shotsToFireEnv2Min, shotsToFireEnv2Max);
                if (shotsFiredEnv < rand2)
                {
                    if (randomSpawnEnv5)
                    {
                        spawnPointIntEnv = Random.Range(0, spawnPointsEnv5.Count);
                    }

                    if (spawnPointIntEnv < spawnPointsEnv5.Count)
                    {
                        spawnPointEnv = spawnPointsEnv5[spawnPointIntEnv];
                        HandleEnvironmental(0f, null);
                    }
                    else
                    {
                        spawnPointIntEnv = 0;
                    }
                }
                else
                {
                    environmentalState = State.IDLE;
                }
                
                break;

            case State.ATTACK_3:
                environmentalChoice = 5;
                int rand3 = Random.Range(shotsToFireEnv3Min, shotsToFireEnv3Max);
                if (shotsFiredEnv < rand3)
                {
                    if (randomSpawnEnv6)
                    {
                        spawnPointIntEnv = Random.Range(0, spawnPointsEnv6.Count);
                    }

                    if (spawnPointIntEnv < spawnPointsEnv6.Count)
                    {
                        spawnPointEnv = spawnPointsEnv6[spawnPointIntEnv];
                        HandleEnvironmental(0f, null);
                    }
                    else
                    {
                        spawnPointIntEnv = 0;
                    }
                }
                else
                {
                    environmentalState = State.IDLE;
                }

                break;
        }
    }

    void OnTriggerEnter (Collider hit)
    {
        if (hit.gameObject.layer == 8)
        {
            LevelManager.Instance.DamageBoss(hit.gameObject.GetComponent<BulletData>().damage);
        }
    }

    void HandleIntro()
    {
        bossPhase1.SetActive(true);
        animator.SetTrigger("Intro");
    }

    private int generateRandomNumber(int min, int max, int last)
    {
        int result = Random.Range(min, max);

        if (result == last)
        {

            return generateRandomNumber(min, max, last);

        }

        last = result;
        return result;
    }

    public void HandleAttackState(int rand)
    {
        // choose attack to do
        if (rand == 0)
        {
            if (shotsToFireAttack1 > 1)
            {
                spawnPointInt = 0;
            }
            attackInt = shotsToFireAttack1;
            state = State.ATTACK_1;
            entered = false;
            animator.SetBool("Idle", false);
        }
        else if (rand == 1)
        {
            if (shotsToFireAttack2 > 1)
            {
                spawnPointInt = 0;
            }
            attackInt = shotsToFireAttack2;
            state = State.ATTACK_2;
            entered = false;
            animator.SetBool("Idle", false);
        }
        else if (rand == 2)
        {
            if (shotsToFireAttack3 > 1)
            {
                spawnPointInt = 0;
            }
            attackInt = shotsToFireAttack3;
            state = State.ATTACK_3;
            entered = false;
            animator.SetBool("Idle", false);
        }
        else if (rand == 3)
        {
            state = State.BOUNCE;
            entered = false;
            animator.SetBool("Idle", false);
        }
    }

    public void HandleEnvironmentalState()
    {
        envIdleEntered = false;
        animator.SetTrigger("EnvironmentalAttack");
        int rand = generateRandomNumber(0, 3, lastEnvState);
        if (rand == 0)
        {
            environmentalState = State.ATTACK_1;
        }
        else if (rand == 1)
        {
            environmentalState = State.ATTACK_2;
        }
        else if (rand == 2)
        {
            CameraShaker.Instance.ShakeOnce(cameraShakeMagnitude, cameraShakeRoughness, cameraShakeFadeIn, cameraShakeFadeOut);
            environmentalState = State.ATTACK_3;
        }
        
    }

    public void HandleAttack(int shotsToFire, bool randomSpawn, List<Transform> spawnPoints, GameObject anticipationEffect, float anticipationTimerEffect, GameObject attackEffect)
    {
        if (waitBeforeAttackPeriod <= 0)
        {
            if (!attackEntered)
            {
                anticipationPeriod = anticipationTimerEffect;
                animator.SetTrigger("Attack");
                anticipationEffectActive = Instantiate(anticipationEffect, gameObject.transform.position, anticipationEffect.transform.rotation);
                attackEffectActive = Instantiate(attackEffect, spawnPointEffect.position, attackEffect.transform.rotation);

                animator.SetBool("Idle", true);
                attackEntered = true;
            }

            if (anticipationPeriod <= 0)
            {
                if (shotsFired < shotsToFire)
                {
                    if (randomSpawn)
                    {
                        spawnPointInt = Random.Range(0, spawnPoints.Count);
                    }

                    if (spawnPointInt < spawnPoints.Count)
                    {
                        spawnPoint = spawnPoints[spawnPointInt];
                        HandleShoot();
                    }
                    else
                    {
                        spawnPointInt = 0;
                    }
                }
                else
                {
                    Destroy(anticipationEffectActive);
                    anticipationEffectActive = null;

                    Destroy(attackEffectActive);
                    attackEffectActive = null;

                    attackEntered = false;
                    attackStateEntered = false;
                    state = State.IDLE;
                }
            }
            else
            {
                anticipationPeriod -= Time.deltaTime;
            }
        }
        else
        {
            waitBeforeAttackPeriod -= Time.deltaTime;
        }
    }

    public void HandleEnvironmentalAttack(bool randomSpawn, List<Transform> spawnPoints, float environmentalTimer, GameObject environmentalEffect, int shotsToFireAttack)
    {
        if (shotsFired < shotsToFireAttack - 1)
        {
            if (randomSpawn)
            {
                spawnPointIntEnv = Random.Range(0, spawnPoints.Count);
            }

            if (spawnPointIntEnv < spawnPoints.Count)
            {
                spawnPointEnv = spawnPoints[spawnPointIntEnv];
                HandleEnvironmental(environmentalTimer, environmentalEffect);
            }
            else
            {
                spawnPointIntEnv = 0;
            }
        }
        else
        {
            anticipationDone = true;
        }
    }

    public void HandleShoot()
    {
        if (timeBetweenShots <= 0)
        {
            GameObject newProjectile = projectilePools[projectileChoice].GetPooledObject();

            newProjectile.transform.position = spawnPoint.position;
            newProjectile.transform.rotation = spawnPoint.rotation;
            newProjectile.SetActive(true);

            BulletData temp = newProjectile.GetComponent<BulletData>();
            if (temp != null)
            {
                timeBetweenShots = newProjectile.GetComponent<BulletData>().timeBetweenShots;
                waitTime = newProjectile.GetComponent<BulletData>().stopTimer;
            }
            else
            {
                timeBetweenShots = 1;
            }
            shotsFired++;
            spawnPointInt++;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    public void HandleEnvironmental(float anticipationTimerEnv, GameObject anticipationEnv)
    {
        if (timeBetweenShotsEnv <= 0)
        {
            if (!anticipationDone)
            {
                if (anticipationEnv != null)
                {
                    Instantiate(anticipationEnv, spawnPointEnv.position + effectOffset, anticipationEnv.transform.rotation);
                }
                anticipationEnvPeriod = anticipationTimerEnv;
                anticipationDone = true;
            }
            if (anticipationEnvPeriod <= 0)
            {
                GameObject newProjectile = environmentalPools[environmentalChoice].GetPooledObject();

                newProjectile.transform.position = spawnPointEnv.position;
                newProjectile.transform.rotation = spawnPointEnv.rotation;
                newProjectile.SetActive(true);

                BulletData temp = newProjectile.GetComponent<BulletData>();
                if (temp != null)
                {
                    timeBetweenShotsEnv = newProjectile.GetComponent<BulletData>().timeBetweenShots;
                }
                else
                {
                    Laser laser = newProjectile.GetComponent<Laser>();
                    if (laser != null)
                    {
                        timeBetweenShotsEnv = laser.timeBetweenShots;
                    }
                    else
                    {
                        timeBetweenShotsEnv = 1;
                    }
                }
                spawnPointIntEnv++;
                shotsFiredEnv++;
                if (state == State.IDLE)
                {
                    environmentalState = State.IDLE;
                }
                anticipationDone = false;
            }
            else
            {
                anticipationEnvPeriod -= Time.deltaTime;
            }
        }
        else
        {
            timeBetweenShotsEnv -= Time.deltaTime;
        }
    }

    public void HandleBounce()
    {
        GameObject newProjectile = bouncePool.GetPooledObject();

        newProjectile.transform.position = spawnPoint.position;
        newProjectile.transform.rotation = spawnPoint.rotation;
        newProjectile.SetActive(true);

        shotsFired++;
    }
}

// no bugs plz