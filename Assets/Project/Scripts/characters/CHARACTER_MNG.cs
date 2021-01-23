using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHARACTER_MNG : MonoBehaviour
{

    public int coins;
    public int numberOfMelee;
    public int numberOfRanged;
    public int numberOfHealer;
    [HideInInspector] public int numberOfAllies;
    [HideInInspector] public int RewardL1 = 300;
    [HideInInspector] public int RewardL2 = 350;
    [HideInInspector] public int RewardL3 = 400;
    [HideInInspector] public int RewardL4 = 450;
    [HideInInspector] public int RewardL5 = 500;
    [HideInInspector] public int RewardL6 = 550;
    [HideInInspector] public int RewardL7 = 600;
    [HideInInspector] public List<CHARACTER_PREFS> characterPrefs;

    private void Start()
    {
        characterPrefs = new List<CHARACTER_PREFS>();
        // leer todas las player prefs de cada personaje
        // se las añadimos por cada index
        DontDestroyOnLoad(this.gameObject);
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

    public int NumOfAllies()
    {
        return numberOfAllies = numberOfMelee 
                              + numberOfRanged 
                              + numberOfHealer;
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

    public void CheckIfDead()
    {
        for (int i = 0; i < NumOfAllies(); i++)
        {
            if (characterPrefs[i].GetComponent<UnitGridCombat>().imDead)
            {
                characterPrefs.RemoveAt(i);
                numberOfAllies--;
            }
        }
    }
}
