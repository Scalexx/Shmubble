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

    public float outOfBoundsDamage;

    public Slider healthBar;
    public Slider exBar;

    [Tooltip("Game Over screen object.")]
    public GameObject gameOver;
    public Slider gameOverProgressBar;

    public GameObject winScreen;

    public Transform spawnPosition;

    public Transform playerTransform;
    public Transform bossTransform;

    // Called before Start ()
    private void Awake ()
    {
        Instance = this;
        healthTrigger1 = health / 4;

        Cursor.visible = false;
        healthTrigger2 = 1;

        healthBar.GetComponent<Slider>().maxValue = health;
        gameOverProgressBar.maxValue = bossHealth;
    }

    void Update ()
    {
        
    }

    public void Win ()
    {
        Destroy(bossTransform.gameObject);
        Time.timeScale = 0f;
        Cursor.visible = true;

        winScreen.SetActive(true);
    }

    public void OutOfBounds()
    {
        // Out of bounds
        playerTransform.position = spawnPosition.position;
        playerTransform.GetComponent<Player>().Invulnerable();
        GetDamaged(outOfBoundsDamage);
    }

    public void GetDamaged (float damage)
    {
        health -= damage;

        healthBar.GetComponent<Slider>().value = health;

        if (damageDealt >= damage)
        {
            damageDealt -= damage;
        }
        else
        {
            damageDealt = 0;
        }

        exBar.GetComponent<Slider>().value = damageDealt;

        if (health <= 0)
        {
            GameOver();
        }
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

        if (bossHealth <= 0)
        {
            Win();
        }
    }

    public void DamageDealt(float damage)
    {
        damageDealt += damage;
        exBar.GetComponent<Slider>().value = damageDealt;
    }

    public void SpecialDone (float damage)
    {
        damageDealt = 0 - damage ;
    }

    public void GameOver()
    {
        Destroy(playerTransform.gameObject);
        Cursor.visible = true;
        Time.timeScale = 0f;

        gameOver.SetActive(true);
    }
}

// no bugs plz