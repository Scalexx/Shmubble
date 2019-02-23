using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { set; get; }

    public float health = 3;
    private float healthTrigger1;
    private float healthTrigger2;
    public float bossHealth = 1200;

    [Tooltip("Charge it needs to do the EX attack.")]
    public float specialMaxCharge;
    public float damageDealt;

    public Slider healthBar;
    public Slider exBar;

    public Transform spawnPosition;

    public Transform playerTransform;

    // Called before Start ()
    private void Awake ()
    {
        Instance = this;
        healthTrigger1 = health / 4;

        Cursor.visible = false;
        healthTrigger2 = 1;

        healthBar.GetComponent<Slider>().maxValue = health;
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

        healthBar.GetComponent<Slider>().value = health;

        damageDealt -= damage;

        exBar.GetComponent<Slider>().value = damageDealt;
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

        if (damageDealt < specialMaxCharge)
        {
            DamageDealt(damage);
        }
    }

    public void DamageDealt(float damage)
    {
        damageDealt += damage;
        exBar.GetComponent<Slider>().value = damageDealt;
    }

    public void SpecialDone ()
    {
        damageDealt = 0;
    }
}

// no bugs plz