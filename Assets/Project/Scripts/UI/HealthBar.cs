using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text CurrHealth;
    private UnitGridCombat unitGridCombat;

    private void Start()
    {
        unitGridCombat = GetComponentInParent<UnitGridCombat>();
        SetMaxHealth(unitGridCombat.maxHealth);
    }

    private void Update()
    {
        SetHealth(unitGridCombat.curHealth);
        SetHealthNumber();
    }

    public void SetHealthNumber()
    {
        CurrHealth.SetText(""+ unitGridCombat.curHealth);
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
