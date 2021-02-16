using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspirationUI : MonoBehaviour
{
    public GameObject pointMoveUI;
    public GameObject pointAttackUI;

    [HideInInspector] public bool pointMove = false;
    [HideInInspector] public bool pointAttack = false;


    public void ShowPointMove()
    {
        pointMoveUI.SetActive(true);
        pointAttackUI.SetActive(false);
        pointMove = true;
        pointAttack = false;
    }
    public void ShowPointAttack()
    {
        pointAttackUI.SetActive(true);
        pointMoveUI.SetActive(false);
        pointAttack = true;
        pointMove = false;
    }
    public void HidePointsSkip()
    {
        pointAttackUI.SetActive(false);
        pointMoveUI.SetActive(false);
        pointMove = false;
        pointAttack = false;
    }

    //if you are moving or attacking, stop showing the point buttons
    public void HidePointsAction()
    {
        pointAttackUI.SetActive(false);
        pointMoveUI.SetActive(false);
    }
}
