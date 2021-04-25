using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Exit()
    {
        StartCoroutine(QuitGame());
    }

    public void Options()
    {
        SoundManager.PlaySound("ClickMenu");
    }

    public IEnumerator QuitGame()
    {
        //SoundManager.PlaySound("Return");
        SoundManager.PlaySound("ClickMenu");
        yield return new WaitForSeconds(1.0f);
        Application.Quit();
    }

    public void ExitOptionsMenu()
    {
        //SoundManager.PlaySound("Return");
        SoundManager.PlaySound("ClickMenu");
    }

    public void HoverSound()
    {
        SoundManager.PlaySound("HoverMenu");
    }
}
