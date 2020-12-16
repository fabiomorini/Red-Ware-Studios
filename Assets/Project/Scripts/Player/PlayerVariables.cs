using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public int coins = 0;
    public int allies_counter = 0;
    private GridCombatSystem gridCombatSystem;
    [HideInInspector]
    public List<CHARACTER_PREFS> alliesList;
    private CHARACTER_PREFS characterPrefs;


    private void Start()
    {
        alliesList = new List<CHARACTER_PREFS>();

        for (int i = 0; i < allies_counter; i++)
        {
            alliesList.Add(characterPrefs);
        }
    }

    public void spawnAlly() {
        // pasarle a la funcion el máximo de tropas por cada nivel
        // comparar allies counter 
        // for que por cada aliado spawn un prefab de ally en la escena de batalla con las estadísticas
        // el maximo del for es el numero de aliados máximos que puedes espawnear, si en la escena puedes jugar con 10, el for llega a 10
    }

    public void BuyAlly(int price)
    {
        coins -= price;
        allies_counter++;
    }

    public void AllyDead()
    {
        allies_counter--;
    }
}
