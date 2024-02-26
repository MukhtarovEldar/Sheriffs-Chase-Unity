using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public List<GameObject> pauseMenuUI;
    public AudioSource backgroundMusic;

    public bool isPaused = false;

    void Start()
    {
        SetPauseMenuUIActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
                backgroundMusic.UnPause();
            }
            else
            {
                Pause();
                backgroundMusic.Pause();
            }
        }
    }

    void Pause()
    {
        SetPauseMenuUIActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        SetPauseMenuUIActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void SetPauseMenuUIActive(bool isActive)
    {
        foreach (GameObject uiElement in pauseMenuUI)
        {
            uiElement.SetActive(isActive);
        }
    }
}
