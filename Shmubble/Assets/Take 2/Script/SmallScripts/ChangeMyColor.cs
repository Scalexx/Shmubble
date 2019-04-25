using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMyColor : MonoBehaviour {

    [Tooltip("Start color should be changed when health is low.")]
    public bool changeStartColor;
    [Tooltip("Color over lifetime should be changed when health is low.")]
    public bool changeColorOverLifetime;

    [Space(10)]
    public Color startColor1;
    public Color startColor2;
    public Color startColor3;

    [Space(10)]
    public Gradient colorOverLifetime1;
    public Gradient colorOverLifetime2;
    public Gradient colorOverLifetime3;

    private float health;
    private float healthTrigger1;
    private float healthTrigger2;

    void Update()
    {
        health = LevelManager.Instance.health;
        healthTrigger1 = LevelManager.Instance.healthTrigger1;
        healthTrigger2 = LevelManager.Instance.healthTrigger2;

        var col = GetComponent<ParticleSystem>().colorOverLifetime;
        var main = GetComponent<ParticleSystem>().main;


        if (health > healthTrigger1)
        {
            if (changeStartColor)
            {
                main.startColor = startColor1;
            }
            if(changeColorOverLifetime)
            {
                col.color = colorOverLifetime1;
            }
        }
        else if (health <= healthTrigger1 && health > healthTrigger2)
        {
            if (changeStartColor)
            {
                main.startColor = startColor2;
            }
            if (changeColorOverLifetime)
            {
                col.color = colorOverLifetime2;
            }
        }
        else if (health <= healthTrigger2)
        {
            if (changeStartColor)
            {
                main.startColor = startColor3;
            }
            if (changeColorOverLifetime)
            {
                col.color = colorOverLifetime3;
            }
        }

    }

}

// no bugs plz
