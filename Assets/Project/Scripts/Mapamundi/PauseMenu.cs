using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenu, UI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) 
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    public void Resume() 
    {
        UI.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause() 
    {
        UI.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu() 
    {
        SoundManager.PlaySound("ClickMenu");
    }

    public void QuitGame()
    {
        SoundManager.PlaySound("Return");
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void ExitOptionsMenu()
    {
        SoundManager.PlaySound("Return");
    }

    public void HoverSound()
    {
        SoundManager.PlaySound("HoverMenu");
    }
}
