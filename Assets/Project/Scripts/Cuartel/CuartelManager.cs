using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuartelManager : MonoBehaviour
{
    public int coins = 0;
    private int knightCounter = 0;
    private int ArcherCounter = 0;
    private int knightPrice = 100;
    private int archerPrice = 120;

    public TMP_Text coinsText;
    public TMP_Text ArcherText;
    public TMP_Text KnightText;
    public GameObject cuartel;

    private bool isActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isActive)
        {
            cuartel.SetActive(true);
            isActive = true;
        }
        if(Input.GetKey("escape") && isActive)
        {
            cuartel.SetActive(false);
            isActive = false;
        }
        coinsText.SetText("¥ " + coins);
        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
    }

    public void BuyKnight()
    {
        if(coins >= knightPrice)
        {
            coins -= knightPrice;
            knightCounter++;
        }

    }
    public void BuyArcher()
    {
        if (coins >= knightPrice)
        {
            coins -= archerPrice;
            ArcherCounter++;
        }
    }

}
