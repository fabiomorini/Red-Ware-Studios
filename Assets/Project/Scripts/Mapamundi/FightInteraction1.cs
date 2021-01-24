using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightInteraction1 : MonoBehaviour
{
    public GameObject pressEText;
    private bool pressedCombatButton = false;
    private GameObject CombatHandler;

    private void Update(){
        CombatHandler = GameObject.FindWithTag("characterManager");
        CombatHandler.GetComponent<CHARACTER_MNG>().NumOfAllies();
        if (pressedCombatButton && Input.GetKeyDown(KeyCode.E) && CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
            StartCoroutine(PlayGame());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pressEText.SetActive(true);
            pressedCombatButton = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pressEText.SetActive(false);
            pressedCombatButton = false;
        }
    }

    public IEnumerator PlayGame()
    {
        SoundManager.PlaySound("playLevel");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}