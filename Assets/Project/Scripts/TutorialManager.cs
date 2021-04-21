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
    [HideInInspector] public bool hasSkip = false;
    [HideInInspector] public bool hasExit = false;

    public Button exitButton;
    public GameObject endText;
    [HideInInspector] public int tutorialIndex = 0;
    public GameObject ToolTip;
    public TMP_Text TooltipDescription;
    public TMP_Text TooltipName;
    public GameObject NextTooltipButton;
    public GameObject SurrenderCheck;

    public Button moveButton;
    public Button attackButton;
    public Button habilityButton;
    public Button skipButton;
    public Button InspirationAbility;

    public GameObject inspirationMoveButton;
    public GameObject inspirationAttackButton;

    public GameObject arrowMinimenu;
    public GameObject arrowUI;
    public GameObject arrowInspiration;
    public GameObject arrowMove;

    public TMP_Text welcomeText;
    private bool tutorial1 = true;
    private bool isWaiting1 = false;
    private bool isWaiting2 = false;
    private bool isWaiting3 = false;
    private bool isWaiting4 = false;
    public GameObject pressSpaceButton;
    private int firstPartIndex = 0;
    public GameObject firstTutorial;
    public GameObject yourTurnUI;

    //Gifs
    public Texture2D[] framesMove; 
    public Texture2D[] framesAttack; 
    public Texture2D[] framesAbility; 
    public Texture2D[] framesSkip; 
    public Texture2D[] framesExit; 
    public int fps = 10;
    public RawImage gif;

    private bool gifMove = false;
    private bool gifAttack = false;
    private bool gifAbility = false;
    private bool gifSkip = false;
    private bool gifExit = false;

    private void Start()
    {
        gridCombatSystem = GameObject.FindGameObjectWithTag("CombatHandler").GetComponent<GridCombatSystem>();
        StartFirstPartTutorial();
    }

    private void Update()
    {
        if (!tutorial1) // segunda parte del tutorial
        {
            if (gifMove) GifMove();
            else if (gifAttack) GifAttack();
            else if (gifSkip) GifSkip();
            else if (gifAbility) GifAbility();
            else if (gifExit) GifSurrender();

            UnlockButtons();
            if (hasMoved && hasAttacked && hasUsedHability) exitButton.interactable = true;
            if (hasUsedHability) InspirationAbility.interactable = true;
            if (gridCombatSystem.canMoveThisTurn) moveButton.interactable = true;
            if (gridCombatSystem.canAttackThisTurn) attackButton.interactable = true;
            skipButton.interactable = true;
            if (gridCombatSystem.inspiration >= 3) habilityButton.interactable = true; InspirationAbility.interactable = true;
        }
        else //primera parte del tutorial
        {
            UpdateFirstPartTutorial();
        }
    }

    public void OkTooltip()
    {
        NextTooltipButton.SetActive(false);
        gridCombatSystem.isPaused = false;
        ToolTip.SetActive(false);
        if(hasExit) SurrenderCheck.SetActive(true);
    }


    public void NextButton()
    {

        if(firstPartIndex == 3)
        {
            tutorial1 = false;
            gridCombatSystem.isPaused = false;
            firstTutorial.SetActive(false);
            inspirationMoveButton.SetActive(false);
            inspirationAttackButton.SetActive(false);
            arrowMove.SetActive(true);
            moveButton.interactable = true;
        }
        else
        {
            firstPartIndex++;
        }
    }

    private IEnumerator WelcomeWaitTime()
    {
        yield return new WaitForSeconds(1f);
        pressSpaceButton.SetActive(true);
    }

    private void UpdateFirstPartTutorial()
    {
        if(firstPartIndex == 0) // welcome
        {
            if (!isWaiting1)
            {
                pressSpaceButton.SetActive(false);
                welcomeText.SetText("Welcome to the Battle Tale tutorial! \n\n Now you will learn to use the different elements that will be shown on the battlefield");
                isWaiting1 = true;
                StartCoroutine(WelcomeWaitTime());
            }
        }
        else if (firstPartIndex == 1) // minimenu
        {
            if (!isWaiting2)
            {
                pressSpaceButton.SetActive(false);
                arrowMinimenu.SetActive(true);
                welcomeText.SetText("In the Actions Menu you will be able to use the different commands that your units can perform in combat");
                isWaiting2 = true;
                StartCoroutine(WelcomeWaitTime());
            }
        }
        else if (firstPartIndex == 2) // UI
        {
            if (!isWaiting3)
            {
                pressSpaceButton.SetActive(false);
                welcomeText.SetText("This is the Unit Stats Menu, here you can see the attributes corresponding to the current unit.");
                arrowMinimenu.SetActive(false);
                arrowUI.SetActive(true);
                isWaiting3 = true;
                StartCoroutine(WelcomeWaitTime());
            }
        }
        else if (firstPartIndex == 3) // Inspiration
        {
            if (!isWaiting4)
            {
                pressSpaceButton.SetActive(false);
                welcomeText.SetText("In the top you have your inspiration points. \n You gain one point each turn and you can use them to enhance your actions.");
                inspirationMoveButton.SetActive(true);
                inspirationAttackButton.SetActive(true);
                arrowUI.SetActive(false);
                arrowInspiration.SetActive(true);
                isWaiting4 = true;
                StartCoroutine(WelcomeWaitTime());
                yourTurnUI.GetComponent<TMP_Text>().SetText("Your Turn");
            }
        }
    }

    private void StartFirstPartTutorial()
    {
        endText.SetActive(false);
        NextTooltipButton.SetActive(false);
        ToolTip.SetActive(false);
        yourTurnUI.GetComponent<TMP_Text>().SetText("");

        tutorial1 = true;
        gridCombatSystem.isPaused = true;

        firstTutorial.SetActive(true);
        pressSpaceButton.SetActive(false);
        inspirationMoveButton.SetActive(false);
        inspirationAttackButton.SetActive(false);
        arrowMinimenu.SetActive(false);
        arrowUI.SetActive(false);
        arrowInspiration.SetActive(false);
        arrowMove.SetActive(false);

        moveButton.interactable = false;
        attackButton.interactable = false;
        skipButton.interactable = false;
        habilityButton.interactable = false;
        exitButton.interactable = false;

        hasUsedHability = false;
        hasAttacked = false;
        hasMoved = false;
    }

    private void UnlockButtons()
    {
        if (!hasMoved)
        {
            attackButton.interactable = false;
            skipButton.interactable = false;
        }
        else if (hasMoved && gridCombatSystem.canAttackThisTurn)
        {
            attackButton.interactable = true;
        }
        if (hasMoved)
        {
            skipButton.interactable = true;
        }
        if (gridCombatSystem.inspiration < 3 || !gridCombatSystem.canAttackThisTurn)
        {
            habilityButton.interactable = false;
        }
        else if (gridCombatSystem.inspiration >= 3 && gridCombatSystem.canAttackThisTurn)
        {
            habilityButton.interactable = true;
        }
    }

    private IEnumerator ToolTipWaitTime()
    {
        gridCombatSystem.isPaused = true;
        moveButton.interactable = false;
        attackButton.interactable = false;
        skipButton.interactable = false;
        habilityButton.interactable = false;
        yield return new WaitForSeconds(1f);
        NextTooltipButton.SetActive(true);
    }

    public void MoveTutorialText() 
    {
       gifMove = true;
       gifAttack = false;
       gifAbility = false;
       gifSkip = false;
       gifExit = false;
       if (!hasMoved)
       {
            arrowMove.SetActive(false);
            TooltipDescription.SetText("The unit will advance to the desired position within its range. \n\nIf you use its inspiration button, the unit will advance more squares");
            TooltipName.SetText("Action: \nMove");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
       }
    }
    public void AttackTutorialText()
    {
        gifAttack = true;
        gifMove = false;
        gifAbility = false;
        gifSkip = false;
        gifExit = false;
        if (!hasAttacked)
        {
            TooltipDescription.SetText("Your unit will inflict damage to the selected enemy within its range \n\nIf you use its inspiration button, the attack will be more powerfull");
            TooltipName.SetText("Action: Attack");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
        }
    }
    public void AbilityTutorialText()
    {
        InspirationAbility.interactable = false;
        gifAbility = true;
        gifAttack = false;
        gifMove = false;
        gifSkip = false;
        gifExit = false;
        if (!hasUsedHability)
        {
            TooltipDescription.SetText("It's a special skill that costs three inspiration points \n\nEvery class has two Special Skills");
            TooltipName.SetText("Action: Ability");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
        }
    }

    public void SkipTutorialText()
    {
        gifSkip = true;
        gifAbility = false;
        gifAttack = false;
        gifMove = false;
        gifExit = false;
        if (!hasSkip)
        {
            TooltipDescription.SetText("If you dont want to execute any more action with the current unit, you can use Skip to pass turn. \n\nYou will skip turn if you already moved and attacked or used an hability");
            TooltipName.SetText("Action: Skip Turn");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());
        }
        hasSkip = true;
    }

    public void ExitTutorialText()
    {
        gifExit = true;
        gifSkip = false;
        gifAbility = false;
        gifAttack = false;
        gifMove = false;
        if (!hasExit)
        {
            TooltipDescription.SetText("If you want to get out of a real combat for any reason, you can use Surrender and get back to the world map. \nYou will not get any experience or money.");
            TooltipName.SetText("Action: Surrender");
            ToolTip.SetActive(true);
            StartCoroutine(ToolTipWaitTime());

        }
        hasExit = true;
    }

    private void GifMove()
    {
        int index = (int)(Time.time * fps) % framesMove.Length;
        gif.texture = framesMove[index];
    }

    private void GifAttack()
    {
        int index = (int)(Time.time * fps) % framesAttack.Length;
        gif.texture = framesAttack[index];
    }

    private void GifAbility()
    {
        int index = (int)(Time.time * fps) % framesAbility.Length;
        gif.texture = framesAbility[index];
    }

    private void GifSkip()
    {
        int index = (int)(Time.time * fps) % framesSkip.Length;
        gif.texture = framesSkip[index];
    }

    private void GifSurrender()
    {
        int index = (int)(Time.time * fps) % framesExit.Length;
        gif.texture = framesExit[index];
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
        SceneManager.LoadScene("Mapamundi Final");
    }

    public void DontExit()
    {
        endText.SetActive(false);
    }
}
