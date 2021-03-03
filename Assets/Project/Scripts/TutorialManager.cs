using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [HideInInspector] public GridCombatSystem gridCombatSystem;
    private bool hasMoved = false;
    private bool hasAttacked = false;
    private bool hasUsedHability = false;
    public Button exitButton;
    public GameObject endText;


    private void Start()
    {
        gridCombatSystem = GameObject.FindGameObjectWithTag("CombatHandler").GetComponent<GridCombatSystem>();
        exitButton.interactable = false;
        endText.SetActive(false);
    }

    private void Update()
    {
        if (hasMoved && hasAttacked && hasUsedHability) exitButton.interactable = true;
    }

    public void SetMoveBool()
    {
        hasMoved = true;
    }
    public void SetAttackBool()
    {
        hasAttacked = true;
    }
    public void SethabilityBool()
    {
        hasUsedHability = true;
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
