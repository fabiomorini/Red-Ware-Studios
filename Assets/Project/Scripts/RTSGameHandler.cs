//SCRIPT PRINCIPAL INICIALIZACION DEL JUEGO
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;
using Cinemachine;

public class RTSGameHandler : MonoBehaviour {

    //Inicializaciones de Cinemachine y pathfinding
    public static RTSGameHandler Instance { get; private set; }

    [SerializeField] private Transform cinemachineFollowTransform;
    [SerializeField] private VisualMovementRange movementTilemapVisual;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private Grid<CombatHandler.GridObject> grid;
    private MovementTilemap movementTilemap;
    public GridPathfinding gridPathfinding;

    private void Awake() {
        Instance = this;
            //Tamaño
        int mapWidth = 40;
        int mapHeight = 25;
        float cellSize = 10f;

        Vector3 origin = new Vector3(0, 0);

        grid = new Grid<CombatHandler.GridObject>(mapWidth, mapHeight, cellSize, origin, (Grid<CombatHandler.GridObject> g, int x, int y) => new CombatHandler.GridObject(g, x, y));

        gridPathfinding = new GridPathfinding(origin + new Vector3(1, 1) * cellSize * .5f, new Vector3(mapWidth, mapHeight) * cellSize, cellSize);
        gridPathfinding.RaycastWalkable();
        
        movementTilemap = new MovementTilemap(mapWidth, mapHeight, cellSize, origin);
    }

    private void Start() {
        movementTilemap.SetTilemapVisual(movementTilemapVisual);       
    }
    //Mover la camara libremente
    private void Update() {
        HandleCameraMovement();
    }

    //Mover la camara libremente
    private void HandleCameraMovement() {
        Vector3 moveDir = new Vector3(0, 0);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveDir.y = +1;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveDir.y = -1;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveDir.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveDir.x = +1;
        }
        moveDir.Normalize();

        float moveSpeed = 80f;
        cinemachineFollowTransform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    public Grid<CombatHandler.GridObject> GetGrid() {
        return grid;
    }

    public MovementTilemap GetMovementTilemap() {
        return movementTilemap;
    }

    //Mover la camara al jugador que sea su turno
    public void SetCameraFollowPosition(Vector3 targetPosition) {
        cinemachineFollowTransform.position = targetPosition;
    }

    public class EmptyGridObject {

        private Grid<EmptyGridObject> grid;
        private int x;
        private int y;

        public EmptyGridObject(Grid<EmptyGridObject> grid, int x, int y) {
            this.grid = grid;
            //X e Y del jugador
            this.x = x;
            this.y = y;

            Vector3 worldPos00 = grid.GetWorldPosition(x, y);
            Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
            Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
            Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

            Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
            Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
            Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
            Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
        }

        public override string ToString() {
            return "";
        }
    }
}