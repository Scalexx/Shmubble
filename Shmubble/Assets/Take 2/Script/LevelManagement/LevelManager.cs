using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { set; get; }
    
    [Header("Health")]
    [Tooltip("Health of the player.")]
    public float health = 3;
    private float healthTrigger1;
    private float healthTrigger2;
    [Tooltip("Health of the boss.")]
    public float bossHealth = 1200;

    [Header("Special move")]
    [Tooltip("Charge it needs to do the EX attack.")]
    public float specialMaxCharge;
    [Tooltip("Damage dealt by player.")]
    public float damageDealt;

    [Header("Out of bounds")]
    [Tooltip("Amount of damage done to the player when going out of bounds.")]
    public float outOfBoundsDamage;

    [Header("UI")]
    [Tooltip("Healthbar gameobject.")]
    public Slider healthBar;
    [Tooltip("EXbar gameobject.")]
    public Slider exBar;

    [Space(10)]
    [Tooltip("Game Over screen object.")]
    public GameObject gameOver;
    [Tooltip("Progressbar gameobject.")]
    public Slider gameOverProgressBar;

    [Space(10)]
    [Tooltip("Win screen panel.")]
    public GameObject winScreen;

    [Space(10)]
    [Tooltip("Pause menu script.")]
    public PauseMenu pauseMenu;

    [Header("Spawn")]
    [Tooltip("The spawn location of the player at the start and when going out of bounds.")]
    public Transform spawnPosition;

    [Header("Scene subjects")]
    [Tooltip("The player gameobject.")]
    public Transform playerTransform;
    [Tooltip("The boss gameobject.")]
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
        pauseMenu.someoneDied = true;

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
        pauseMenu.someoneDied = true;

        gameOver.SetActive(true);
    }
}

// no bugs plz