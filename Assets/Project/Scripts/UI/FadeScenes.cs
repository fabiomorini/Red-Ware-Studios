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
        SoundManager.PlaySound("Play");
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
