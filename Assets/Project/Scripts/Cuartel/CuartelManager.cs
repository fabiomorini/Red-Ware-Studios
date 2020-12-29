using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuartelManager : MonoBehaviour
{
    private int knightCounter = 0;
    private int ArcherCounter = 0;
    [SerializeField]
    private int knightPrice = 100;
    [SerializeField]
    private int archerPrice = 120;

    public TMP_Text coinsText;
    public TMP_Text ArcherText;
    public TMP_Text KnightText;
    public GameObject cuartel;

    public GameObject characterManager;
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
        coinsText.SetText("¥ " + characterManager.GetComponent<CHARACTER_MNG>().coins);
        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
    }

    public void BuyKnight()
    {
        if(characterManager.GetComponent<CHARACTER_MNG>().coins >= knightPrice)
        {
            characterManager.GetComponent<CHARACTER_MNG>().coins -= knightPrice;
            knightCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMelee++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }

    }
    public void BuyArcher()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= knightPrice)
        {
            characterManager.GetComponent<CHARACTER_MNG>().coins -= archerPrice;
            ArcherCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfRanged++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
    }
    public void BuyHealer()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= knightPrice)
        {
            characterManager.GetComponent<CHARACTER_MNG>().coins -= archerPrice;
            ArcherCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfHealer++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
    }
 
}
