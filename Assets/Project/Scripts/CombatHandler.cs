using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class CombatHandler : MonoBehaviour
{
   [SerializeField] private PlayerStateUpdater[] unitGridCombatArray;

    private State state;
    private PlayerStateUpdater unitGridCombat;
    private List<PlayerStateUpdater> blueTeamList;
    private List<PlayerStateUpdater> redTeamList;
    private int blueTeamActiveUnitIndex;
    private int redTeamActiveUnitIndex;
    private bool canMoveThisTurn;
    private bool canAttackThisTurn;

    private enum State {
        Normal,
        Waiting
    }

    private void Awake() {
        state = State.Normal;
    }

    private void Start() {
        blueTeamList = new List<PlayerStateUpdater>();
        redTeamList = new List<PlayerStateUpdater>();
        blueTeamActiveUnitIndex = -1;
        redTeamActiveUnitIndex = -1;

        // Set all UnitGridCombat on their GridPosition
        foreach (PlayerStateUpdater unitGridCombat in unitGridCombatArray) {
            RTSGameHandler.Instance.GetGrid().GetGridObject(unitGridCombat.GetPosition())
                .SetUnitGridCombat(unitGridCombat);

            if (unitGridCombat.GetTeam() == PlayerStateUpdater.Team.Blue) {
                blueTeamList.Add(unitGridCombat);
            } else {
                redTeamList.Add(unitGridCombat);
            }
        }

        SelectNextActiveUnit();
        UpdateValidMovePositions();
    }

    //Sistema por turnos, primero 1 rojo despues 1 azul
    //for rojos en la lista de redteam - GetNextActiveUnit hasta que la count de la lista sea = 0
    //pasar a la lista de blueteam
    //despues volver
    private void SelectNextActiveUnit() {
        if (unitGridCombat == null || unitGridCombat.GetTeam() == PlayerStateUpdater.Team.Red) {
            unitGridCombat = GetNextActiveUnit(PlayerStateUpdater.Team.Blue);
        } else {
            unitGridCombat = GetNextActiveUnit(PlayerStateUpdater.Team.Red);
        }

        RTSGameHandler.Instance.SetCameraFollowPosition(unitGridCombat.GetPosition());
        canMoveThisTurn = true;
        canAttackThisTurn = true;
    }

    private PlayerStateUpdater GetNextActiveUnit(PlayerStateUpdater.Team team) {
        if (team == PlayerStateUpdater.Team.Blue) {
            blueTeamActiveUnitIndex = (blueTeamActiveUnitIndex + 1) % blueTeamList.Count;
            if (blueTeamList[blueTeamActiveUnitIndex] == null || blueTeamList[blueTeamActiveUnitIndex].IsDead()) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return blueTeamList[blueTeamActiveUnitIndex];
            }
        } else {
            redTeamActiveUnitIndex = (redTeamActiveUnitIndex + 1) % redTeamList.Count;
            if (redTeamList[redTeamActiveUnitIndex] == null || redTeamList[redTeamActiveUnitIndex].IsDead()) {
                // Unit is Dead, get next one
                return GetNextActiveUnit(team);
            } else {
                return redTeamList[redTeamActiveUnitIndex];
            }
        }
    }

    private void UpdateValidMovePositions() {
        Grid<GridObject> grid = RTSGameHandler.Instance.GetGrid();
        GridPathfinding gridPathfinding = RTSGameHandler.Instance.gridPathfinding;

        // Get Unit Grid Position X, Y
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        // Set entire Tilemap to Invisible
        RTSGameHandler.Instance.GetMovementTilemap().SetAllTilemapSprite(
            MovementTilemap.TilemapObject.TilemapSprite.None
        );

        // Reset Entire Grid ValidMovePositions
        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                grid.GetGridObject(x, y).SetIsValidMovePosition(false);
            }
        }

        //Global mas adelante
        int maxMoveDistance = 5;
        for (int x = unitX - maxMoveDistance; x <= unitX + maxMoveDistance; x++) {
            for (int y = unitY - maxMoveDistance; y <= unitY + maxMoveDistance; y++) {
                if (gridPathfinding.IsWalkable(x, y)) {
                    // Position is Walkable
                    if (gridPathfinding.HasPath(unitX, unitY, x, y)) {
                        // There is a Path
                        if (gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance) {
                            // Path within Move Distance

                            // Set Tilemap Tile to Move
                            RTSGameHandler.Instance.GetMovementTilemap().SetTilemapSprite(
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
                    //
                }
            }
        }
    }

    private void Update() {
        switch (state) {
            case State.Normal:
                if (Input.GetMouseButtonDown(0)) {
                    Grid<GridObject> grid = RTSGameHandler.Instance.GetGrid();
                    //GetMouseWorldPosition de Utils, traerlo aqui o a un script compartido
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
                                   /* PlayerStateUpdater.AttackUnit(gridObject.GetUnitGridCombat(), () => {
                                        state = State.Normal;
                                        TestTurnOver();
                                    });*/
                                }
                            } else {
                                // Cannot attack enemy
                                //CodeMonkey.CMDebug.TextPopupMouse("Cannot attack!");
                            }
                            break;
                        } else {
                            // Not an enemy
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
                            RTSGameHandler.Instance.GetMovementTilemap().SetAllTilemapSprite(
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
        private PlayerStateUpdater unitGridCombat;

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

        public void SetUnitGridCombat(PlayerStateUpdater unitGridCombat) {
            this.unitGridCombat = unitGridCombat;
        }

        public void ClearUnitGridCombat() {
            SetUnitGridCombat(null);
        }

        public PlayerStateUpdater GetUnitGridCombat() {
            return unitGridCombat;
        }

    }
    //Pasar a un script de funciones mas adelante y usarlo como libreria
    //Recoge la posicion del raton en el mundo
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
}
