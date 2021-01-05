using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHARACTER_MNG : MonoBehaviour
{
    [HideInInspector]
    public int numberOfAllies;
    public int numberOfMelee;
    public int numberOfRanged;
    public int numberOfHealer;
    public int coins;

    [HideInInspector] 
    public List<CHARACTER_PREFS> characterPrefs;

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
