using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private TMP_Text maxHealth;
    private TMP_Text currHealth;
    private TMP_Text level;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }
}
