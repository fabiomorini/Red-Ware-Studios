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
}
