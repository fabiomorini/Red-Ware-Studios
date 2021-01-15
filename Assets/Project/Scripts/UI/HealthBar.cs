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
    private UnitGridCombat unitGridCombat;


    private void Start()
    {
        unitGridCombat = gameObject.GetComponentInParent<UnitGridCombat>();
    }

    private void Update()
    {
        SetMaxHealth(unitGridCombat.maxHealth);
        SetHealth(unitGridCombat.curHealth);
        SetHealthNumber();
    }
    public void SetHealthNumber()
    {
        miniCurrHealth.SetText(""+ unitGridCombat.curHealth);
    }

    public void UpdateHealth(UnitGridCombat unitGridCombat)
    {
        SetMaxHealth(unitGridCombat.maxHealth);
        currHealthText.SetText("Health:  " + unitGridCombat.curHealth +"/" + unitGridCombat.maxHealth);
        levelText.SetText("Nv. " + unitGridCombat.level);
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
