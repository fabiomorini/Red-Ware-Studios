using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuartelManager : MonoBehaviour
{
    private int knightCounter = 0;
    private int ArcherCounter = 0;
    private int HealerCounter = 0;
    private int TankCounter = 0;
    private int MageCounter = 0;
    [SerializeField] private int knightPrice = 100;
    [SerializeField] private int archerPrice = 130;
    [SerializeField] private int healerPrice = 140;
    [SerializeField] private int tankPrice = 150;
    [SerializeField] private int magePrice = 160;

    public TMP_Text coinsText;
    public TMP_Text KnightText;
    public TMP_Text ArcherText;
    public TMP_Text HealerText;
    public TMP_Text TankText;
    public TMP_Text MageText;

    public TMP_Text KnightTextLvl;
    public TMP_Text ArcherTextLvl;
    public TMP_Text HealerTextLvl;
    public TMP_Text TankTextLvl;
    public TMP_Text MageTextLvl;
    public TMP_Text KnightTextExp;
    public TMP_Text ArcherTextExp;
    public TMP_Text HealerTextExp;
    public TMP_Text TankTextExp;
    public TMP_Text MageTextExp;

    public GameObject cuartel;
    public GameObject infoUI;

    public GameObject characterManager;
    [HideInInspector] public CHARACTER_MNG charManager;

    private bool isActive;

    private void Start()
    {
        if (GameObject.FindWithTag("characterManager") == null)
        {
            Instantiate(characterManager);
        }
        characterManager = GameObject.FindWithTag("characterManager");
        charManager = characterManager.GetComponent<CHARACTER_MNG>();
        knightCounter = charManager.numberOfMelee;
        ArcherCounter = charManager.numberOfRanged;
        HealerCounter = charManager.numberOfHealer;
        TankCounter = charManager.numberOfTank;
        MageCounter = charManager.numberOfMage;

        SetExpText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !isActive)
        {
            SoundManager.PlaySound("openCuartel");
            cuartel.SetActive(true);
            infoUI.SetActive(false);
            isActive = true;
        }
        if (Input.GetKey("escape") && isActive)
        {
            SoundManager.PlaySound("closeCuartel");
            cuartel.SetActive(false);
            infoUI.SetActive(true);
            isActive = false;
        }
        coinsText.SetText(charManager.coins + "g");
        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
        HealerText.SetText("x " + HealerCounter);
        TankText.SetText("x " + TankCounter);
        MageText.SetText("x " + MageCounter);
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
    public void BuyTank()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= tankPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= tankPrice;
            TankCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfTank++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
    }
    public void BuyMage()
    {
        if (characterManager.GetComponent<CHARACTER_MNG>().coins >= magePrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= magePrice;
            MageCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMage++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
    }

    public void CheatButton()
    {
        characterManager.GetComponent<CHARACTER_MNG>().coins += 200;
    }

    private void SetExpText()
    {
        if (charManager.meleeLevel == 1)
        {
            KnightTextLvl.SetText("Lvl 1");
            KnightTextExp.SetText(charManager.meleeExp + "/" + charManager.level2Exp);
        }
        else if (charManager.meleeLevel == 2)
        {
            KnightTextLvl.SetText("Lvl 2");
            KnightTextExp.SetText(charManager.meleeExp + "/" + charManager.level3Exp);
        }
        else
        {
            KnightTextLvl.SetText("Lvl 3");
            KnightTextExp.SetText("MAX / MAX");
        }
        ///
        if (charManager.archerLevel == 1)
        {
            ArcherTextLvl.SetText("Lvl 1");
            ArcherTextExp.SetText(charManager.archerExp + "/" + charManager.level2Exp);
        }
        else if (charManager.archerLevel == 2)
        {
            ArcherTextLvl.SetText("Lvl 2");
            ArcherTextExp.SetText(charManager.archerExp + "/" + charManager.level3Exp);
        }
        else
        {
            ArcherTextLvl.SetText("Lvl 3");
            ArcherTextExp.SetText("MAX / MAX");
        }
        ///
        if (charManager.healerLevel == 1)
        {
            HealerTextLvl.SetText("Lvl 1");
            HealerTextExp.SetText(charManager.healerExp + "/" + charManager.level2Exp);
        }
        else if (charManager.healerLevel == 2)
        {
            HealerTextLvl.SetText("Lvl 2");
            HealerTextExp.SetText(charManager.healerExp + "/" + charManager.level3Exp);
        }
        else
        {
            HealerTextLvl.SetText("Lvl 3");
            HealerTextExp.SetText("MAX / MAX");
        }
        ///
        if (charManager.tankLevel == 1)
        {
            TankTextLvl.SetText("Lvl 1");
            TankTextExp.SetText(charManager.tankExp + "/" + charManager.level2Exp);
        }
        else if (charManager.tankLevel == 2)
        {
            TankTextLvl.SetText("Lvl 2");
            TankTextExp.SetText(charManager.tankExp + "/" + charManager.level3Exp);
        }
        else
        {
            TankTextLvl.SetText("Lvl 3");
            TankTextExp.SetText("MAX / MAX");
        }
        ///
        if (charManager.mageLevel == 1)
        {
            MageTextLvl.SetText("Lvl 1");
            MageTextExp.SetText(charManager.mageExp + "/" + charManager.level2Exp);
        }
        else if (charManager.mageLevel == 2)
        {
            MageTextLvl.SetText("Lvl 2");
            MageTextExp.SetText(charManager.mageExp + "/" + charManager.level3Exp);
        }
        else
        {
            MageTextLvl.SetText("Lvl 3");
            MageTextExp.SetText("MAX / MAX");
        }
    }
}
