using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuartelManager : MonoBehaviour
{
    private int knightCounter = 0;
    private int ArcherCounter = 0;
    private int HealerCounter = 0;
    [SerializeField] private int knightPrice = 100;
    [SerializeField] private int archerPrice = 130;
    [SerializeField] private int healerPrice = 160;

    public TMP_Text coinsText;
    public TMP_Text ArcherText;
    public TMP_Text KnightText;
    public TMP_Text HealerText;
    public GameObject cuartel;

    public GameObject characterManager;
    private bool isActive;

    private void Start()
    {
        if (GameObject.FindWithTag("characterManager") == null)
        {
            Instantiate(characterManager);
        }
        characterManager = GameObject.FindWithTag("characterManager");
        knightCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfMelee;
        ArcherCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfRanged;
        HealerCounter = characterManager.GetComponent<CHARACTER_MNG>().numberOfHealer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isActive)
        {
            SoundManager.PlaySound("openCuartel");
            cuartel.SetActive(true);
            isActive = true;
        }
        if (Input.GetKey("escape") && isActive)
        {
            SoundManager.PlaySound("closeCuartel");
            cuartel.SetActive(false);
            isActive = false;
        }
        coinsText.SetText("¥ " + characterManager.GetComponent<CHARACTER_MNG>().coins);
        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
        HealerText.SetText("x " + HealerCounter);
    }

    public void BuyKnight()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= knightPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= knightPrice;
            knightCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMelee++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }

    }
    public void BuyArcher()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= archerPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= archerPrice;
            ArcherCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfRanged++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
    }
    public void BuyHealer()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= healerPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= healerPrice;
            HealerCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfHealer++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
    }

    public void CheatButton()
    {
        characterManager.GetComponent<CHARACTER_MNG>().coins += 200;
    }

}
