using System.Collections;
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

    [HideInInspector] public int meleeLevel = 1;
    [HideInInspector] public int archerLevel = 1;
    [HideInInspector] public int healerLevel = 1;
    [HideInInspector] public int tankLevel = 1;
    [HideInInspector] public int mageLevel = 1;

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
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                reward = RewardL1;
                break;
            case 3:
                reward = RewardL2;
                break;
            case 4:
                reward = RewardL3;
                break;
            case 5:
                reward = RewardL4;
                break;
            case 6:
                reward = RewardL5;
                break;
        }
        return reward;
    }
}
