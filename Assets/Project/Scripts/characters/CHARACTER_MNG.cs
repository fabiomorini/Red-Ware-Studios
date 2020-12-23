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
        // DontDestroyOnLoad(this.gameObject);
    }

    public int numAllies()
    {
        return numberOfAllies = numberOfHealer + numberOfRanged + numberOfMelee;
    }

    public void checkIfDead()
    {
        for (int i = 0; i < numberOfAllies; i++)
        {
            if (characterPrefs[i].GetComponent<UnitGridCombat>().imDead)
            {
                characterPrefs.RemoveAt(i);
                numberOfAllies--;
            }
        }
    }
}
