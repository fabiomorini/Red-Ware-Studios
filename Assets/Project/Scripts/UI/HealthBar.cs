using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text currHealthText;
    public TMP_Text miniCurrHealth;
    public TMP_Text levelText;
    public UnitGridCombat unitGridCombat;

    private void Start()
    {
        levelText.SetText("Nv. " + unitGridCombat.level);
    }

    private void Update()
    {
        SetMaxHealth(unitGridCombat.maxHealth);
        SetHealth(unitGridCombat.curHealth);
        SetHealthNumber();
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
