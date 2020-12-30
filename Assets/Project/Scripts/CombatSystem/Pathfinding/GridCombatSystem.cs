using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;
using UnityEngine.SceneManagement;

public class GridCombatSystem : MonoBehaviour {

    public List<UnitGridCombat> unitGridCombatArray;
    private State state;
    private UnitGridCombat unitGridCombat;
    /////////////////////////////////////////////////////////////////
    ///listas de personajes de cada equipo
    [HideInInspector]
    public List<UnitGridCombat> blueTeamList;
    [HideInInspector]
    public List<UnitGridCombat> blueTeamKO;
    [HideInInspector]
    public List<UnitGridCombat> newBlueTeamList;
    [HideInInspector]
    public List<UnitGridCombat> redTeamList;
    [HideInInspector]
    public List<UnitGridCombat> redTeamKO;
    [HideInInspector]
    public List<UnitGridCombat> newRedTeamList;
    /////////////////////////////////////////////////////////////////
    [HideInInspector]
    public int blueTeamActiveUnitIndex;
    [HideInInspector]
    public int redTeamActiveUnitIndex;
    private bool canMoveThisTurn;
    private bool canAttackThisTurn;

    /////////////////////////////////////////////////////////////////
    ///Sistema de spawning de tropas según cuantas tienes compradas en el cuartel
    private GameObject hola; // deep lore, se usa solo para que no de error, no sirve para nada
    public GameObject Ally; /// Se tiene que cambiar por los game objects de cada tipo de soldado
    private int numberOfAllies; //lo inicializa según el character Manager
    private List<CHARACTER_PREFS> characterPrefs; // lista paralela a unitGridCombatArray donde comprobamos las características de cada aliado (CHARACTER_MNG)

    /////////////////////////////////////////////////////////////////
    /// Sistema de limitación de spawning de tropas por escenas 
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
    private int maxL3 = 4;
    private int maxL4 = 5;
    private int maxL5 = 6;
    private int maxL6 = 7;
    /////////////////////////////////////////////////////////////////
    /// IA
    [HideInInspector]
    private bool canAttackIA = true;
    public IA_enemies iA_Enemies;
    /////////////////////////////////////////////////////////////////

    private bool isBlueTurn = true;
    [HideInInspector]
    public int BlueIndex = 0;
    private int RedIndex = 0;
    public int CurrentAliveBlue;
    public int CurrentAliveRed;
    public GameObject BlueTurn;
    public GameObject RedTurn;
    public GameObject GameOverUI;
    private bool TextShow;
    private bool GameOver;
    private float SecondsWaitingUI = 1.0f;

    public int maxMoveDistance = 5;
    private enum State {
        Normal,
        Waiting
    }

    private void Awake() {
        state = State.Normal;
    }

    private void Start() {
        numberOfAllies = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>().numAllies();
        characterPrefs = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>().characterPrefs;
        spawnCharacters();
        blueTeamList = new List<UnitGridCombat>();
        redTeamList = new List<UnitGridCombat>();
        blueTeamActiveUnitIndex = -1;
        redTeamActiveUnitIndex = -1;

        // Set all UnitGridCombat on their GridPosition
        foreach (UnitGridCombat unitGridCombat in unitGridCombatArray)
        {
            GameHandler_GridCombatSystem.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition()).SetUnitGridCombat(unitGridCombat);
            if (unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue)
            {
                blueTeamList.Add(unitGridCombat);
                BlueIndex++;
            }
            else
            {
                redTeamList.Add(unitGridCombat);
                RedIndex++;
            }
        }

        CurrentAliveBlue = BlueIndex;
        CurrentAliveRed = RedIndex;


