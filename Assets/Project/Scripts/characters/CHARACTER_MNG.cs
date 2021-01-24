using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHARACTER_MNG : MonoBehaviour
{
    private GameObject player;
    [HideInInspector] public Vector3 playerPosition;

    public int coins;
    public int numberOfMelee;
    public int numberOfRanged;
    public int numberOfHealer;
    public int numberOfMeleeFight;
    public int numberOfArcherFight;
    public int numberOfHealerFight;
    [HideInInspector] public int numberOfAllies;
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
    private MinimapManager minimapManager;


    private void Start()
    {
        playerPosition = new Vector3(-30f, -2.5f, 0f);
        characterPrefs = new List<CHARACTER_PREFS>();
        // leer todas las player prefs de cada personaje
        // se las añadimos por cada index
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void CheckLevelNumber()
    {
        if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            VictoryL1True();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            VictoryL2True();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
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

    public void KeepPlayerPosition()
    {
        playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, 0);
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
}
