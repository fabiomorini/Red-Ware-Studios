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

    public GameObject Inspiration1;
    public GameObject Inspiration2;
    public GameObject Inspiration3;
    public GameObject Inspiration4;

    [HideInInspector] public int inspirationIndexUI;

    [HideInInspector] public bool pointMove = false;
    [HideInInspector] public bool pointAttack = false;
    [HideInInspector] public bool alreadyRestedInspiration = false;
    [HideInInspector] public bool alreadyUsedInspiration = false;

    public GridCombatSystem combatSystem;

    private void Start()
    {
        alreadyRestedInspiration = false;
        alreadyUsedInspiration = false;
        inspirationIndexUI = 1;
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
        Debug.Log(combatSystem.inspiration);
        Debug.Log(inspirationIndexUI + "index");
        Debug.Log(alreadyRestedInspiration);
        pointMoveUI.SetActive(true);
        pointAttackUI.SetActive(false);
        pointAttack = true;
        pointMove = true;
        combatSystem.inspiredAttack = false;
        combatSystem.inspiredMovement = false;
        if (alreadyRestedInspiration && !alreadyUsedInspiration)
        {
            inspirationIndexUI = combatSystem.inspiration;
            alreadyRestedInspiration = false;
        }
        InspirationMove();
        InspirationAttack();
    }
    public void ShowPointAttack()
    {
        pointAttackUI.SetActive(true);
        pointMoveUI.SetActive(false);
        pointAttack = true;
        pointMove = true;
        combatSystem.inspiredAttack = false;
        combatSystem.inspiredMovement = false;
        if (alreadyRestedInspiration && !alreadyUsedInspiration)
        {
            inspirationIndexUI = combatSystem.inspiration;
            alreadyRestedInspiration = false;
        }
        InspirationAttack();
        InspirationMove();
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
    }

    //if you are moving or attacking, stop showing the point buttons
    public void HidePointsAction()
    {
        pointAttackUI.SetActive(false);
        pointMoveUI.SetActive(false);
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

    public void ManageInspiration()
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
}
