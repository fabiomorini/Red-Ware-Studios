using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelAlliesManager : MonoBehaviour
{

    private int numberOfAllies;
    private int maxOfCharacters;
    public GameObject Ally;
    public GridCombatSystem gridCombatSystem;
    private GameObject hola; // deep lore
    private List<CHARACTER_PREFS> characterPrefs;

    //Escenas de Unity por buildIndex
    private int IndexL1 = 2;
    private int IndexL2 = 3;
    private int IndexL3 = 4;
    private int IndexL4 = 5;
    private int IndexL5 = 6;
    private int IndexL6 = 7;

    //MAX personajes por nivel
    private int maxL1 = 3;
    private int maxL2 = 4;
    private int maxL3 = 4;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;

    private void Start()
    {
        numberOfAllies = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>().numAllies();
        characterPrefs = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>().characterPrefs;
        checkMaxCharacters();
        spawnCharacters();
        
    }
    public void spawnCharacters()
    {
        for (int i = 0; i < numberOfAllies; i++)
        {
            // leer de la lista los playerprefs y añadirlos a los characters
            hola = Instantiate(Ally, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
            Ally.name = "Ally" + i;
            gridCombatSystem.unitGridCombatArray.Add(hola.GetComponent<UnitGridCombat>());
            characterPrefs.Add(hola.GetComponent<CHARACTER_PREFS>());
        }
    }
    private void checkMaxCharacters()
    {
        if (SceneManager.GetActiveScene().buildIndex == IndexL1)
        {
            maxOfCharacters = maxL1;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL2)
        {
            maxOfCharacters = maxL2;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL3)
        {
            maxOfCharacters = maxL3;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL4)
        {
            maxOfCharacters = maxL4;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL5)
        {
            maxOfCharacters = maxL5;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL6)
        {
            maxOfCharacters = maxL6;
        }

        if (numberOfAllies > maxOfCharacters)
        {
            numberOfAllies = maxOfCharacters;
        }
    }
}

