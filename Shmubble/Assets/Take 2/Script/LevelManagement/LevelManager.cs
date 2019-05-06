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
    [HideInInspector]
    public float healthTrigger1;
    [HideInInspector]
    public float healthTrigger2;
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
    float outOfBoundsDam;

    [Header("UI")]
    [Tooltip("Healthbar gameobject.")]
    public Slider healthBar;
    [Tooltip("Healthbar fill area object.")]
    public Image healthBarFillArea;


    [Space(10)]
    [Tooltip("Color of the healthbar when it's full.")]
    public Color healthBarFillColor1;
    [Tooltip("Color of the healthbar when it's at 25%.")]
    public Color healthBarFillColor2;
    [Tooltip("Color of the healthbar when at 1 HP.")]
    public Color healthBarFillColor3;

    [Space(10)]
    [Tooltip("EXbar gameobject.")]
    public Slider exBar;
    [HideInInspector]
    public float tValue;
    [HideInInspector]
    public float perc;
    [Tooltip("How fast the bars change their value.")]
    public float speedBar;
    [Tooltip("Amount of time for the bars to reach its new value.")]
    public float totalTimeBar;

    public Color bossFlashColor1;
    public Color bossFlashColor2;
    public Color bossFlashColor3;

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
    [Tooltip("The spawn locations of the player when going out of bounds.")]
    public List<Transform> spawnPositions;

    [Header("Scene subjects")]
    [Tooltip("The player gameobject.")]
    public Transform playerTransform;
    [Tooltip("The boss gameobject.")]
    public Transform bossTransform;

    [Header("Extras")]
    [Tooltip("The animation of the camera's constant shaking.")]
    public Animation camAnim;

    [HideInInspector]
    public int spawnPointNumber;

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

    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayAmbientSound("TrainSound");
            AudioManager.instance.PlayAmbientSound("WindSound");
        }
    }

    void Update ()
    {
        camAnim.Play();
        var healthBarValue = healthBar.GetComponent<Slider>().value;
        if (healthBarValue != health)
        {
            tValue += speedBar * Time.unscaledDeltaTime;

            if (tValue > totalTimeBar)
            {
                tValue = totalTimeBar;   
            }

            perc = tValue / totalTimeBar;

            healthBar.GetComponent<Slider>().value = Mathf.SmoothStep(healthBarValue, health, perc);

            if (health > healthTrigger1)
            {
                healthBarFillArea.color = Color.Lerp(healthBarFillArea.color, healthBarFillColor1, perc);
            }
            else if (health <= healthTrigger1 && health > healthTrigger2)
            {
                healthBarFillArea.color = Color.Lerp(healthBarFillArea.color, healthBarFillColor2, perc);
            }
            else if (health <= healthTrigger2)
            {
                healthBarFillArea.color = Color.Lerp(healthBarFillArea.color, healthBarFillColor3, perc);
            }
        }
        if (exBar.GetComponent<Slider>().value != damageDealt)
        {
            tValue += speedBar * Time.unscaledDeltaTime;

            if (tValue > totalTimeBar)
            {
                tValue = totalTimeBar;
            }

            perc = tValue / totalTimeBar;

            exBar.GetComponent<Slider>().value = Mathf.SmoothStep(exBar.GetComponent<Slider>().value, damageDealt, perc);
        }
    }

    public void Win ()
    {
        Destroy(bossTransform.gameObject);
        Time.timeScale = 0f;
        Cursor.visible = true;
        pauseMenu.someoneDied = true;
        AudioManager.instance.StopAllSounds();

        winScreen.SetActive(true);
    }

    public void OutOfBounds()
    {
        if (bossTransform.GetComponent<Boss>().enabled == true)
        {
            outOfBoundsDam = outOfBoundsDamage;
        }
        else
        {
            outOfBoundsDam = 0f;
        }

        // Out of bounds
        playerTransform.position = spawnPositions[spawnPointNumber].position;
        playerTransform.GetComponent<Player>().Invulnerable();
        GetDamaged(outOfBoundsDam);
    }

    public void GetDamaged (float damage)
    {
        health -= damage;

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
            bossTransform.GetComponent<Boss>().flashColor = bossFlashColor3;
            damage = damage * 2;
        }
        else if (health <= healthTrigger1)
        {
            bossTransform.GetComponent<Boss>().flashColor = bossFlashColor2;
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
        AudioManager.instance.StopAllSounds();

        gameOver.SetActive(true);
    }
}

// no bugs plz