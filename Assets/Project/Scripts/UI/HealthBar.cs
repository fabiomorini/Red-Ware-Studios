using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject knightSprite;
    public GameObject archerSprite;
    public GameObject healerSprite;
    public TMP_Text currHealthText;
    private TMP_Text miniCurrHealth;
    public TMP_Text levelText;
    private UnitGridCombat unitGridCombat;

    private void Start()
    {
        levelText.SetText("Nv. " + unitGridCombat.level);
    }

    private void Update()
    {
        SetMaxHealth(unitGridCombat.maxHealth);
        SetHealth(unitGridCombat.curHealth);
        SetHealthNumber();
        UpdateSprite(unitGridCombat);
    }
    public void SetHealthNumber() //mini ui
    {
        miniCurrHealth.SetText(""+ unitGridCombat.curHealth);
    }

    public void UpdateHealth(UnitGridCombat unitGridCombat)
    {
        SetMaxHealth(unitGridCombat.maxHealth);
        currHealthText.SetText("HP:  " + unitGridCombat.curHealth +" / " + unitGridCombat.maxHealth);
        SetHealth(unitGridCombat.curHealth);
    }

    public void UpdateSprite(UnitGridCombat unitGridCombat)
    {
        if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            knightSprite.SetActive(true);
            archerSprite.SetActive(false);
            healerSprite.SetActive(false);
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            knightSprite.SetActive(false);
            archerSprite.SetActive(true);
            healerSprite.SetActive(false);
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            knightSprite.SetActive(false);
            archerSprite.SetActive(false);
            healerSprite.SetActive(true);
        }
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
