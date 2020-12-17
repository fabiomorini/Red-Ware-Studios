using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CHARACTER_MNG : MonoBehaviour
{
    public GridCombatSystem gridCombatSystem;
    public GameObject Ally;
    public int numberOfAllies;
    private int maxOfCharacters;
    [HideInInspector]
    public List<CHARACTER_PREFS> characterPrefs;
    private GameObject hola; // deep lore

    //Escenas de Unity por buildIndex
    private int IndexL1 = 1;
    private int IndexL2 = 3;
    private int IndexL3 = 4;
    private int IndexL4 = 5;
    private int IndexL5 = 6;
    private int IndexL6 = 7;

    //MAX personajes por nivel
    private int maxL1 = 5;
    private int maxL2 = 5;
    private int maxL3 = 5;
    private int maxL4 = 5;
    private int maxL5 = 5;
    private int maxL6 = 5;

    private void Start()
    {
        characterPrefs = new List<CHARACTER_PREFS>();
        checkMaxCharacters();
        spawnCharacters();

        // leer todas las player prefs de cada personaje
        // se las añadimos por cada index
    }

    // Dependiendo de la escena, mira el build index del scene manager y asigna un maximo de personajes jugables 
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
    public void checkIfDead()
    {
        for (int i = 0; i < numberOfAllies; i++)
        {
            if (characterPrefs.Equals(null))
            {
                characterPrefs.RemoveAt(i);
                numberOfAllies--;
            }
        }
    }
}
