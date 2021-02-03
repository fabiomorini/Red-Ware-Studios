using System.Collections;
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
    private bool canMoveThisTurn;
    private bool canAttackThisTurn;

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
    // lista paralela a unitGridCombatArray donde comprobamos las características de cada aliado (CHARACTER_MNG)
    private List<CHARACTER_PREFS> characterPrefs; 

    // Sistema de limitación de spawning de tropas por escenas 
    private int maxOfCharacters; 
    //Escenas de Unity por buildIndex
    private int IndexL1 = 2;
    private int IndexL2 = 3;
    private int IndexL3 = 4;
    private int IndexL4 = 5;
    private int IndexL5 = 6;
    private int IndexL6 = 7;
    //MAX personajes por nivel
    private int maxL1 = 3;
    private int maxL2 = 4;
    private int maxL3 = 5;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;

    //-------- IA------------
    public IA_enemies iA_Enemies;
    [HideInInspector] private bool isAllyTurn = true;
    public GameObject allyTurn;
    private bool gameOver;
    //tiempo de espera antes de que se vaya la ui de cambio de turno
    private float SecondsWaitingUI = 1.0f;
    [HideInInspector] public int maxMoveDistance = 5;

    //minimenu in-game
    public GameObject Minimenu;
    private bool isMenuVisible;
    private bool isMoving;
    [HideInInspector] public bool moving;
    [HideInInspector] public bool attacking;
    [HideInInspector] public bool healing;
    [HideInInspector] public bool isWaiting = true;
    public StatisticMenu statisticMenu;
    public Button attackButton;
    public Button moveButton;

    public GameObject healthMenu;

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


    private void Start() {
        characterManager = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>();
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
        StartCoroutine(YourTurnUI());
    }

    private void Update()
    {
        if (!gameOver)
        {
            gameHandler.HandleCameraMovement();
            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
            {
                healthMenu.SetActive(true);
                //update del menu de estadísticas
                UpdateStatisticMenu();
                unitGridCombat.setSelectedActive();
                setMenuVisible();
                if (moving)
                {
                    maxMoveDistance = 5;
                    UpdateValidMovePositions();
                    MoveAllyVisual();
                }
                if (attacking)
                {
                    if(unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
                    {
                        maxMoveDistance = 2;
                    }
                    else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
                    {
                        maxMoveDistance = 5;
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

                    UpdateValidMovePositions();
                    AttackAllyVisual();
                }

                if (!canAttackThisTurn)
                {
                    StartCoroutine(WaitAttack());
                }
            }
            else
            {
                healthMenu.SetActive(false);
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
        canMoveThisTurn = false; // temporal
                                 // Attack Enemy
        if (SeekEnemiesIA(unitGridCombat) == true)
        {
            unitGridCombat.AttackUnit(iA_Enemies.lookForEnemies(unitGridCombat));
        }
        else
        {
            iA_Enemies.lookForEnemiesDist(unitGridCombat);
            UpdateValidMovePositions();
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
            //temporal, este metodo siempre da la preferencia a los melees
            if(numberOfMelee >= 1)
            {
                Ally = Instantiate(MeleePrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                MeleePrefab.name = "Melee" + i;
                numberOfMelee--;
            }
            else if( numberOfRanged >= 1)
            {
                Ally = Instantiate(RangedPrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                RangedPrefab.name = "Ranged" + i;
                numberOfRanged--;
            }
            else if(numberOfHealer >= 1)
            {
                Ally = Instantiate(HealerPrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                HealerPrefab.name = "Healer" + i;
                numberOfHealer--;
            }
            else if (numberOfTank >= 1)
            {
                Ally = Instantiate(TankPrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                TankPrefab.name = "Tank" + i;
                numberOfTank--;
            }
            else if (numberOfMage >= 1)
            {
                Ally = Instantiate(MagePrefab, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
                MagePrefab.name = "Mage" + i;
                numberOfMage--;
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
        if(isAllyTurn) allyTurn.SetActive(true);
        yield return new WaitForSeconds(SecondsWaitingUI);
        allyTurn.SetActive(false);
    }

    public void UpdateValidMovePositions() {
        Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidMovePositions
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++) {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++) {
                if (gridPathfinding.IsWalkable(x, y)) {
                    // Position is Walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                        // There is a Path
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance) {
                            // Path within Move Distance

                            // Set Tilemap Tile to Move
                            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetTilemapSprite(
                                x, y, MovementTilemap.TilemapObject.TilemapSprite.Move
                            );

                            grid.GetGridObject(x, y).SetIsValidMovePosition(true);
                        } else { 
                            // Path outside Move Distance!
                        }
                    } else {
                        // No valid Path
                    }
                } else {
                    // Position is not Walkable
                }
            }
        }
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
        alliesLeftText.SetText("Allies left: " + (numberOfAllies - allydeads));
        totalAlliesLeftText.SetText("Total allies left: " + (characterManager.numberOfAllies - allydeads));
        endGameUI.SetActive(true);
    }

    private void ShowVictoryUI()
    {
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
            ShowEndGameUI();
            coinsRewardText.SetText("Reward: 0 coins");
            victory.gameObject.SetActive(false);
        }
        else
        {
            ShowEndGameUI();
            coinsRewardText.SetText("Reward: " + (int)characterManager.GetLevelIndex() / 2 + " coins");
            characterManager.coins += (int)characterManager.GetLevelIndex() / 2;
            victory.gameObject.SetActive(false);
        }
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
        SceneManager.LoadScene("Mapamundi");
    }

    //mira si hay un enemigo a una casilla
    public bool SeekEnemiesIA(UnitGridCombat thisUnit) 
    {
        Vector3 myPosition = thisUnit.GetPosition();
        for (int i = 0; i < alliesTeamList.Count; i++)
        {
            float distance = Vector3.Distance(myPosition, alliesTeamList[i].GetPosition());
            if (distance <= 10)
                return true;
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
            iA_Enemies.ResetPositions();
            SelectNextActiveUnit();
            UpdateValidMovePositions();
            GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None);
            CheckMinimenuAlly();
            isWaiting = true;
    }

    private void CheckMinimenuAlly() 
    {
        //función para esconder el minimenu si les toca a la IA
        if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
        {
            moveButton.interactable = true;
            attackButton.interactable = true;
            isMenuVisible = true;
        }
        else
        {
            Minimenu.SetActive(false);
            isMenuVisible = false;
        }
    }

    private void SelectNextActiveUnit()
    {
        if (allyTeamActiveUnitIndex + 1 == alliesTeamList.Count && isAllyTurn)
        {
            isAllyTurn = false;
            allyTeamActiveUnitIndex = -1;
        }
        if (enemiesTeamActiveUnitIndex + 1 == enemiesTeamList.Count && !isAllyTurn)
        {
            YourTurnUI();
            isAllyTurn = true;
            enemiesTeamActiveUnitIndex = -1;
        }

        unitGridCombat = GetNextActiveUnit();
        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;
    }
    public UnitGridCombat GetNextActiveUnit()
    {
        if (isAllyTurn)
        {
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
        if (Input.GetMouseButtonDown(0))
        {
            Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
            GridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());

            // Check if clicking on a unit position
            if (gridObject.GetUnitGridCombat() == null)
            {
                if (gridObject.GetIsValidMovePosition())
                {
                    // Valid Move Position
                    if (canMoveThisTurn)
                    {
                        moveButton.interactable = false;
                        moving = false;
                        canMoveThisTurn = false;
                        // Set entire Tilemap to Invisible
                        GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
                            MovementTilemap.TilemapObject.TilemapSprite.None
                        );

                        // Remove Unit from current Grid Object
                        grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                        // Set Unit on target Grid Object
                        gridObject.SetUnitGridCombat(unitGridCombat);

                        unitGridCombat.MoveTo(GetMouseWorldPosition(), () =>
                        {
                            GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
                            Minimenu.SetActive(true);
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
        Minimenu.SetActive(false);
    }

    public void AttackAllyVisual()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
            GridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());

            // Check if clicking on a unit position
            if (gridObject.GetUnitGridCombat() != null)
            {
                if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat()))
                {
                    // Clicked on an Enemy of the current unit
                    if (unitGridCombat.CanAttackUnit(gridObject.GetUnitGridCombat()))
                    {
                        // Can Attack Enemy
                        if (canAttackThisTurn)
                        {
                            Minimenu.SetActive(true);
                            canAttackThisTurn = false;
                            // Attack Enemy
                            unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat());
                            attacking = false;
                            attackButton.interactable = false;
                        }
                    }
                }
                //healer
                if (unitGridCombat.CanHealUnit(gridObject.GetUnitGridCombat()) && unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.HEALER)
                {
                    if(canAttackThisTurn)
                    {
                        Minimenu.SetActive(true);
                        canAttackThisTurn = false;
                        unitGridCombat.HealAlly(gridObject.GetUnitGridCombat());
                        attacking = false;
                        attackButton.interactable = false;
                        //CheckTurnOver();
                    }
                }
            }
        }
    }
    public void SetAttackingTrue()
    {
        attacking = true;
        moving = false;
        Minimenu.SetActive(false);
    }
    public void SetHealingTrue()
    {
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
        //función para mostrar el minimenu si 
        //le das a la tecla "M"
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
