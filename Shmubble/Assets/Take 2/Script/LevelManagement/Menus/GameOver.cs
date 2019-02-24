using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour {

    public Slider progress;

    public List<string> flavorTexts = new List<string>();

    public TMP_Text flavorText;

    public float speed;
    float tValue;

    void OnEnable()
    {
        int rand = Random.Range(0, flavorTexts.Count);

        flavorText.text = flavorTexts[rand];
    }

    void Update()
    {
        float progressValue = progress.maxValue - LevelManager.Instance.bossHealth;
        if (progress.value < progressValue)
        {
            tValue += speed * Time.unscaledDeltaTime;
            progress.value = Mathf.SmoothStep(progress.minValue, progressValue, tValue);
        }
    }

}

// no bugs plz
