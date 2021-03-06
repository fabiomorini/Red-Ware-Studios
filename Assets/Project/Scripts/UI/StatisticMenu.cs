﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatisticMenu : MonoBehaviour
{
    public GameObject knightSprite;
    public GameObject archerSprite;
    public GameObject healerSprite;
    public GameObject tankSprite;
    public GameObject mageSprite;

    public Slider slider;

    public TMP_Text currHealthText;
    public TMP_Text levelText;
    public TMP_Text tipoText;
    public TMP_Text atkText;
    private UnitGridCombat unitGridCombat;
    public GridCombatSystem gridCombatSystem;

    private void Update()
    {
        unitGridCombat = gridCombatSystem.unitGridCombat;
        SetMaxHealth(unitGridCombat.maxHealth);
        SetHealth(unitGridCombat.curHealth);
        UpdateSprite(unitGridCombat);
    }

    public void UpdateHealth(UnitGridCombat unitGridCombat)
    {
        SetMaxHealth(unitGridCombat.maxHealth);
        currHealthText.SetText("hp:  " + unitGridCombat.curHealth + " / " + unitGridCombat.maxHealth);
        SetHealth(unitGridCombat.curHealth);
    }
    
    private void SetLevelText()
    {
        if (unitGridCombat.GetComponent<CHARACTER_PREFS>().level == CHARACTER_PREFS.Level.NIVEL1)
        {
            levelText.SetText("Lvl. 1");
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().level == CHARACTER_PREFS.Level.NIVEL2)
        {
            levelText.SetText("Lvl. 2");
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().level == CHARACTER_PREFS.Level.NIVEL3)
        {
            levelText.SetText("Lvl. 3");
        }
    }
    
    public void UpdateSprite(UnitGridCombat unitGridCombat)
    {
        if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            knightSprite.SetActive(true);
            archerSprite.SetActive(false);
            healerSprite.SetActive(false);
            tankSprite.SetActive(false);
            mageSprite.SetActive(false);
            tipoText.SetText("Knight");
            atkText.SetText("Atk: 25");
            SetLevelText();
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            knightSprite.SetActive(false);
            archerSprite.SetActive(true);
            healerSprite.SetActive(false);
            tankSprite.SetActive(false);
            mageSprite.SetActive(false);
            tipoText.SetText("Archer");
            atkText.SetText("Atk: 20");
            SetLevelText();
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            knightSprite.SetActive(false);
            archerSprite.SetActive(false);
            healerSprite.SetActive(true);
            tankSprite.SetActive(false);
            mageSprite.SetActive(false);
            tipoText.SetText("Priest");
            atkText.SetText("Atk: 10");
            SetLevelText();
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            knightSprite.SetActive(false);
            archerSprite.SetActive(false);
            healerSprite.SetActive(false);
            tankSprite.SetActive(true);
            mageSprite.SetActive(false);
            tipoText.SetText("Paladin");
            atkText.SetText("Atk: 15");
            SetLevelText();
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            knightSprite.SetActive(false);
            archerSprite.SetActive(false);
            healerSprite.SetActive(false);
            tankSprite.SetActive(false);
            mageSprite.SetActive(true);
            tipoText.SetText("Wizard");
            atkText.SetText("Atk: 25");
            SetLevelText();
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
