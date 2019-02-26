using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public enum State
    {
        INTRO,
        IDLE,
        ATTACK_1,
        ATTACK_2,
        ATTACK_3,
        DEATH
    }

    public State state = State.INTRO;

    [Header("Phases")]
    [Tooltip ("Amount of HP to trigger phase 2.")]
    public int healthTriggerPhase2;

    [Header("Intro")]
    [Tooltip("Intro duration for phase 1.")]
    public float introDuration1 = 1.0f;
    [Tooltip("Intro duration for phase 2.")]
    public float introDuration2 = 1.0f;
    private float time;

    [Header("Idle")]
    [Tooltip("Minimum amount of time in the Idle state.")]
    public float idleMinTime;
    [Tooltip("Maximum amount of time in the Idle state.")]
    public float idleMaxTime;
    private bool entered;

    [Header("Attacks")]
    [Tooltip("Amount of shots to fire with the first attack of the projectilePool.")]
    public int shotsToFireAttack1;
    [Tooltip("Amount of shots to fire with the second attack of the projectilePool.")]
    public int shotsToFireAttack2;
    [Tooltip("Amount of shots to fire with the third attack of the projectilePool.")]
    public int shotsToFireAttack3;
    [Tooltip("Amount of shots to fire with the fourth attack of the projectilePool.")]
    public int shotsToFireAttack4;
    [Tooltip("Amount of shots to fire with the fifth attack of the projectilePool.")]
    public int shotsToFireAttack5;
    [Tooltip("Amount of shots to fire with the sixth attack of the projectilePool.")]
    public int shotsToFireAttack6;
    int shotsFired;

    [Space(10)]
    [Tooltip("The GameObjects the bullets can spawn from during the first attack.")]
    public List<Transform> spawnPointsAttack1 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the second attack.")]
    public List<Transform> spawnPointsAttack2 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the third attack.")]
    public List<Transform> spawnPointsAttack3 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the fourth attack.")]
    public List<Transform> spawnPointsAttack4 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the fifth attack.")]
    public List<Transform> spawnPointsAttack5 = new List<Transform>();
    [Tooltip("The GameObjects the bullets can spawn from during the sixth attack.")]
    public List<Transform> spawnPointsAttack6 = new List<Transform>();
    int spawnPointInt;
    Transform spawnPoint;

    [Space(10)]
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the first attack.")]
    public bool randomSpawnAttack1;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the second attack.")]
    public bool randomSpawnAttack2;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the third attack.")]
    public bool randomSpawnAttack3;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the fourth attack.")]
    public bool randomSpawnAttack4;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the fifth attack.")]
    public bool randomSpawnAttack5;
    [Tooltip("Chooses whether the bullets spawn from random points in the list during the sixth attack.")]
    public bool randomSpawnAttack6;

    float timeBetweenShots;
    int projectileChoice;
    
    [Space(10)]
    [Tooltip("The GameObject with the Object Pool on it.")]
    public ObjectPooler[] projectilePools;

    private int rand;

    void Start () {
        time = introDuration1;
	}
	
	void Update () {
        if (LevelManager.Instance.bossHealth > healthTriggerPhase2)
        {
            Phase1();
            
        }
        else if (LevelManager.Instance.bossHealth <= healthTriggerPhase2)
        {
            Phase2();
        }
        if (LevelManager.Instance.bossHealth <= 0)
        {
            state = State.DEATH;
        }
    }

    void Phase1()
    {
        switch (state)
        {
            case State.INTRO:
                // do intro stuff
                

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
                // do idle stuff
                // play idle animation
                if (entered == false)
                {
                    time = Random.Range(idleMinTime, idleMaxTime);
                    entered = true;
                }
                else
                {
                    shotsFired = 0;
                    
                    if (time <= 0)
                    {
                        // choose attack to do
                        int rand = Random.Range(0, 3);
                        if (rand == 0)
                        {
                            if (shotsToFireAttack1 > 1)
                            {
                                spawnPointInt = 0;
                            }
                            state = State.ATTACK_1;
                            entered = false;
                        }
                        else if (rand == 1)
                        {
                            if (shotsToFireAttack2 > 1)
                            {
                                spawnPointInt = 0;
                            }
                            state = State.ATTACK_2;
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
                // do attack stuff
                projectileChoice = 0;
                if (shotsFired < shotsToFireAttack1)
                {
                    if (randomSpawnAttack1)
                    {
                        spawnPointInt = Random.Range(0, spawnPointsAttack1.Count);
                    }

                    if (spawnPointInt < spawnPointsAttack1.Count)
                    {
                        spawnPoint = spawnPointsAttack1[spawnPointInt];
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
            case State.ATTACK_2:
                // do attack stuff
                projectileChoice = 1;
                if (shotsFired < shotsToFireAttack2)
                {
                    if (randomSpawnAttack2)
                    {
                        spawnPointInt = Random.Range(0, spawnPointsAttack2.Count);
                    }

                    if (spawnPointInt < spawnPointsAttack2.Count)
                    {
                        spawnPoint = spawnPointsAttack2[spawnPointInt];
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
            case State.ATTACK_3:
                // do attack stuff
                projectileChoice = 2;
                if (shotsFired < shotsToFireAttack3)
                {
                    if (randomSpawnAttack3)
                    {
                        spawnPointInt = Random.Range(0, spawnPointsAttack3.Count);
                    }
                    
                    if (spawnPointInt < spawnPointsAttack3.Count)
                    {
                        spawnPoint = spawnPointsAttack3[spawnPointInt];
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
                    time = Random.Range(idleMinTime, idleMaxTime);
                    entered = true;
                }
                else
                {
                    shotsFired = 0;

                    if (time <= 0)
                    {
                        // choose attack to do
                        int rand = Random.Range(0, 3);
                        if (rand == 0)
                        {
                            state = State.ATTACK_1;
                            entered = false;
                        }
                        else if (rand == 1)
                        {
                            state = State.ATTACK_2;
                            entered = false;
                        }
                        else if (rand == 2)
                        {
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
                // do attack stuff
                projectileChoice = 3;
                if (shotsFired < shotsToFireAttack4)
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
            case State.ATTACK_2:
                // do attack stuff
                projectileChoice = 4;
                if (shotsFired < shotsToFireAttack5)
                {
                    if (randomSpawnAttack5)
                    {
                        spawnPointInt = Random.Range(0, spawnPointsAttack5.Count);
                    }

                    if (spawnPointInt < spawnPointsAttack5.Count)
                    {
                        spawnPoint = spawnPointsAttack5[spawnPointInt];
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
            case State.ATTACK_3:
                // do attack stuff
                projectileChoice = 5;
                if (shotsFired < shotsToFireAttack4)
                {
                    if (randomSpawnAttack6)
                    {
                        spawnPointInt = Random.Range(0, spawnPointsAttack6.Count);
                    }

                    if (spawnPointInt < spawnPointsAttack6.Count)
                    {
                        spawnPoint = spawnPointsAttack6[spawnPointInt];
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
    }

    void OnTriggerEnter (Collider hit)
    {
        if (hit.gameObject.layer == 8)
        {
            LevelManager.Instance.DamageBoss(hit.gameObject.GetComponent<BulletData>().damage);
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
}

// no bugs plz