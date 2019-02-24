using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour {

    public List<string> flavorTexts = new List<string>();

    public TMP_Text flavorText;

    void OnEnable()
    {
        int rand = Random.Range(0, flavorTexts.Count);

        flavorText.text = flavorTexts[rand];
    }

}

// no bugs plz