        SelectNextActiveUnit();
        UpdateValidMovePositions();
        StartCoroutine(TurnSwap());
    }
    public void spawnCharacters()
    {
        for (int i = 0; i < numberOfAllies; i++)
        {
            // leer de la lista los playerprefs y añadirlos a los characters
            hola = Instantiate(Ally, this.gameObject.transform.GetChild(i).position, Quaternion.identity);
            Ally.name = "Ally" + i;
            unitGridCombatArray.Add(hola.GetComponent<UnitGridCombat>());
            characterPrefs.Add(hola.GetComponent<CHARACTER_PREFS>());
        }
    }
    private void checkMaxCharacters()
    {
        if (SceneManager.GetActiveScene().buildIndex == IndexL1)
        {
            maxOfCharacters = maxL1;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL2)
        {
            maxOfCharacters = maxL2;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL3)
        {
            maxOfCharacters = maxL3;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL4)
        {
            maxOfCharacters = maxL4;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL5)
        {
            maxOfCharacters = maxL5;
        }
        else if (SceneManager.GetActiveScene().buildIndex == IndexL6)
        {
            maxOfCharacters = maxL6;
        }

        if (numberOfAllies > maxOfCharacters)
        {
            numberOfAllies = maxOfCharacters;
        }
    }
    public void SelectNextActiveUnit(){
        if(unitGridCombat == null || isBlueTurn){
            if(TextShow){
                StartCoroutine(TurnSwap());
                TextShow = false;               
            }
            unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Blue);
            if(blueTeamActiveUnitIndex == BlueIndex -1){
                isBlueTurn = false;
                TextShow = true;
            }
        }
        else{
            if(TextShow){
                StartCoroutine(TurnSwap());
                TextShow = false;
            }
         unitGridCombat = GetNextActiveUnit(UnitGridCombat.Team.Red);
         if(redTeamActiveUnitIndex == RedIndex -1){
                isBlueTurn = true;
                TextShow = true;
            }
        }
        GameHandler_GridCombatSystem.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;
    }

    public IEnumerator TurnSwap(){
        if(isBlueTurn){
            BlueTurn.SetActive(true);
        }
        else RedTurn.SetActive(true);

        yield return new WaitForSeconds(SecondsWaitingUI);
        RedTurn.SetActive(false);
        BlueTurn.SetActive(false);
    }

    public UnitGridCombat GetNextActiveUnit(UnitGridCombat.Team team) {
        //Comprobamos si no hay más jugadores de cada equipo
        if (team == UnitGridCombat.Team.Blue) {
            blueTeamActiveUnitIndex = (blueTeamActiveUnitIndex + 1) % blueTeamList.Count;
            if (blueTeamList[blueTeamActiveUnitIndex] == null ) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return blueTeamList[blueTeamActiveUnitIndex];
            }
        } else {
            redTeamActiveUnitIndex = (redTeamActiveUnitIndex + 1) % redTeamList.Count;
            if (redTeamList[redTeamActiveUnitIndex] == null ) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return redTeamList[redTeamActiveUnitIndex];
            }
        }
    }

    private void RestartGame(){
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
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

    private void CheckNumberPlayers(){
        if(CurrentAliveRed == 0){
            GameOver = true;  
            RestartGame();
        }
        if(CurrentAliveBlue == 0){
            GameOver = true;
            RestartGame();
        }
    }

    private void Update() { 
        if(GameOver == false)
        {
            Debug.Log(isBlueTurn);
            CheckNumberPlayers();
            if(unitGridCombat.GetTeam() == UnitGridCombat.Team.Blue) // turno de aliados
            {
                switch (state) {
                    case State.Normal:
                        if (Input.GetMouseButtonDown(0)) {
                            Grid<GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
                            GridObject gridObject = grid.GetGridObject(GetMouseWorldPosition());

                            // Check if clicking on a unit position
                            if (gridObject.GetUnitGridCombat() != null) {
                                // Clicked on top of a Unit
                                if (unitGridCombat.IsEnemy(gridObject.GetUnitGridCombat())) {
                                    // Clicked on an Enemy of the current unit
                                    if (unitGridCombat.CanAttackUnit(gridObject.GetUnitGridCombat())) {
                                        // Can Attack Enemy
                                        if (canAttackThisTurn) {
                                            canAttackThisTurn = false;
                                            // Attack Enemy
                                            state = State.Waiting;
                                            unitGridCombat.AttackUnit(gridObject.GetUnitGridCombat());
                                            state = State.Normal;
                                            TestTurnOver();
                                        }
                                    } else {
                                        // Cannot attack enemy
                                    }
                                    break;
                                } else {
                                    if (unitGridCombat.CanHealUnit(gridObject.GetUnitGridCombat()) && unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.healer)
                                    // si eres un healer
                                    {
                                        if (canAttackThisTurn)
                                        {
                                            canAttackThisTurn = false;
                                            state = State.Waiting;
                                            unitGridCombat.HealAlly(gridObject.GetUnitGridCombat());
                                            state = State.Normal;
                                            TestTurnOver();
                                        }
                                    }
                                    break;
                                }
                            } else {
                                // No unit here
                            }

                            if (gridObject.GetIsValidMovePosition()) {
                                // Valid Move Position

                                if (canMoveThisTurn) {
                                    canMoveThisTurn = false;

                                    state = State.Waiting;

                                    // Set entire Tilemap to Invisible
                                    GameHandler_GridCombatSystem.Instance.GetMovementTilemap().SetAllTilemapSprite(
                                        MovementTilemap.TilemapObject.TilemapSprite.None
                                    );

                                    // Remove Unit from current Grid Object
                                    grid.GetGridObject(unitGridCombat.GetPosition()).ClearUnitGridCombat();
                                    // Set Unit on target Grid Object
                                    gridObject.SetUnitGridCombat(unitGridCombat);

                                    unitGridCombat.MoveTo(GetMouseWorldPosition(), () => {
                                        state = State.Normal;
                                        UpdateValidMovePositions();
                                        TestTurnOver();
                                    });
                                }
                            }
                        }

                        if (Input.GetKeyDown(KeyCode.Space)) {
                            ForceTurnOver();
                        }
                        break;
                    case State.Waiting:
                        break;
                }
            }
            else // turno de enemigos
            {
                if (canAttackThisTurn)
                {
                    canAttackThisTurn = false;
                    canMoveThisTurn = false;
                    // Attack Enemy
                    state = State.Waiting;
                    unitGridCombat.AttackUnit(iA_Enemies.lookForEnemies(unitGridCombat));
                    state = State.Normal;
                    TestTurnOver();
                }
            }
        }
    }
    

    private void TestTurnOver() {
        if (!canMoveThisTurn && !canAttackThisTurn) {
            // Cannot move or attack, turn over
            ForceTurnOver();
        }
    }

    private void ForceTurnOver() {
        SelectNextActiveUnit();
        UpdateValidMovePositions();
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

    // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
}
