using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TMP_Text CurrHealth;

    public void SetHealthNumber(int curHealth)
    {
        if(gameObject.GetComponentInParent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.DUMMY) CurrHealth.SetText("99");
        else CurrHealth.SetText(""+ curHealth);
    }
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        if (this.gameObject.GetComponentInParent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.DUMMY) slider.value = 20000;
        slider.value = health;
    }

}
