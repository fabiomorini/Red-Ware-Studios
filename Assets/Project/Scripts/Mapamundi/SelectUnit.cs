using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectUnit : MonoBehaviour
{
    public GameObject UI;
    private int maxLevel;

    private int knightCounter = 0;
    private int ArcherCounter = 0;
    private int HealerCounter = 0;
    private int TankCounter = 0;
    private int MageCounter = 0;
    private int totalCounter = 0;
    private int knightsToFight = 0;
    private int archersToFight = 0;
    private int healersToFight = 0;
    private int tanksToFight = 0;
    private int magesToFight = 0;

    public TMP_Text explainText;
    public TMP_Text totalText;

    public TMP_Text ArcherText;
    public TMP_Text KnightText;
    public TMP_Text HealerText;
    public TMP_Text TankText;
    public TMP_Text MageText;

    public TMP_Text knightsToFightText;
    public TMP_Text archersToFightText;
    public TMP_Text healersToFightText;
    public TMP_Text tanksToFightText;
    public TMP_Text magesToFightText;

    private GameObject characterManager;
    public MapamundiManager minimapManager;

    private void Update()
    {
        CheckButtons();
        characterManager = GameObject.FindWithTag("characterManager");
        explainText.SetText("You can only bring " + maxLevel + " soldiers to the fight");

        knightCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfMelee;
        ArcherCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfRanged;
        HealerCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfHealer;
        TankCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfTank;
        MageCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfMage;

        KnightText.SetText("x " + knightCounter);
        ArcherText.SetText("x " + ArcherCounter);
        HealerText.SetText("x " + HealerCounter);
        TankText.SetText("x " + TankCounter);
        MageText.SetText("x " + MageCounter);
        totalText.SetText("Total Soldiers: " + totalCounter);

        knightsToFightText.SetText("x " + knightsToFight);
        archersToFightText.SetText("x " + archersToFight);
        healersToFightText.SetText("x " + healersToFight);
        tanksToFightText.SetText("x " + tanksToFight);
        magesToFightText.SetText("x " + magesToFight);
    }

    private void CheckButtons()
    {
        if (minimapManager.N1) maxLevel = minimapManager.maxL1;
        else if (minimapManager.N2) maxLevel = minimapManager.maxL2;
        else if (minimapManager.N3) maxLevel = minimapManager.maxL3;
    }


    public void AddKnight()
    {
        if (totalCounter < maxLevel && knightsToFight < knightCounter)
        {
            totalCounter++;
            knightsToFight++;
        }
    }

    public void AddArcher()
    {
        if (totalCounter < maxLevel && archersToFight < ArcherCounter)
        {
            totalCounter++;
            archersToFight++;
        }
    }

    public void AddHealer()
    {
        if(totalCounter < maxLevel && healersToFight < HealerCounter)
        {
            totalCounter++;
            healersToFight++;
        }
    }

    public void AddTank()
    {
        if (totalCounter < maxLevel && tanksToFight < TankCounter)
        {
            totalCounter++;
            tanksToFight++;
        }
    }

    public void AddMage()
    {
        if (totalCounter < maxLevel && magesToFight < MageCounter)
        {
            totalCounter++;
            magesToFight++;
        }
    }

    public void RemoveKnight()
    {
        if (totalCounter > 0 && knightsToFight > 0)
        {
            totalCounter--;
            knightsToFight--;
        }
    }

    public void RemoveArcher()
    {
        if (totalCounter > 0 && archersToFight > 0)
        {
            totalCounter--;
            archersToFight--;
        }
    }

    public void RemoveHealer()
    {
        if (totalCounter > 0 && healersToFight > 0)
        {
            totalCounter--;
            healersToFight--;
        }
    }

    public void RemoveTank()
    {
        if (totalCounter > 0 && tanksToFight > 0)
        {
            totalCounter--;
            tanksToFight--;
        }
    }

    public void RemoveMage()
    {
        if (totalCounter > 0 && magesToFight > 0)
        {
            totalCounter--;
            magesToFight--;
        }
    }

    public void StartBattle()
    {
        if ((knightsToFight + archersToFight + healersToFight + tanksToFight + magesToFight) > 0) 
        {
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = knightsToFight;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfArcherFight = archersToFight;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfHealerFight = healersToFight;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfTankFight = tanksToFight;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMageFight = magesToFight;
            PlayGame();
        }
    }

    public void PlayGame()
    {
        if (minimapManager.N1) SceneManager.LoadScene("Nivel1");
        if (minimapManager.N2) SceneManager.LoadScene("Nivel2");
        if (minimapManager.N3) SceneManager.LoadScene("Nivel3");
    }
}
