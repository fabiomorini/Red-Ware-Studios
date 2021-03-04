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
    [HideInInspector] public bool isPaused = false;
    public GameObject ToolTip;
    public TMP_Text TooltipDescription;
    public TMP_Text TooltipName;
    public Image TooltipImage;
    public GameObject spaceKey;



    private void Start()
    {
        gridCombatSystem = GameObject.FindGameObjectWithTag("CombatHandler").GetComponent<GridCombatSystem>();
        exitButton.interactable = false;
        endText.SetActive(false);
        spaceKey.SetActive(false);
        ToolTip.SetActive(false);
    }

    private void Update()
    {
        if (hasMoved && hasAttacked && hasUsedHability) exitButton.interactable = true;

        if (isPaused && Input.GetKeyDown(KeyCode.Space))
        {
            spaceKey.SetActive(false);
            isPaused = false;
            ToolTip.SetActive(false);
        }
    }

    private IEnumerator ToolTipWaitTime()
    {
        isPaused = true;
        yield return new WaitForSeconds(3f);
        spaceKey.SetActive(true);
    }

    public void MoveTutorialText() 
    {
       if (!hasMoved)
       {
            TooltipDescription.SetText("Accion de Movimiento, Para moverte clicka en una de las casillas dentro del rango de movimiento de tu personaje.");
            TooltipName.SetText("Tutorial - Movimiento");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
        }
    }
    public void AttackTutorialText()
    {
        if (!hasAttacked)
        {
            TooltipDescription.SetText("Accion de Ataque, Para atacar clicka en una de las casillas dentro del rango de movimiento de tu personaje.");
            TooltipName.SetText("Tutorial - Atacar");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
        }
    }
    public void AbilityTutorialText()
    {
        if (!hasUsedHability)
        {
            TooltipDescription.SetText("Accion de Habilidad, Para usar habilidad clicka en una de las casillas dentro del rango de movimiento de tu personaje.");
            TooltipName.SetText("Tutorial - Habilidades");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
        }
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
}
