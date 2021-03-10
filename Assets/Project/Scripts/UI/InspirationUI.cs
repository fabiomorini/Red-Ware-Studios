using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspirationUI : MonoBehaviour
{
    public GameObject pointMoveUI;
    public GameObject pointAttackUI;
    public Image AttackButton;
    public Image MoveButton;
    public Image Hability1Button;
    public Image Hability2Button;

    public GameObject pointHabilitiesUI;
    public GameObject Hability1UI;
    public GameObject Hability2UI;

    public GameObject Inspiration1;
    public GameObject Inspiration2;
    public GameObject Inspiration3;
    public GameObject Inspiration4;

    [HideInInspector] public int inspirationIndexUI;

    [HideInInspector] public bool pointMove = false;
    [HideInInspector] public bool pointAttack = false;
    [HideInInspector] public bool pointHability1 = false;
    [HideInInspector] public bool pointHability2 = false;
    [HideInInspector] public bool alreadyRestedInspiration = false;
    [HideInInspector] public bool alreadyUsedInspiration = false;

    public GridCombatSystem combatSystem;

    private void Start()
    {
        alreadyRestedInspiration = false;
        alreadyUsedInspiration = false;
        inspirationIndexUI = 4;
    }

    private void Update()
    {
        SetInspirationUI();
    }

    private void SetInspirationUI()
    {
        if (inspirationIndexUI == 0)
        {
            Inspiration1.SetActive(false);
            Inspiration2.SetActive(false);
            Inspiration3.SetActive(false);
            Inspiration4.SetActive(false);
        }
        else if (inspirationIndexUI == 1)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(false);
            Inspiration3.SetActive(false);
            Inspiration4.SetActive(false);
        }
        else if (inspirationIndexUI == 2)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(true);
            Inspiration3.SetActive(false);
            Inspiration4.SetActive(false);
        }
        else if (inspirationIndexUI == 3)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(true);
            Inspiration3.SetActive(true);
            Inspiration4.SetActive(false);
        }
        else if (inspirationIndexUI == 4)
        {
            Inspiration1.SetActive(true);
            Inspiration2.SetActive(true);
            Inspiration3.SetActive(true);
            Inspiration4.SetActive(true);
        }
    }

    public void ShowPointMove()
    {
        pointMoveUI.SetActive(true);
        pointAttackUI.SetActive(false);
        pointHabilitiesUI.SetActive(false);
        pointAttack = true;
        pointMove = true;
        pointHability1 = true;
        pointHability2 = true;
        combatSystem.inspiredAttack = false;
        combatSystem.inspiredMovement = false;
        DeactivateHabilities();
        if (alreadyRestedInspiration && !alreadyUsedInspiration)
        {
            inspirationIndexUI = combatSystem.inspiration;
            alreadyRestedInspiration = false;
        }
        InspirationMove();
        InspirationAttack();
        ActivateHability1();
        ActivateHability2();
    }
    public void ShowPointAttack()
    {
        pointAttackUI.SetActive(true);
        pointMoveUI.SetActive(false);
        pointHabilitiesUI.SetActive(false);
        pointAttack = true;
        pointMove = true; 
        pointHability1 = true;
        pointHability2 = true;
        combatSystem.inspiredAttack = false;
        combatSystem.inspiredMovement = false;
        DeactivateHabilities();
        if (alreadyRestedInspiration && !alreadyUsedInspiration)
        {
            inspirationIndexUI = combatSystem.inspiration;
            alreadyRestedInspiration = false;
        }
        InspirationAttack();
        InspirationMove();
        ActivateHability1();
        ActivateHability2();
    }

    public void ShowHabilities() 
    {
        pointMoveUI.SetActive(false);
        pointAttackUI.SetActive(false);
        pointHabilitiesUI.SetActive(true);
        //vuelve a poner los puntos en blanco
        pointAttack = true;
        pointMove = true;
        Hability1UI.GetComponent<Button>().interactable = false;

        if (alreadyRestedInspiration && !alreadyUsedInspiration)
        {
            inspirationIndexUI = combatSystem.inspiration;
            alreadyRestedInspiration = false;
        }
    }

    public void ActivateHability1()
    {
        if (pointHability1) Hability1Button.color = Color.white;
        if (combatSystem.inspiration > 2)
        {
            if (!pointHability1)
            {
                Hability1Button.color = new Color32(241, 178, 35, 255);
                pointHability1 = true;
                Hability1UI.GetComponent<Button>().interactable = true;
                //unitgridcombat llamar ataque especial = true
            }
            else
            {
                Hability1Button.color = Color.white;
                pointHability1 = false;
                Hability1UI.GetComponent<Button>().interactable = false;
                //unitgridcombat llamar ataque especial = false
            }
        }
    }

    public void ActivateHability2()
    {
        if (pointHability2) Hability2Button.color = Color.white;
        if (combatSystem.inspiration > 2)
        {
            if (!pointHability1)
            {
                Hability2Button.color = new Color32(241, 178, 35, 255);
                pointHability2 = true;
                //unitgridcombat llamar ataque especial = true
            }
            else
            {
                Hability2Button.color = Color.white;
                pointHability2 = false;
                //unitgridcombat llamar ataque especial = false
            }
        }
    }

    public void InspirationHability1()
    {
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            combatSystem.doubleSlash = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            combatSystem.boltOfPrecision = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            combatSystem.hexOfNature = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            combatSystem.overload = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            combatSystem.fireBurst = true;
        }
    }



    public void InspirationHability2()
    {
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            combatSystem.justicesExecute = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            combatSystem.windRush = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            combatSystem.divineGrace = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            combatSystem.whirlwind = true;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            combatSystem.summon = true;
        }
    }

    private void DeactivateHabilities()
    {
        combatSystem.feedbackHability = false;
        combatSystem.doubleSlash = false;
        combatSystem.justicesExecute = false;
        combatSystem.boltOfPrecision = false;
        combatSystem.windRush = false;
        combatSystem.hexOfNature = false;
        combatSystem.divineGrace = false;
        combatSystem.whirlwind = false;
        combatSystem.fireBurst = false;
        combatSystem.summon = false;
    }


    public void HidePointsSkip()
    {
        if (alreadyRestedInspiration && !alreadyUsedInspiration)
        {
            inspirationIndexUI = combatSystem.inspiration;
            alreadyRestedInspiration = false;
        }
        pointAttackUI.SetActive(false);
        pointMoveUI.SetActive(false);
        pointHabilitiesUI.SetActive(false);
        pointHability1 = true;
        pointHability2 = true;
        DeactivateHabilities();
        ActivateHability1();
        ActivateHability2();
    }

    //if you are moving or attacking, stop showing the point buttons
    public void HidePointsAction()
    {
        pointAttackUI.SetActive(false);
        pointMoveUI.SetActive(false);
        pointHabilitiesUI.SetActive(false);

    }

    public void InspirationAttack()
    {
        if (pointAttack) AttackButton.color = Color.white;
        if (combatSystem.inspiration > 0)
        {
            if (!pointAttack)
            {
                AttackButton.color = new Color32(241, 178, 35, 255);
                pointAttack = true;
                combatSystem.inspiredAttack = true;
                //Ataque cargado
            }
            else
            {
                AttackButton.color = Color.white;
                pointAttack = false;
                combatSystem.inspiredAttack = false;
            }
        }
    }

    public void InspirationMove()
    {
        if (pointMove) MoveButton.color = Color.white;
        if (combatSystem.inspiration > 0)
        {
            combatSystem.hasUpdatedPositionMove = false;
            if (!pointMove)
            {
                MoveButton.color = new Color32(241, 178, 35, 255);
                pointMove = true;
                //Movimiento cargado
                combatSystem.inspiredMovement = true;
            }
            else
            {
                MoveButton.color = Color.white;
                pointMove = false;
                combatSystem.inspiredMovement = false;
            }
        }
    }

    public void HabilitiesSelectorL1()
    {
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            combatSystem.doubleSlash = true;
            combatSystem.feedbackHability = true;
            combatSystem.SetAttackingTrue();
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
            combatSystem.SetAttackingTrue();
            combatSystem.AttackAllyVisual();
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            combatSystem.overload = true;
            combatSystem.feedbackHability = true;
            combatSystem.alliesTeamList[combatSystem.allyTeamActiveUnitIndex].GetComponent<UnitGridCombat>().isOverloaded = true;
            combatSystem.alliesTeamList[combatSystem.allyTeamActiveUnitIndex].GetComponent<UnitGridCombat>().DoOverloadFeedback();
            combatSystem.inspiration -= 3;
            Hability1UI.GetComponent<Button>().interactable = false;
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            combatSystem.feedbackHability = true;
            combatSystem.fireBurst = true;
            combatSystem.SpawnGridHability();
        }
    }

    public void HabilitiesSelectorL2()
    {
        if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
        }
        else if (combatSystem.unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
        }
    }

    public void ManageInspirationBase()
    {
        if (combatSystem.inspiration > 0 && inspirationIndexUI >= 0)
        {
            if (!alreadyRestedInspiration)
            {
                inspirationIndexUI--;
                alreadyRestedInspiration = true;
            }
            else
            {
                inspirationIndexUI++;
                alreadyRestedInspiration = false;
            }
        }
    }

    public void ManageInspirationHability()
    {
        if (combatSystem.inspiration > 2 && inspirationIndexUI >= 0)
        {
            if (!alreadyRestedInspiration)
            {
                inspirationIndexUI = inspirationIndexUI - 3;
                alreadyRestedInspiration = true;
            }
            else
            {
                inspirationIndexUI = inspirationIndexUI + 3;
                alreadyRestedInspiration = false;
                DeactivateHabilities();
            }
        }
    }
}
