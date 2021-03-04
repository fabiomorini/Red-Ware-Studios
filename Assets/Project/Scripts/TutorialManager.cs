using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [HideInInspector] public GridCombatSystem gridCombatSystem;
    [HideInInspector] public bool hasMoved = false;
    [HideInInspector] public bool hasAttacked = false;
    [HideInInspector] public bool hasUsedHability = false;
    public Button exitButton;
    public GameObject endText;
    [HideInInspector] public int tutorialIndex = 0;
    public GameObject ToolTip;
    public TMP_Text TooltipDescription;
    public TMP_Text TooltipName;
    public Image TooltipImage;
    private bool SkipTooltip = false;


    private void Start()
    {
        gridCombatSystem = GameObject.FindGameObjectWithTag("CombatHandler").GetComponent<GridCombatSystem>();
        exitButton.interactable = false;
        endText.SetActive(false);
    }

    private void Update()
    {
        if (hasMoved && hasAttacked && hasUsedHability) exitButton.interactable = true;

        if (hasMoved)
        {
            ToolTip.SetActive(false);
            TooltipDescription.SetText("Accion de Pasar, Para pasar clicka en el boton de pasar");
            TooltipName.SetText("Tutorial - Skip");
            ToolTip.SetActive(true);
        }
    }

    private IEnumerator ToolTipWaitTime()
    {
        Time.timeScale = 0;
        yield return new WaitForSeconds(3);
        if (SkipTooltip)
        {
            ToolTip.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void MoveTutorialText()
    {
       if (!hasMoved)
       {
            TooltipDescription.SetText("Accion de Movimiento, Para moverte clicka en una de las casillas dentro del rango de movimiento de tu personaje.");
            TooltipName.SetText("Tutorial - Movimiento");
            ToolTip.SetActive(true);
       }
       else
       {

            //Disable Tooltip Move
            //Show Tooltip Skip
        }
    }
    public void AttackTutorialText()
    {
        if (!hasAttacked)
        {
            //Show Tooltip
        }
        else
        {
            //Disable Tooltip
        }
    }
    public void AbilityTutorialText()
    {/*
       if (InspirationUI >= 3)
       {
            if (!hasUsedHability)
            {
                //Show Tooltip
            }
            else
            {
                //Disable Tooltip
            }
       }*/
    }

    public void ShowExitText()
    {
        endText.SetActive(true);
    }

    public void ShowEndText()
    {
        endText.SetActive(true);
    }

    public void Exit()
    {
        SceneManager.LoadScene("Mapamundi");
    }

    public void DontExit()
    {
        endText.SetActive(false);
    }

    private void ShowTooltips()
    {
        if (tutorialIndex == 0)
        {

        }
        if (tutorialIndex == 1)
        {

        }
        if (tutorialIndex == 2)
        {

        }
    }
}
