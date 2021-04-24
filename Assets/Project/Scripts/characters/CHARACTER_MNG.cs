﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHARACTER_MNG : MonoBehaviour
{
    public int coins;
    public int numberOfMelee = 1;
    public int numberOfRanged = 1;
    public int numberOfHealer;
    public int numberOfTank;
    public int numberOfMage;
    public int numberOfMeleeFight;
    public int numberOfArcherFight;
    public int numberOfHealerFight;
    public int numberOfTankFight;
    public int numberOfMageFight;
    [HideInInspector] public int numberOfAllies = 2;
    [HideInInspector] public int RewardL1 = 300;
    [HideInInspector] public int RewardL2 = 350;
    [HideInInspector] public int RewardL3 = 400;
    [HideInInspector] public int RewardL4 = 450;
    [HideInInspector] public int RewardL5 = 500;
    [HideInInspector] public int RewardL6 = 550;
    [HideInInspector] public int RewardL7 = 600;
    [HideInInspector] public List<CHARACTER_PREFS> characterPrefs;

    [HideInInspector] public bool VictoryL1 = false;
    [HideInInspector] public bool VictoryL2 = false;
    [HideInInspector] public bool VictoryL3 = false;
    [HideInInspector] public bool VictoryL4 = false;
    [HideInInspector] public bool VictoryL5 = false;
    [HideInInspector] public bool VictoryL6 = false;
    [HideInInspector] public bool VictoryL7 = false;
    [HideInInspector] public bool VictoryL8 = false;
    [HideInInspector] public bool VictoryL9 = false;
    [HideInInspector] public bool VictoryL10 = false;

    public int meleeLevel = 3;
    public int archerLevel = 3;
    public int healerLevel = 3;
    public int tankLevel = 3;
    public int mageLevel = 3;

    [HideInInspector] public int meleeExp = 0;
    [HideInInspector] public int archerExp = 0;
    [HideInInspector] public int healerExp = 0;
    [HideInInspector] public int tankExp = 0;
    [HideInInspector] public int mageExp = 0;

    [HideInInspector] public int level2Exp = 90;
    [HideInInspector] public int level3Exp = 200;

    private void Start()
    {
        characterPrefs = new List<CHARACTER_PREFS>();
        // leer todas las player prefs de cada personaje
        // se las añadimos por cada index
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (meleeExp >= level3Exp)
        {
            meleeLevel = 3;
        }
        else if (meleeExp >= level2Exp)
        {
            meleeLevel = 2;
        }

        if (archerExp >= level3Exp)
        {
            archerLevel = 3;
        }
        else if (archerExp >= level2Exp)
        {
            archerLevel = 2;
        }

        if (healerExp >= level3Exp)
        {
            healerLevel = 3;
        }
        else if (healerExp >= level2Exp)
        {
            healerLevel = 2;
        }

        if (tankExp >= level3Exp)
        {
            tankLevel = 3;
        }
        else if (tankExp >= level2Exp)
        {
            tankLevel = 2;
        }

        if (mageExp >= level3Exp)
        {
            mageLevel = 3;
        }
        else if (mageExp >= level2Exp)
        {
            mageLevel = 2;
        }
    }

    public void CheckLevelNumber()
    {
        if(SceneManager.GetActiveScene().name == "Nivel1")
        {
            VictoryL1True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel2")
        {
            VictoryL2True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel3")
        {
            VictoryL3True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel4")
        {
            VictoryL4True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel5")
        {
            VictoryL5True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel6")
        {
            VictoryL6True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel7")
        {
            VictoryL7True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel8")
        {
            VictoryL8True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel9")
        {
            VictoryL9True();
        }
        else if (SceneManager.GetActiveScene().name == "Nivel10")
        {
            VictoryL10True();
        }
    }
    private void VictoryL1True()
    {
        VictoryL1 = true;
    }
    private void VictoryL2True()
    {
        VictoryL2 = true;
    }
    private void VictoryL3True()
    {
        VictoryL3 = true;
    }
    private void VictoryL4True()
    {
        VictoryL4 = true;
    }
    private void VictoryL5True()
    {
        VictoryL5 = true;
    }
    private void VictoryL6True()
    {
        VictoryL6 = true;
    }
    private void VictoryL7True()
    {
        VictoryL7 = true;
    }
    private void VictoryL8True()
    {
        VictoryL8 = true;
    }
    private void VictoryL9True()
    {
        VictoryL9 = true;
    }
    private void VictoryL10True()
    {
        VictoryL10 = true;
    }
    public int NumMelee()
    {
        return numberOfMelee;
    }
    public int NumRanged()
    {
        return numberOfRanged;
    }
    public int NumHealers()
    {
        return numberOfHealer;
    }
    public int NumTanks()
    {
        return numberOfTank;
    }
    public int NumMages()
    {
        return numberOfMage;
    }
    public int NumOfAllies()
    {
        return numberOfAllies = numberOfMelee 
                              + numberOfRanged 
                              + numberOfHealer
                              + numberOfTank
                              + numberOfMage;
    }
    public int GetLevelIndex()
    {
        int reward = 0;
        switch (SceneManager.GetActiveScene().name)
        {
            case "Nivel1":
                reward = RewardL1;
                break;
            case "Nivel2":
                reward = RewardL2;
                break;
            case "Nivel3":
                reward = RewardL3;
                break;
            case "Nivel4":
                reward = RewardL4;
                break;
            case "Nivel5":
                reward = RewardL5;
                break;
        }
        return reward;
    }
}
