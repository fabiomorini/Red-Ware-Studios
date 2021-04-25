using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public TMP_Text CurrHealth;

    public void SetHealthNumber(int curHealth)
    {
        if(gameObject.GetComponentInParent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.DUMMY) CurrHealth.SetText("0");
        else CurrHealth.SetText(""+ curHealth);
    }
}
