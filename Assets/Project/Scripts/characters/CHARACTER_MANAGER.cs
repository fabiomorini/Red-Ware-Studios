using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHARACTER_MANAGER : MonoBehaviour
{
    public GridCombatSystem gridCombatSystem;
    public GameObject Ally;
    public int numberOfAllies = 3;
    private const int maxOfAlliesBattle1 = 6;
    private List<CHARACTER_PREFS> character_base;
    private GameObject hola; // deep lore

    private void Start()
    {
        character_base = new List<CHARACTER_PREFS>();

        // leer todas las player prefs de cada personaje
        // se las añadimos por cada index

        if (numberOfAllies > maxOfAlliesBattle1)
        {
            numberOfAllies = maxOfAlliesBattle1;
        }

        for (int i = 0; i < numberOfAllies; i++)
        {
            // leer de la lista los playerprefs y añadirlos a los characters
            hola = Instantiate(Ally, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
            Ally.name = "Ally" + i;
            gridCombatSystem.unitGridCombatArray.Add(hola.GetComponent<UnitGridCombat>());
        }

    }


}
