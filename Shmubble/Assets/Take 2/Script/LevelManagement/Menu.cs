using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour {

    public AudioMixer audioMixer;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropDown;

    void Start ()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void LoadScene (string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitButton ()
    {
        Application.Quit();
    }

    public void SetMasterVolume (float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume (float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}
