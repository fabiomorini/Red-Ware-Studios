using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspirationUI : MonoBehaviour
{
    public GameObject Inspiration1;
    public GameObject Inspiration2;
    public GameObject Inspiration3;
    public GameObject Inspiration4;

    [HideInInspector] public bool pointMove = false;
    [HideInInspector] public bool pointAttack = false;

    public Button inspiredAttack;
    public Button inspiredMove;

    public GridCombatSystem combatSystem;

    public Button ability1;
    public Button ability2;

    public GameObject habilitiesUI;
    private bool abilitiesUIShowing = false;

    private void Update()
    {
        SetInspirationUI();
        SetAbilitiesInteractable();
        /*if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE && combatSystem.attacking == false) 
        {
            ability1.interactable = false; 
        }
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER && combatSystem.attacking == false)
        {
            ability1.interactable = false;
            ability2.interactable = false;
        }*/
    }

    public void ResetAbilities()
    {
        ability1.interactable = true;
        ability2.interactable = true;
    }

    private void SetInspirationUI()
    {
        if (combatSystem.inspiration == 0)
        {
            Inspiration1.SetActive(false);
            Inspiration2.SetActive(false);
            Inspiration3.SetActive(false);
            Inspiration4.SetActive(false);
        }
        else if (combatSystem.inspiration == 1)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(false);
            Inspiration3.SetActive(false);
            Inspiration4.SetActive(false);
        }
        else if (combatSystem.inspiration == 2)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(true);
            Inspiration3.SetActive(false);
            Inspiration4.SetActive(false);
        }
        else if (combatSystem.inspiration == 3)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(true);
            Inspiration3.SetActive(true);
            Inspiration4.SetActive(false);
        }
        else if (combatSystem.inspiration == 4)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(true);
            Inspiration3.SetActive(true);
            Inspiration4.SetActive(true);
        }
    } //enseña la energía en la UI

    public void InspiredMovement() // para los botones de inspiración move
    {
        pointMove = !pointMove;
        Debug.Log(pointMove);
        if (pointMove && combatSystem.inspiration > 0) {
            combatSystem.inspiredMovement = true;
            combatSystem.hasUpdatedPositionMove = false;
        }
        else
        {
            combatSystem.inspiredMovement = false;
            combatSystem.hasUpdatedPositionMove = false;
        }
    }

    public void InspiredAttack()  // para los botones de inspiración attack
    {
        pointAttack = !pointAttack;
        if (pointAttack && combatSystem.inspiration > 0) { 
            combatSystem.inspiredAttack = true;
        }
        else { combatSystem.inspiredAttack = false; }
    }

    public void ShowAbilitiesUI()
    {
        if (!abilitiesUIShowing)
        {
            habilitiesUI.SetActive(true);
            abilitiesUIShowing = true;
        }
        else if (abilitiesUIShowing)
        {
            habilitiesUI.SetActive(false);
            abilitiesUIShowing = false;
        }
    }

    public void StopShowingAbilitiesUI()
    {
        habilitiesUI.SetActive(false);
        abilitiesUIShowing = false;
    }

    public void SetAbilitiesInteractable()
    {
        // N1
        if(combatSystem.inspiration > 2) { ability1.interactable = true; }
        else { ability1.interactable = false; }

        // N3
        if (combatSystem.inspiration > 3 && combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().level == CHARACTER_PREFS.Level.NIVEL3)
        { ability2.interactable = true; }
        else { ability2.interactable = false; }
    }

    public void HabilitiesSelectorL1()
    {
        pointAttack = false;
        pointMove = false;
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            combatSystem.doubleSlash = true;
            combatSystem.feedbackHability = true;
            combatSystem.SetAbilityTrue();
            combatSystem.AttackAllyVisual();
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            combatSystem.boltOfPrecision = true;
            combatSystem.feedbackHability = true;
            combatSystem.hasUpdatedPositionAttack = false;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            combatSystem.feedbackHability = true;
            combatSystem.hexOfNature = true;
            combatSystem.SetAbilityTrue();
            combatSystem.AttackAllyVisual();
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {

            combatSystem.overload = true;
            combatSystem.feedbackHability = true;
            for (int i = 0; i < combatSystem.alliesTeamList.Count; i++)
            {
                if (combatSystem.alliesTeamList[i] == combatSystem.unitGridCombat)
                {
                    combatSystem.unitGridCombat.GetComponent<UnitGridCombat>().isOverloaded = true;
                    combatSystem.unitGridCombat.GetComponent<UnitGridCombat>().DoOverloadFeedback();
                }
            }
            combatSystem.inspiration -= 3;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            combatSystem.feedbackHability = true;
            combatSystem.fireBurst = true;
            combatSystem.SpawnGridHability();
        }
    }  // aquí llamamos las habilidades

    public void HabilitiesSelectorL2()
    {
        pointAttack = false;
        pointMove = false;
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            combatSystem.justicesExecute = true;
            combatSystem.feedbackHability = true;
            combatSystem.SetAbilityTrue();
            combatSystem.AttackAllyVisual();
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            combatSystem.feedbackHability = true;
            combatSystem.windRush = true;
            combatSystem.SetAbilityTrue();
            combatSystem.AttackAllyVisual();
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            combatSystem.feedbackHability = true;
            combatSystem.divineGrace = true;
            combatSystem.unitGridCombat.HealAlly(combatSystem.unitGridCombat);
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            combatSystem.whirlwind = true;
            combatSystem.feedbackHability = true;
            combatSystem.SetAbilityTrue();
            combatSystem.AttackAllyVisual();
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            combatSystem.feedbackHability = true;
            combatSystem.shatter = true;
            combatSystem.SetAbilityTrue();
            combatSystem.AttackAllyVisual();
        }
    }  // aquí llamamos las habilidades
}
