using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SelectUnit : MonoBehaviour
{
    public GameObject UI;

    private int maxL1 = 3;
    private int maxL2 = 4;
    private int maxL3 = 5;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;
    private int maxLevel;

    private int knightCounter = 0;
    private int ArcherCounter = 0;
    private int HealerCounter = 0;
    private int totalCounter = 0;
    private int knightsToFight = 0;
    private int archersToFight = 0;
    private int healersToFight = 0;

    public TMP_Text explainText;
    public TMP_Text totalText;

    public TMP_Text ArcherText;
    public TMP_Text KnightText;
    public TMP_Text HealerText;

    public TMP_Text knightsToFightText;
    public TMP_Text archersToFightText;
    public TMP_Text healersToFightText;

    private GameObject characterManager;
    private MinimapManager minimapManager;

    private void Update()
    {
        characterManager = GameObject.FindWithTag("characterManager");
        minimapManager = GameObject.FindWithTag("MinimapManager").GetComponent<MinimapManager>();
        CheckLevelMax();
        explainText.SetText("You can only bring " + maxLevel + " soldiers to the fight");

        knightCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfMelee;
        ArcherCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfRanged;
        HealerCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfHealer;

        KnightText.SetText("x " + knightCounter);
        ArcherText.SetText("x " + ArcherCounter);
        HealerText.SetText("x " + HealerCounter);
        totalText.SetText("Total Soldiers: " + totalCounter);

        knightsToFightText.SetText("x " + knightsToFight);
        archersToFightText.SetText("x " + archersToFight);
        healersToFightText.SetText("x " + healersToFight);
    }

    private void CheckLevelMax()
    {
        if (minimapManager.L1)
        {
            maxLevel = maxL1;
        }
        if (minimapManager.L2)
        {
            maxLevel = maxL2;
        }
        if (minimapManager.L3)
        {
            maxLevel = maxL3;
        }
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

    public void StartBattle()
    {
        characterManager.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = knightsToFight;
        characterManager.GetComponent<CHARACTER_MNG>().numberOfArcherFight = archersToFight;
        characterManager.GetComponent<CHARACTER_MNG>().numberOfHealerFight = healersToFight;
        StartCoroutine(PlayGame());
    }

    public IEnumerator PlayGame()
    {
        if (minimapManager.L1)
        {
            SoundManager.PlaySound("playLevel");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (minimapManager.L2)
        {
            SoundManager.PlaySound("playLevel");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        if (minimapManager.L3)
        {
            SoundManager.PlaySound("playLevel");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }

    }
}
