﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GridCombatSystem : MonoBehaviour {

    //personaje
    [HideInInspector] public UnitGridCombat unitGridCombat; 
    //lista general de personajes
    public List<UnitGridCombat> unitGridCombatArray; 
    //listas de personajes de cada equipo
    [HideInInspector] public List<UnitGridCombat> alliesTeamList; 
    [HideInInspector] public List<UnitGridCombat> alliesTeamKO;
    [HideInInspector] public int allydeads = 0;
    [HideInInspector] public List<UnitGridCombat> newAlliesTeamList;
    [HideInInspector] public List<UnitGridCombat> enemiesTeamList;
    [HideInInspector] public List<UnitGridCombat> enemiesTeamKo;
    [HideInInspector] public List<UnitGridCombat> newEnemiesTeamList;
    //personaje activo del array actual
    [HideInInspector] public int allyTeamActiveUnitIndex;
    [HideInInspector] public int enemiesTeamActiveUnitIndex;

    public GameHandler_GridCombatSystem gameHandler;
    private CHARACTER_MNG characterManager;

    //booleanos de atacar y mover
    [HideInInspector] public bool canMoveThisTurn;
    [HideInInspector] public bool canAttackThisTurn;
    [HideInInspector] public bool hasUpdatedPositionMove = false;
    [HideInInspector] public bool hasUpdatedPositionAttack = false;

    //Sistema de spawning de tropas según cuantas tienes compradas en el cuartel
    //deep lore, se usa solo para que no de error, no sirve para nada más
    private GameObject Ally; 

    public GameObject MeleePrefab;
    public GameObject RangedPrefab;
    public GameObject HealerPrefab;
    public GameObject TankPrefab;
    public GameObject MagePrefab;

    //CHARACTER_MNG
    private int numberOfMelee;
    private int numberOfRanged;
    private int numberOfHealer;
    private int numberOfTank;
    private int numberOfMage;
    private int numberOfAllies;

    [HideInInspector] public int numberOfMeleeLeft = 0;
    [HideInInspector] public int numberOfRangedLeft = 0;
    [HideInInspector] public int numberOfHealerLeft = 0;
    [HideInInspector] public int numberOfTankLeft = 0;
    [HideInInspector] public int numberOfMageLeft = 0;

    // lista paralela a unitGridCombatArray donde comprobamos las características de cada aliado (CHARACTER_MNG)
    private List<CHARACTER_PREFS> characterPrefs; 

    // Sistema de limitación de spawning de tropas por escenas 
    private int maxOfCharacters; 
    //Escenas de Unity por buildIndex
    private int IndexL1 = 3;
    private int IndexL2 = 4;
    private int IndexL3 = 5;
    private int IndexL4 = 6;
    private int IndexL5 = 7;
    private int IndexL6 = 8;
    //MAX personajes por nivel
    private int maxL1 = 3;
    private int maxL2 = 3;
    private int maxL3 = 4;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;

    //-------- IA------------
    public IA_enemies iA_Enemies;
    public MapamundiManager mapamundiManager;
    [HideInInspector] private bool isAllyTurn = true;
    public GameObject allyTurn;
    private bool gameOver;
    //tiempo de espera antes de que se vaya la ui de cambio de turno
    private float SecondsWaitingUI = 1.0f;
    [HideInInspector] public int maxMoveDistance = 3;
    [HideInInspector] public bool inspiredAttack = false;
    [HideInInspector] public bool inspiredMovement = false;

    [HideInInspector] public bool doubleSlash = false;
    [HideInInspector] public bool justicesExecute = false;
    [HideInInspector] public bool boltOfPrecision = false;
    [HideInInspector] public bool windRush = false;
    [HideInInspector] public bool hexOfNature = false;
    [HideInInspector] public bool divineGrace = false;
    [HideInInspector] public bool overload = false;
    [HideInInspector] public bool whirlwind = false;
    [HideInInspector] public bool fireBurst = false;
    [HideInInspector] public bool summon = false;

    //minimenu in-game
    public GameObject Minimenu;
    private bool isMenuVisible;
    [HideInInspector] public bool moving;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool healing;
    [HideInInspector] public bool isWaiting = true;
    public StatisticMenu statisticMenu;
    public Button attackButton;
    public Button moveButton;

    public Button attackButtonTutorial;
    public Button moveButtonTutorial;

    public GameObject healthMenu;
    public GameObject inspirationUI;
    private InspirationUI inspirationManager;

    public TMP_Text hability1Text;
    public TMP_Text hability2Text;

    //EndMenu UI
    public TMP_Text alliesLeftText;
    public TMP_Text totalAlliesLeftText;
    public TMP_Text coinsRewardText;
    public TMP_Text victory;
    public TMP_Text defeat;
    public GameObject allyUI;
    public GameObject enemyUI;
    public GameObject endGameUI;
    private bool surrender;
    public GameObject SurrenderUI;

    public GameObject fireUI;
    public GameObject Center;
    [HideInInspector] public bool isHabilityActive;

    private int experienceKnight;
    private int experienceArcher;
    private int experienceHealer;
    private int experienceTank;
    private int experienceMage;

    public TMP_Text experienceKnightTxt;
    public TMP_Text experienceArcherTxt;
    public TMP_Text experienceHealerTxt;
    public TMP_Text experienceTankTxt;
    public TMP_Text experienceMageTxt;

    private Vector3 fireBurstBox = new Vector3(0, 0, 0);
    [HideInInspector] public bool feedbackHability = false;
    [HideInInspector] public int burstTurns = 0;
    private GameObject temporalFireBurst;
    public GameObject selectedMouse;
    public GameObject selectedFeedback;

    [HideInInspector] public int inspiration;
    public GameObject DamagePopUpPrefab;
    public GameObject inspirationUIGameObject;

    private TutorialManager tutorialManager;
    [HideInInspector] public bool isPaused;

    public Transform arrowRoute;
    public GameObject arrowPrefab;

    private void Start() {
        isPaused = false; 
        selectedFeedback = Instantiate(selectedMouse);
        selectedFeedback.SetActive(false);
        StartCoroutine(YourTurnUI());
        inspirationManager = GameObject.FindGameObjectWithTag("InspirationManager").GetComponent<InspirationUI>();
        characterManager = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>();

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            tutorialManager = GameObject.FindGameObjectWithTag("tutorialManager").GetComponent<TutorialManager>();
        }

        experienceKnight = characterManager.meleeExp;
        experienceArcher = characterManager.archerExp;
        experienceHealer = characterManager.healerExp;
        experienceTank = characterManager.tankExp;
        experienceMage = characterManager.mageExp;


        numberOfMelee = characterManager.numberOfMeleeFight;
        numberOfRanged = characterManager.numberOfArcherFight;
        numberOfHealer = characterManager.numberOfHealerFight;
        numberOfTank = characterManager.numberOfTankFight;
        numberOfMage = characterManager.numberOfMageFight;
        numberOfAllies = numberOfMelee 
                       + numberOfRanged 
                       + numberOfHealer 
                       + numberOfTank 
                       + numberOfMage;
        characterPrefs = characterManager.characterPrefs;
        spawnCharacters();

        alliesTeamList = new List<UnitGridCombat>();
        enemiesTeamList = new List<UnitGridCombat>();
        allyTeamActiveUnitIndex = -1;
        enemiesTeamActiveUnitIndex = -1;

        // Asigna a los personajes en sus posiciones
        foreach (UnitGridCombat unitGridCombat in unitGridCombatArray)
        {
            GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition()).SetUnitGridCombat(unitGridCombat);
            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
            {
                alliesTeamList.Add(unitGridCombat);
            }
            else
            {
                enemiesTeamList.Add(unitGridCombat);
            }
        }
        SelectNextActiveUnit(); 
        inspiration = 1;
    }

    private void Update()
    {
        if (!gameOver && !isPaused)
        {
            gameHandler.HandleCameraMovement();
            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
            {
                inspirationUI.SetActive(true);
                //update del menu de estadísticas
                UpdateStatisticMenu();
                unitGridCombat.setSelectedActive();
                setMenuVisible();

                if (boltOfPrecision && !hasUpdatedPositionAttack)
                {
                    SetAttackingTrue();
                }

                if (fireBurst)
                {
                    FireburstHability();
                }

                if (moving)
                {
                    MoveAllyVisual();
                    if (inspiredMovement)
                    {
                        maxMoveDistance = 6;
                    }
                    else
                    {
                        maxMoveDistance = 4;
                    }
                    if (!hasUpdatedPositionMove)
                    {
                        UpdateValidMovePositions();
                        hasUpdatedPositionMove = true;
                    }
                }
                if (attacking)
                {
                    if(unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
                    {
                        maxMoveDistance = 2;
                    }
                    else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
                    {
                        if (boltOfPrecision && !hasUpdatedPositionAttack)
                        {
                            SpawnGridHability();
                            hasUpdatedPositionAttack = true;
                        }
                        else
                        {
                            maxMoveDistance = 5;
                        }
                    }
                    else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
                    {
                        maxMoveDistance = 4;
                    }
                    else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
                    {
                        maxMoveDistance = 2;
                    }
                    else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
                    {
                        maxMoveDistance = 4;
                    }
                    else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.DUMMY)
                    {
                        maxMoveDistance = 0;
                    }

                    if (!hasUpdatedPositionAttack && !boltOfPrecision)
                    {
                        UpdateValidMovePositions();
                        hasUpdatedPositionAttack = true;
                    }
                    AttackAllyVisual();
                }

                if (!canAttackThisTurn)
                {
                    StartCoroutine(WaitAttack());
                }
            }
            else
            {
                inspiredAttack = false;
                healthMenu.SetActive(false);
                inspirationUI.SetActive(false);
                if (canAttackThisTurn)
                {
                    unitGridCombat.setSelectedActive();
                    //Esperar 1 seg
                    if (isWaiting)
                    {
                        StartCoroutine(WaitForSecondsEnemyTurn());
                        isWaiting = false;
                    }
                }
            }
        }
    }

    private void UpdateStatisticMenu()
    {
        healthMenu.SetActive(true);
        statisticMenu.UpdateHealth(unitGridCombat);
        statisticMenu.UpdateSprite(unitGridCombat);
    }

    private IEnumerator WaitAttack()
    {
        yield return new WaitForSeconds(0.8f);
        CheckTurnOver();
    }

    IEnumerator WaitForSecondsEnemyTurn()
    {
        yield return new WaitForSeconds(1.0f);
        canAttackThisTurn = false;
        canMoveThisTurn = false;

        if (unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.HEALER)
        {
            if (iA_Enemies.CheckAlliesHealth(unitGridCombat) != null)
            {
                if (Vector3.Distance(unitGridCombat.GetPosition(), iA_Enemies.needHealing.GetPosition()) <= unitGridCombat.attackRangeHealer)
                {
                    unitGridCombat.HealAlly(iA_Enemies.needHealing);
                }
                else
                {
                    iA_Enemies.lookForAlliesDist(unitGridCombat);
                }
            }
            else
            {
                if (SeekEnemiesIA(unitGridCombat)) //Player a Rango
                {
                    unitGridCombat.AttackUnit(iA_Enemies.lookForEnemies(unitGridCombat));
                }
                else if (!SeekEnemiesIA(unitGridCombat)) //No Player a Rango
                {
                    iA_Enemies.lookForEnemiesDist(unitGridCombat);
                }
            }
        }
        else 
        {
            if(SeekEnemiesIA(unitGridCombat)) //Player a Rango
            {
                unitGridCombat.AttackUnit(iA_Enemies.lookForEnemies(unitGridCombat));
            }
            else if (!SeekEnemiesIA(unitGridCombat)) //No Player a Rango
            {
                iA_Enemies.lookForEnemiesDist(unitGridCombat);
            }
        }

        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
        MovementTilemap.TilemapObject.TilemapSprite.None);
        yield return new WaitForSeconds(1);
        CheckTurnOver();
    }

    public void spawnCharacters()
    {
        checkMaxCharacters();

        for (int i = 0; i < numberOfAllies; i++) {
            if(numberOfMelee >= 1)
            {
                Ally = Instantiate(MeleePrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                MeleePrefab.name = "Melee" + i;
                numberOfMelee--;
                numberOfMeleeLeft++;

                if (characterManager.meleeLevel == 1)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL1;
                }
                else if (characterManager.meleeLevel == 2)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL2;
                }
                else if (characterManager.meleeLevel == 3)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL3;
                }
            }
            else if( numberOfRanged >= 1)
            {
                Ally = Instantiate(RangedPrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                RangedPrefab.name = "Ranged" + i;
                numberOfRanged--;
                numberOfRangedLeft++;

                if (characterManager.archerLevel == 1)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL1;
                }
                else if (characterManager.archerLevel == 2)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL2;
                }
                else if (characterManager.archerLevel == 3)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL3;
                }
            }
            else if(numberOfHealer >= 1)
            {
                Ally = Instantiate(HealerPrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                HealerPrefab.name = "Healer" + i;
                numberOfHealer--;
                numberOfHealerLeft++;

                if (characterManager.healerLevel == 1)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL1;
                }
                else if (characterManager.healerLevel == 2)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL2;
                }
                else if (characterManager.healerLevel == 3)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL3;
                }
            }
            else if (numberOfTank >= 1)
            {
                Ally = Instantiate(TankPrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                TankPrefab.name = "Tank" + i;
                numberOfTank--;
                numberOfTankLeft++;

                if (characterManager.tankLevel == 1)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL1;
                }
                else if (characterManager.tankLevel == 2)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL2;
                }
                else if (characterManager.tankLevel == 3)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL3;
                }
            }
            else if (numberOfMage >= 1)
            {
                Ally = Instantiate(MagePrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                MagePrefab.name = "Mage" + i;
                numberOfMage--;
                numberOfMageLeft++;

                if (characterManager.mageLevel == 1)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL1;
                }
                else if (characterManager.mageLevel == 2)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL2;
                }
                else if (characterManager.mageLevel == 3)
                {
                    Ally.GetComponent<CHARACTER_PREFS>().level = CHARACTER_PREFS.Level.NIVEL3;
                }
            }
            unitGridCombatArray.Add(Ally.GetComponent<UnitGridCombat>());
        }
    }
    public void CheckIfDead()
    {
        for (int i = 0; i < alliesTeamList.Count; i++)
        {
            if (alliesTeamList[i].GetComponent<UnitGridCombat>().imDead)
            {
                if (alliesTeamList[i].GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
                {
                    characterManager.numberOfMelee--;
                    numberOfMelee--;
                }
                else if (alliesTeamList[i].GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
                {
                    characterManager.numberOfRanged--;
                    numberOfRanged--;
                }
                else if (alliesTeamList[i].GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
                {
                    characterManager.numberOfHealer--;
                    numberOfHealer--;
                }
                else if (alliesTeamList[i].GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
                {
                    characterManager.numberOfTank--;
                    numberOfTank--;
                }
                else if (alliesTeamList[i].GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
                {
                    characterManager.numberOfMage--;
                    numberOfMage--;
                }
            }
        }
    }

    private void checkMaxCharacters()
    {
        if (SceneManager.GetActiveScene().buildIndex == IndexL1)
            maxOfCharacters = maxL1;
        else if (SceneManager.GetActiveScene().buildIndex == IndexL2)
            maxOfCharacters = maxL2;
        else if (SceneManager.GetActiveScene().buildIndex == IndexL3)
            maxOfCharacters = maxL3;
        else if (SceneManager.GetActiveScene().buildIndex == IndexL4)
            maxOfCharacters = maxL4;
        else if (SceneManager.GetActiveScene().buildIndex == IndexL5)
            maxOfCharacters = maxL5;
        else if (SceneManager.GetActiveScene().buildIndex == IndexL6)
            maxOfCharacters = maxL6;

        if (numberOfAllies > maxOfCharacters)
            numberOfAllies = maxOfCharacters;
    }

    public IEnumerator YourTurnUI(){
        allyTurn.SetActive(true);
        yield return new WaitForSeconds(SecondsWaitingUI);
        allyTurn.SetActive(false);
    }

    public void UpdateValidMovePositions()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        //gridPathfinding.RaycastWalkable();

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidMovePositions
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++)
        {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++)
            {
                if (gridPathfinding.IsWalkable(x, y))
                {
                    // Position is Walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x, y))
                    {
                        // There is a Path
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance)
                        {
                            // Path within Move Distance

                            // Set Tilemap Tile to Move
                            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(
                               x, y, MovementTilemap.TilemapObject.TilemapSprite.Move
                            );

                            grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                        }
                        else
                        {
                            // Path outside Move Distance!
                        }
                    }
                    else
                    {
                        // No valid Path
                    }
                }
                else
                {
                    // Position is not Walkable
                }
            }
        }
    }

    public void PrintTilemap()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        grid.GetXY(Center.transform.position, out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        if (!isHabilityActive) 
        { 
            for (int x = unitX - 7; x <= unitX + 7; x++)
            {
                for (int y = unitY - 7; y <= unitY + 7; y++)
                {
                    if (gridPathfinding.IsWalkable(x, y))
                    {
                        // Set Tilemap Tile to Move
                        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(
                            x, y, MovementTilemap.TilemapObject.TilemapSprite.Move
                        );
                    }
                }
            }
        }
    }
    private void ShowMouseCell()
    {
        selectedFeedback.SetActive(true);
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;
        grid.GetXY(GetMouseWorldPosition(), out int unitX, out int unitY);
        if (!gridPathfinding.IsWall(unitX, unitY))
            selectedFeedback.transform.position = LookForCellCenter();
        else
            selectedFeedback.SetActive(false);
    }

    public Vector3 LookForCellCenter()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;
        grid.GetXY(GetMouseWorldPosition(), out int unitX, out int unitY);
        selectedFeedback.SetActive(true);
        Vector3 myPosition = new Vector3(GetMouseWorldPosition().x, GetMouseWorldPosition().y, 0);

        int x = (int)myPosition.x;
        float lastDigitX = Mathf.Abs(myPosition.x) % 10;
        int lastDigitXInt = Mathf.Abs(x) % 10;

        if (lastDigitX >= 9 && lastDigitX < 10) x -= 4;
        else if (lastDigitX >= 0 && lastDigitX < 1) x += 5;
        else
        {
            switch (lastDigitXInt)
            {
                case 8:
                    x -= 3;
                    break;
                case 7:
                    x -= 2;
                    break;
                case 6:
                    x -= 1;
                    break;
                case 5:
                    x -= 0;
                    break;
                case 4:
                    x += 1;
                    break;
                case 3:
                    x += 2;
                    break;
                case 2:
                    x += 3;
                    break;
                case 1:
                    x += 4;
                    break;
            }
        }

        int y = (int)myPosition.y;
        float lastDigitY = Mathf.Abs(myPosition.y) % 10;
        int lastDigitYInt = Mathf.Abs(y) % 10;

        if (lastDigitY >= 9 && lastDigitY < 10) y -= 4;
        else if (lastDigitY >= 0 && lastDigitY < 1) y += 5;
        else
        {
            switch (lastDigitYInt)
            {
                case 8:
                    y -= 3;
                    break;
                case 7:
                    y -= 2;
                    break;
                case 6:
                    y -= 1;
                    break;
                case 5:
                    y -= 0;
                    break;
                case 4:
                    y += 1;
                    break;
                case 3:
                    y += 2;
                    break;
                case 2:
                    y += 3;
                    break;
                case 1:
                    y += 4;
                    break;
            }
        }

        return new Vector3(x, y, 0);
    }


    public void CheckIfGameIsOver(){
        if (enemiesTeamList.Count == 0){
            SoundManager.PlaySound("Victory");
            gameOver = true;
            ShowVictoryUI();
            DontShowUI();
        }
        if(alliesTeamList.Count == 0 || surrender){
            gameOver = true;
            ShowDefeatUI();
            DontShowUI();
        }
    }

    private void ShowEndGameUI()
    {
        inspirationUIGameObject.SetActive(false);
        alliesLeftText.SetText("Allies left: " + (numberOfAllies - allydeads));
        totalAlliesLeftText.SetText("Total allies left: " + (characterManager.numberOfAllies - allydeads));
        endGameUI.SetActive(true);
    }

    private void ShowVictoryUI()
    {
        characterManager.meleeExp += 15 * numberOfMeleeLeft;
        characterManager.archerExp += 15 * numberOfRangedLeft;
        characterManager.healerExp += 15 * numberOfHealerLeft;
        characterManager.tankExp += 15 * numberOfTankLeft;
        characterManager.mageExp += 15 * numberOfMageLeft;

        experienceKnightTxt.SetText("+ " + (characterManager.meleeExp - experienceKnight) + "Exp");
        experienceArcherTxt.SetText("+ " + (characterManager.archerExp - experienceArcher) + "Exp");
        experienceHealerTxt.SetText("+ " + (characterManager.healerExp - experienceHealer) + "Exp");
        experienceTankTxt.SetText("+ " + (characterManager.tankExp - experienceTank) + "Exp");
        experienceMageTxt.SetText("+ " + (characterManager.mageExp - experienceMage) + "Exp");

        characterManager.CheckLevelNumber();
        ShowEndGameUI();
        coinsRewardText.SetText("Reward: " + characterManager.GetLevelIndex() + " coins");
        characterManager.coins += characterManager.GetLevelIndex();
        defeat.gameObject.SetActive(false);
    }
    private void ShowDefeatUI()
    {
        if (surrender)
        {
            coinsRewardText.SetText("Reward: 0 coins");
            victory.gameObject.SetActive(false);
        }
        else
        {
            coinsRewardText.SetText("Reward: " + (int)characterManager.GetLevelIndex() / 2 + " coins");
            characterManager.coins += (int)characterManager.GetLevelIndex() / 2;
            victory.gameObject.SetActive(false);
        }

        ShowEndGameUI();
        experienceKnightTxt.SetText("+ " + (characterManager.meleeExp - experienceKnight) + "Exp");
        experienceArcherTxt.SetText("+ " + (characterManager.archerExp - experienceArcher) + "Exp");
        experienceHealerTxt.SetText("+ " + (characterManager.healerExp - experienceHealer) + "Exp");
        experienceTankTxt.SetText("+ " + (characterManager.tankExp - experienceTank) + "Exp");
        experienceMageTxt.SetText("+ " + (characterManager.mageExp - experienceMage) + "Exp");
    }

    private void DontShowUI()
    {
        Minimenu.SetActive(false);
        allyUI.SetActive(false);
        enemyUI.SetActive(false);
        unitGridCombat.selectedGameObject.SetActive(false);
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
        MovementTilemap.TilemapObject.TilemapSprite.None);
    }

    public void ExitFromBattle()
    {
        SceneManager.LoadScene("Mapamundi Final");
    }

    //mira si hay un enemigo a una casilla
    public bool SeekEnemiesIA(UnitGridCombat thisUnit) 
    {
        Vector3 myPosition = thisUnit.GetPosition();
        for (int i = 0; i < alliesTeamList.Count; i++)
        {
            float distance = Vector3.Distance(myPosition, alliesTeamList[i].GetPosition());
            if (distance <= 11 && (thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE || thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK))
            {
                return true;
            }
            else if (distance <= 31 && (thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE || thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER))
            {
                return true;
            }
            else if (distance <= 41 && (thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED))
            {
                return true;
            }
        }
        return false;
    }

    private void CheckTurnOver() {
        if (!canMoveThisTurn && !canAttackThisTurn) {
            // Si la unidad no puede atacar ni mover, pasará el turno a la siguiente
            ForceTurnOver();
        }
    }

    public void ForceTurnOver()
    {
        unitGridCombat.setSelectedFalse();
        SelectNextActiveUnit();
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
        MovementTilemap.TilemapObject.TilemapSprite.None);
        CheckMinimenuAlly();
        isWaiting = true;
        hasUpdatedPositionMove = false;
        hasUpdatedPositionAttack = false;
        if (SceneManager.GetActiveScene().name == "Tutorial") tutorialManager.SkipTutorialText();
    }

    private void CheckMinimenuAlly() 
    {
        //función para esconder el minimenu si les toca a la IA
        if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
        {
            moveButton.interactable = true;
            attackButton.interactable = true;
            isMenuVisible = true;
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                moveButtonTutorial.interactable = true;
                attackButtonTutorial.interactable = true;
            }
        }
        else
        {
            Minimenu.SetActive(false);
            isMenuVisible = false;
        }
    }

    private void CheckNameHability()
    {
        if(unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            hability1Text.SetText("Double Slash");
            hability2Text.SetText("Justice’s Execute");
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            hability1Text.SetText("Bolt of Precision");
            hability2Text.SetText("Wind Rush");
        }
        else if(unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            hability1Text.SetText("Hex of Nature");
            hability2Text.SetText("Divine Grace");
        }
        else if(unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            hability1Text.SetText("Overload");
            hability2Text.SetText("Whirlwind");
        }
        else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            hability1Text.SetText("Fire Burst");
            hability2Text.SetText("Summon");
        }
    }

    private void SelectNextActiveUnit()
    {
        if (allyTeamActiveUnitIndex + 1 == alliesTeamList.Count && isAllyTurn)
        {
            if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.DUMMY)
                unitGridCombat.curHealth = unitGridCombat.maxHealth;
            isAllyTurn = false;
            allyTeamActiveUnitIndex = -1;
            if (burstTurns < 5)
            {
                burstTurns++;
                CheckFireDamage();
            }
            else
            {
                Destroy(temporalFireBurst);
                fireBurstBox.x = 0;
                fireBurstBox.y = 0;
            }
            selectedFeedback.SetActive(false);
        }
        if (enemiesTeamActiveUnitIndex + 1 == enemiesTeamList.Count && !isAllyTurn)
        {
            if (burstTurns < 5)
            {
                burstTurns++;
                CheckFireDamage();
            }
            else
            {
                Destroy(temporalFireBurst);
                fireBurstBox.x = 0;
                fireBurstBox.y = 0;
            }
            selectedFeedback.SetActive(true);
            if (inspiration < 4) //sumamos uno de inspiración al comienzo del turno
            {
                inspiration++;
            }
            for(int i = 0; i < alliesTeamList.Count; i++) // reset de la habilidad overload cuando acaban los enemigos
            {
                alliesTeamList[i].GetComponent<UnitGridCombat>().isOverloaded = false;
            }
            isAllyTurn = true;
            StartCoroutine(YourTurnUI());
            enemiesTeamActiveUnitIndex = -1;
        }

        unitGridCombat = GetNextActiveUnit();
        CheckNameHability(); // update el nombre de las habilidades
        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;
    }
    public UnitGridCombat GetNextActiveUnit()
    {
        if (isAllyTurn)
        {
            inspirationManager.alreadyRestedInspiration = false;
            inspirationManager.alreadyUsedInspiration = false;
            inspirationManager.inspirationIndexUI = inspiration;

            allyTeamActiveUnitIndex = (allyTeamActiveUnitIndex + 1) % alliesTeamList.Count;
            return alliesTeamList[allyTeamActiveUnitIndex];
        }
        else if (!isAllyTurn)
        {
            enemiesTeamActiveUnitIndex = (enemiesTeamActiveUnitIndex + 1) % enemiesTeamList.Count;
            return enemiesTeamList[enemiesTeamActiveUnitIndex];
        }
        return null;
    }

    private void MoveAllyVisual()
    {
        ShowMouseCell();
        if (Input.GetMouseButtonDown(0))
        {
            Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
            GridObject gridObject = grid.GetGridObject(LookForCellCenter());

            // Check if clicking on a unit position
            if (gridObject.GetUnitGridCombat() == null)
            {
                if (gridObject.GetIsValidMovePosition())
                {
                    // Valid Move Position
                    if (canMoveThisTurn)
                    {
                        inspirationManager.HidePointsAction();
                        moveButton.interactable = false;
                        if (SceneManager.GetActiveScene().name == "Tutorial") moveButtonTutorial.interactable = false;
                        moving = false;
                        canMoveThisTurn = false;
                        // Set entire Tilemap to Invisible
                        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
                            MovementTilemap.TilemapObject.TilemapSprite.None
                        );
                        // Remove Unit from current Grid Object
                        grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                        // Set Unit on target Grid Object

                        if (inspirationManager.alreadyRestedInspiration)
                        {
                            inspiration--;
                            inspirationManager.alreadyUsedInspiration = true;
                        }                        

                        unitGridCombat.MoveTo(LookForCellCenter(), () =>
                        {
                            if (SceneManager.GetActiveScene().name == "Tutorial" && !tutorialManager.hasMoved) tutorialManager.hasMoved = true;
                            gridObject.SetUnitGridCombat(unitGridCombat);
                            GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
                            Minimenu.SetActive(true);
                            selectedFeedback.SetActive(false);
                            CheckTurnOver();
                        });
                    }
                }
            }
        }
    }

    public void SetMovingTrue()
    {
        moving = true;
        attacking = false;
        feedbackHability = false;
        hasUpdatedPositionMove = false;
    }

    public void AttackAllyVisual()
    {
        ShowMouseCell();
        if (Input.GetMouseButtonDown(0))
        {
            selectedFeedback.SetActive(false);
            Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
            GridObject gridObject = grid.GetGridObject(LookForCellCenter());

            // Check if clicking on a unit position
            if (gridObject.GetUnitGridCombat() != null)
            {
                if (boltOfPrecision) // borra el rango de la habilidad de rango infinito
                {
                    SpawnGridHability();
                    inspirationManager.ActivateHability1();
                }
                if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat()))
                {
                    // Clicked on an Enemy of the current unit
                    if (unitGridCombat.CanAttackUnit(gridObject.GetUnitGridCombat()))
                    {
                        // Can Attack Enemy
                        if (canAttackThisTurn)
                        {
                            //Tutorial
                            if (SceneManager.GetActiveScene().name == "Tutorial" && !tutorialManager.hasAttacked)
                            {
                                tutorialManager.hasAttacked = true;
                                tutorialManager.tutorialIndex++;
                            }
                            Minimenu.SetActive(true);
                            canAttackThisTurn = false;
                            if (inspirationManager.alreadyRestedInspiration && !doubleSlash && !boltOfPrecision)
                            {
                                inspiration--;
                                inspirationManager.alreadyUsedInspiration = true;
                            }
                            // Attack Enemy
                            if (doubleSlash)
                            {
                                if (SceneManager.GetActiveScene().name == "Tutorial" && !tutorialManager.hasUsedHability) tutorialManager.hasUsedHability = true;
                                StartCoroutine(DoubleSlash(gridObject));
                                inspiration -= 3;
                            }
                            else
                            {
                                unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat());
                            }

                            if(boltOfPrecision)
                            {
                                if (SceneManager.GetActiveScene().name == "Tutorial" && !tutorialManager.hasUsedHability) tutorialManager.hasUsedHability = true;
                                boltOfPrecision = false;
                                inspiration -= 3;
                            }
                            if (SceneManager.GetActiveScene().name == "Tutorial" && !tutorialManager.hasAttacked) tutorialManager.hasAttacked = true;
                            inspiredAttack = false;
                            inspirationManager.pointAttack = true;
                            inspirationManager.InspirationAttack();
                            inspirationManager.HidePointsAction();
                            attacking = false;
                            if (SceneManager.GetActiveScene().name == "Tutorial")
                            {
                                attackButtonTutorial.interactable = false;
                            }
                            attackButton.interactable = false;
                            inspirationManager.Hability1UI.GetComponent<Button>().interactable = false;
                        }
                    }
                }
                //healer
                if (unitGridCombat.CanHealUnit(gridObject.GetUnitGridCombat()) && unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.HEALER)
                {
                    if(canAttackThisTurn)
                    {
                        if(hexOfNature) inspirationManager.Hability1UI.GetComponent<Button>().interactable = false;
                        Minimenu.SetActive(true);
                        canAttackThisTurn = false;
                        unitGridCombat.HealAlly(gridObject.GetUnitGridCombat());
                        attacking = false;
                        attackButton.interactable = false;
                        if (SceneManager.GetActiveScene().name == "Tutorial") attackButtonTutorial.interactable = false; 
                        //CheckTurnOver();
                    }
                }
            }
        }
    }

    public void DamagePopUp(Vector3 position, int damageAmount)
    {
        DamagePopUpPrefab.GetComponent<TextMeshPro>().SetText(damageAmount.ToString());
        Instantiate(DamagePopUpPrefab, new Vector3 (position.x, position.y + 7, 0), Quaternion.identity);
    }

    public void FireburstHability()
    {
        ShowMouseCell();
        if (Input.GetMouseButtonDown(0))
        {
            selectedFeedback.SetActive(false);
            SpawnGridHability();
            burstTurns = 0;
            inspirationManager.Hability1UI.GetComponent<Button>().interactable = false;
            int x = (int)GetMouseWorldPosition().x;
            int lastDigitX = Mathf.Abs(x) % 10;
            //if()
            switch (lastDigitX)
            {
                case 9:
                    x -= 4;
                    break;
                case 8:
                    x -= 3;
                    break;
                case 7:
                    x -= 2;
                    break;
                case 6:
                    x -= 1;
                    break;
                case 5:
                    x -= 0;
                    break;
                case 4:
                    x += 1;
                    break;
                case 3:
                    x += 2;
                    break;
                case 2:
                    x += 3;
                    break;
                case 1:
                    x += 4;
                    break;
                case 0:
                    x -= 5;
                    break;
            }

            int y = (int)GetMouseWorldPosition().y;
            int lastDigitY = Mathf.Abs(y) % 10;
            switch (lastDigitY)
            {
                case 9:
                    y -= 4;
                    break;
                case 8:
                    y -= 3;
                    break;
                case 7:
                    y -= 2;
                    break;
                case 6:
                    y -= 1;
                    break;
                case 5:
                    y -= 0;
                    break;
                case 4:
                    y += 1;
                    break;
                case 3:
                    y += 2;
                    break;
                case 2:
                    y += 3;
                    break;
                case 1:
                    y += 4;
                    break;
                case 0:
                    y -= 5;
                    break;
            }
            Vector3 position = new Vector3(x, y, 0);

            if (temporalFireBurst != null) // si ya hay un fireburst, destruyelo
            {
                Destroy(temporalFireBurst);
            }

            temporalFireBurst = Instantiate(fireUI, position, Quaternion.identity);
            feedbackHability = true;

            Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
            grid.GetXY(position, out int unitX, out int unitY);
            fireBurstBox = new Vector3(unitX, unitY, 0);

            CheckFireDamage();

            attacking = false;
            attackButton.interactable = false;
            canAttackThisTurn = false;
            if (SceneManager.GetActiveScene().name == "Tutorial") attackButtonTutorial.interactable = true;
            inspiration -= 3;
            fireBurst = false;
        }
        
    }

    public void CheckFireDamage()
    {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        for (int i = 0; i < alliesTeamList.Count; i++)
        {
            grid.GetXY(alliesTeamList[i].GetPosition(), out int unitX, out int unitY);

            if ( unitX == fireBurstBox.x && unitY == fireBurstBox.y)
            {
                alliesTeamList[i].FireDamage();
            }
        }
        for (int i = 0; i < enemiesTeamList.Count; i++)
        {
            grid.GetXY(enemiesTeamList[i].GetPosition(), out int unitX, out int unitY);

            if (unitX == fireBurstBox.x && unitY == fireBurstBox.y)
            {
                enemiesTeamList[i].FireDamage();
            }
        }
    }


    public void SpawnGridHability()
    {
        if (!isHabilityActive)
        {
            PrintTilemap();
        }
        else
        {
            PrintTilemap();
        }
        isHabilityActive = !isHabilityActive;
    }

    private IEnumerator DoubleSlash(GridObject gridObject)
    {
        unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat());
        yield return new WaitForSeconds(0.5f);
        unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat());
        doubleSlash = false;
    }

    public void SetAttackingTrue()
    {
        attacking = true;
        moving = false;
        if(boltOfPrecision || doubleSlash)
        {
            feedbackHability = true;
        }
        else
        {
            feedbackHability = false;
        }
        hasUpdatedPositionAttack = false;
    }
    public void SetHealingTrue()
    {/*
        if (inspirationManager.alreadyRestedInspiration)
        {
            inspirationManager.alreadyUsedInspiration = true;
        }
        */
        healing = true;
        Minimenu.SetActive(false);
    }

    public void SkipTurn()
    {
        attacking = false;
        moving = false;
        ForceTurnOver();
    }

    public void Surrender()
    {
        SurrenderUI.SetActive(true);
    }

    public void YesSurrender()
    {
        SurrenderUI.SetActive(false);
        surrender = true;
        CheckIfGameIsOver();
    }
    public void NoSurrender()
    {
        SurrenderUI.SetActive(false);
    }

    private void setMenuVisible()
    {
        if (isMenuVisible)
        {
            Minimenu.SetActive(true);
            isMenuVisible = false;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (Minimenu.activeInHierarchy)
                Minimenu.SetActive(false);
            else
                Minimenu.SetActive(true);
        }
    }

    // El eje Z siempre tiene que ser 0
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }
    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    public class GridObject {

        private Grid<GridObject> grid;
        private int x;
        private int y;
        private bool isValidMovePosition;
        private UnitGridCombat unitGridCombat;

        public GridObject(Grid<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetIsValidMovePosition(bool set) {
            isValidMovePosition = set;
        }

        public bool GetIsValidMovePosition() {
            return isValidMovePosition;
        }

        public void SetUnitGridCombat(UnitGridCombat unitGridCombat) {
            this.unitGridCombat = unitGridCombat;
        }

        public void ClearUnitGridCombat() {
            SetUnitGridCombat(null);
        }

        public UnitGridCombat GetUnitGridCombat() {
            return unitGridCombat;
        }

    }
}
