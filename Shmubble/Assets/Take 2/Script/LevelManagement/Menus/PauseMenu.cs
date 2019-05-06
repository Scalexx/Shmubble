using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;

    public bool someoneDied = false;

    [HideInInspector]
    public Boss boss;

    void Start()
    {
        boss = FindObjectOfType<Boss>();
    }

    void Update ()
    {
        if (!someoneDied)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (gameIsPaused && settingsMenuUI.activeSelf)
                {
                    pauseMenuUI.SetActive(true);
                    settingsMenuUI.SetActive(false);
                }
                else if (gameIsPaused)
                {
                    AudioManager.instance.UnpauseSounds();

                    Resume();
                }
                else
                {
                    AudioManager.instance.PauseSounds();

                    Pause();
                }
            }
        }
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        Cursor.visible = false;
    }

    public void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        Cursor.visible = true;
    }

    public void LoadScene(string sceneToLoad)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

}

// no bugs plz