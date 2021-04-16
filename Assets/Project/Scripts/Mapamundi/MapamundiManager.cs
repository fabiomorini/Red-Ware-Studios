using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapamundiManager : MonoBehaviour
{
    private GameObject CombatHandler;
    public GameObject UI;

    [HideInInspector] public bool N1;
    [HideInInspector] public bool N2;
    [HideInInspector] public bool N3;

    public Button Nv1;
    public Button Nv2;
    public Button Nv3;

    [HideInInspector] public int maxL1 = 3;
    [HideInInspector] public int maxL2 = 3;
    [HideInInspector] public int maxL3 = 4;
    [HideInInspector] public int maxL4 = 5;
    [HideInInspector] public int maxL5 = 6;
    [HideInInspector] public int maxL6 = 7;

    private void Start()
    {
        Nv1.interactable = true;
        Nv2.interactable = false;
        Nv3.interactable = true;
    }

    private void Update()
    {
        CombatHandler = GameObject.FindWithTag("characterManager");
        CombatHandler.GetComponent<CHARACTER_MNG>().NumOfAllies();
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL1)
        {
            Nv2.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL2)
        {
            Nv3.interactable = true;
        }
    }

    public void Nivel1()
    {
        N1 = true;
        N2 = false;
        N3 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                SceneManager.LoadScene("Nivel1");
            }
        }
    }

    public void Nivel2()
    {
        N1 = false;
        N2 = true;
        N3 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL2)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                SceneManager.LoadScene("Nivel2");
            }
        }
    }

    public void Nivel3()
    {
        N1 = false;
        N2 = false;
        N3 = true;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL3)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                SceneManager.LoadScene("Nivel3");
            }
        }
    }
}
