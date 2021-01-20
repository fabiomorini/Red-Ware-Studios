using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animation fadeIn;

    public void PlayLevel() 
    {
        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame()
    {
        SoundManager.PlaySound("Play");
        yield return new WaitForSeconds(0.5f);
        //fadeIn.Play();
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Exit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
