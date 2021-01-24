using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightInteraction2 : MonoBehaviour
{
    public GameObject UI;
    private int maxL1 = 3;
    private int maxL2 = 4;
    private int maxL3 = 5;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;

    public GameObject pressEText;
    private bool pressedCombatButton = false;
    private GameObject CombatHandler;
    private MinimapManager minimapManager;

    private void Update()
    {
        CombatHandler = GameObject.FindWithTag("characterManager");
        minimapManager = GameObject.FindWithTag("MinimapManager").GetComponent<MinimapManager>();
        CombatHandler.GetComponent<CHARACTER_MNG>().NumOfAllies();
        if (pressedCombatButton && Input.GetKeyDown(KeyCode.E) && CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL2)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                StartCoroutine(PlayGame());
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minimapManager.L1 = false;
            minimapManager.L2 = true;
            minimapManager.L3 = false;
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
        //GetComponent<CHARACTER_MNG>().KeepPlayerPosition();

        SoundManager.PlaySound("playLevel");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}