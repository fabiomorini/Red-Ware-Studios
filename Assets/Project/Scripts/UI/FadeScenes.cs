using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScenes : MonoBehaviour
{
    public Animator animator;

    public void PlayLevel()
    {
        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame()
    {
        if(SceneManager.GetActiveScene().name == "Main Menu") SoundManager.PlaySound("Play");
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayMapamundi()
    {
        StartCoroutine(Mapamundi());
    }

    public IEnumerator Mapamundi()
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
