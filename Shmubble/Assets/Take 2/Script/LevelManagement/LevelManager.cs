using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance { set; get; }
    
    [Header("Health")]
    [Tooltip("Health of the player.")]
    public float health = 3;
    private float healthTrigger1;
    private float healthTrigger2;
    [Tooltip("Health of the boss.")]
    public float bossHealth = 1200;

    [Header("Boss")]
    [Tooltip("Amount of HP to trigger environmental attacks.")]
    public float healthTriggerEnvironmental;
    [Tooltip("Amount of HP to trigger phase 3.")]
    public int healthTriggerPhaseFinal;

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
    float tValue;
    [Tooltip("How fast the bars change their value.")]
    public float speedBar;

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

    public Animation camAnim;

    // Called before Start ()
    private void Awake ()
    {
        Instance = this;

        healthTrigger1 = health / 4;
        healthTrigger2 = 1;

        Cursor.visible = false;

        bossTransform.GetComponent<Boss>().healthTriggerPhaseFinal = healthTriggerPhaseFinal;

        healthBar.GetComponent<Slider>().maxValue = health;
        exBar.GetComponent<Slider>().maxValue = specialMaxCharge;
        gameOverProgressBar.maxValue = bossHealth;
    }

    void Update ()
    {
        camAnim.Play();
        if (healthBar.GetComponent<Slider>().value != health)
        {
            tValue += speedBar * Time.unscaledDeltaTime;
            healthBar.GetComponent<Slider>().value = Mathf.SmoothStep(healthBar.GetComponent<Slider>().value, health, tValue);
        }
        if (exBar.GetComponent<Slider>().value != damageDealt)
        {
            tValue += speedBar * Time.unscaledDeltaTime;
            exBar.GetComponent<Slider>().value = Mathf.SmoothStep(exBar.GetComponent<Slider>().value, damageDealt, tValue);
        }
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

        if (damageDealt >= damage)
        {
            damageDealt -= damage;
        }
        else
        {
            damageDealt = 0;
        }

        if (health <= 0)
        {
            GameOver();
        }

        tValue = 0;
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

        if (bossHealth <= healthTriggerEnvironmental && !bossTransform.GetComponent<Boss>().queueEnvironmental)
        {
            bossTransform.GetComponent<Boss>().queueEnvironmental = true;
        }

        if (bossHealth <= 0)
        {
            bossTransform.GetComponent<Boss>().state = Boss.State.DEATH;
        }

        tValue = 0;
    }

    public void DamageDealt(float damage)
    {
        damageDealt += damage;
    }

    public void SpecialDone (float damage)
    {
        tValue = 0;
        if (health <= healthTrigger2)
        {
            damage = damage * 2;
        }
        else if (health <= healthTrigger1)
        {
            damage = damage * 1.25f;
        }
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