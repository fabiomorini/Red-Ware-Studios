using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightInteraction1 : MonoBehaviour
{
    public GameObject UI;
    private int maxL1 = 3;
    private int maxL2 = 4;
    private int maxL3 = 5;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;

    private int level = 1;

    public GameObject pressEText;
    private bool pressedCombatButton = false;
    private GameObject CombatHandler;
    private MinimapManager minimapManager;

    private void Update(){
        CombatHandler = GameObject.FindWithTag("characterManager");
        minimapManager = GameObject.FindWithTag("MinimapManager").GetComponent<MinimapManager>();
        CombatHandler.GetComponent<CHARACTER_MNG>().NumOfAllies();
        if (pressedCombatButton && Input.GetKeyDown(KeyCode.E) && CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                StartCoroutine(PlayGame());
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            minimapManager.L1 = true;
            minimapManager.L2 = false;
            minimapManager.L3 = false;
            pressEText.SetActive(true);
            pressedCombatButton = true;
        }
    }

    private void CheckLevel()
    {
        if(this.gameObject.CompareTag("L1"))
        {
            level = 1;
            minimapManager.L1 = true;
            minimapManager.L2 = false;
            minimapManager.L3 = false;
        }
        else if (this.gameObject.CompareTag("L2"))
        {
            level = 2;
            minimapManager.L1 = false;
            minimapManager.L2 = true;
            minimapManager.L3 = false;
        }
        else if (this.gameObject.CompareTag("L3"))
        {
            level = 3;
            minimapManager.L1 = false;
            minimapManager.L2 = false;
            minimapManager.L3 = true;
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
        SceneManager.LoadScene("Tutorial");
    }

}