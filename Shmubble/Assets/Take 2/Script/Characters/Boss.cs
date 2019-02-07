﻿using System.Collections;
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
    [Tooltip("The GameObject the bullet spawns from.")]
    public Transform spawnPoint;
    float timeBetweenShots;
    int projectileChoice;
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
                //projectileChoice = 0;
                //HandleShoot();

                // if attack is done
                state = State.IDLE;
                break;
            case State.ATTACK_2:
                // do attack stuff
                //projectileChoice = 1;
                //HandleShoot();

                // if attack is done
                state = State.IDLE;
                break;
            case State.ATTACK_3:
                // do attack stuff
                //projectileChoice = 2;
                //HandleShoot();

                // if attack is done
                state = State.IDLE;
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

                // if attack is done
                state = State.IDLE;
                break;
            case State.ATTACK_2:
                // do attack stuff

                // if attack is done
                state = State.IDLE;
                break;
            case State.ATTACK_3:
                // do attack stuff

                // if attack is done
                state = State.IDLE;
                break;
            case State.DEATH:
                // death animation
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

            timeBetweenShots = newProjectile.GetComponent<BulletData>().timeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}