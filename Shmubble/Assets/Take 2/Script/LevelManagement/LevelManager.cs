using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { set; get; }

    public float health = 3;
    private float healthTrigger1;
    private float healthTrigger2;
    public float bossHealth = 1200;

    public Transform spawnPosition;

    public Transform playerTransform;

    // Called before Start ()
    private void Awake ()
    {
        Instance = this;
        healthTrigger1 = health / 4;
        healthTrigger2 = 1;
    }

    void Update ()
    {
        
    }

    public void Win ()
    {

    }

    public void OutOfBounds()
    {
        // Out of bounds
        playerTransform.position = spawnPosition.position;
        playerTransform.GetComponent<Player>().Invulnerable();
        health--;
        if (health <= 0)
        {

        }
    }

    public void GetDamaged (float damage)
    {
        health -= damage;
    }

    public void DamageBoss (float damage)
    {
        if (health <= healthTrigger2)
        {
            damage = damage * 2;
        }
        else if (health <= healthTrigger1)
        {
            damage = damage * 1.25f;
        }
        bossHealth -= damage;
        print(damage);
    }
}

// no bugs plz