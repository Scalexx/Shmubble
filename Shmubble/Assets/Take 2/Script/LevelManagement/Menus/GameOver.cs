using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour {

    public Slider progress;

    public List<string> flavorTexts = new List<string>();

    public TMP_Text flavorText;

    void OnEnable()
    {
        int rand = Random.Range(0, flavorTexts.Count);

        flavorText.text = flavorTexts[rand];

        progress.value = progress.maxValue - LevelManager.Instance.bossHealth;
    }

}

// no bugs plz
