using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenu, UI;

    public void OpenMenu()
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

    public void Resume() 
    {
        UI.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SoundManager.PlaySound("ClickMenu");
    }

    void Pause() 
    {
        UI.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        SoundManager.PlaySound("ClickMenu");
    }

    public void LoadMenu() 
    {
        SoundManager.PlaySound("ClickMenu");
    }

    public void QuitGame()
    {
        //SoundManager.PlaySound("Return");
        SoundManager.PlaySound("ClickMenu");
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitOptionsMenu()
    {
        SoundManager.PlaySound("ClickMenu");
        //SoundManager.PlaySound("Return");
    }

    public void HoverSound()
    {
        SoundManager.PlaySound("HoverMenu");
    }
}
