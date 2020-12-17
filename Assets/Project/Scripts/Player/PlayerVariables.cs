using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public int coins = 0;
    public int allies_counter = 0;
    private GridCombatSystem gridCombatSystem;

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
