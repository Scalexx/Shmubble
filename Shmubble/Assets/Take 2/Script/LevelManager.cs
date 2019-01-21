using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { set; get; }

    public int health = 3;
    public int bossHealth = 1200;

    public Transform spawnPosition;

    public Transform playerTransform;

    // Called before Start ()
    private void Awake ()
    {
        Instance = this;
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
        health--;
        if (health <= 0)
        {

        }
    }

    public void GetDamaged ()
    {
        health--;
    }
}
