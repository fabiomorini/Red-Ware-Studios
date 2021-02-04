using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class IA_enemies : MonoBehaviour
{
    private GameObject gridCombatSystem;
    private int enemiesCount;
    private int maxMoveDistance = 5;
    private int maxMoveDistanceInt = 20;


    bool alreadyEnteredRight = false;
    bool alreadyEnteredLeft = false;
    bool alreadyEnteredTop = false;
    bool alreadyEnteredBot = false;

    private bool canMove = false;
    GridCombatSystem.GridObject gridObject;
    Vector3 target = new Vector3(0, 0, 0);

    private void Awake()
    {
        gridCombatSystem = GameObject.FindGameObjectWithTag("CombatHandler");
    }


    //Ya sabemos que estas a Rango de Ataque y te mueves 
    public UnitGridCombat lookForEnemies(UnitGridCombat thisUnit) // lookForEnemies a una casilla
    {
        enemiesCount = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count;
        Vector3 myPosition = thisUnit.GetPosition();
        for (int i = 0; i <= enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            if (gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i] != null)
            {
                //El primer Player a rango, lo atacas
                float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i].GetPosition());
                if (distance <= 11)
                {
                    return gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
                }
            }
        }
        return null;
    }

    //Sabemos que no estas a Rango de Ataque y te mueves
    //Buscas el Player mas cercano y lo pasas a WalkToEnemy
    public UnitGridCombat lookForEnemiesDist(UnitGridCombat thisUnit) // lookForEnemies de más distancia
    {
        enemiesCount = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count;
        float minDist = 9999.0f; // para encontrar el enemigo más cerca
        Vector3 myPosition = thisUnit.GetPosition();
        UnitGridCombat nearestEnemy = null;
        for (int i = 0; i < enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i].GetPosition());
            if (distance < minDist)
            {
                minDist = distance;
                nearestEnemy = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
            }
        }
        WalkToEnemy(nearestEnemy, thisUnit);
        return nearestEnemy;
    }

    //No me puedo mover al Player mas cercano, busco otro Player
    private UnitGridCombat FindNewEnemy(Vector3 myPosition, UnitGridCombat nearestEnemy)
    {
        UnitGridCombat newNearestEnemy = null;
        float minDist = 9999.0f; // para encontrar el enemigo más cerca
        for (int i = 0; i < enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i].GetPosition());
            if (distance < minDist && gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i] != nearestEnemy)
            {
                minDist = distance;
                newNearestEnemy = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
            }
        }
        return newNearestEnemy;
    }

    public void WalkToEnemy(UnitGridCombat nearestEnemy, UnitGridCombat thisUnit)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;
        
        Vector3 myPosition = thisUnit.GetPosition();
        Vector3 relativePoint;
        Vector3 relativePointTarget;

        CheckCollisionsIA(nearestEnemy);
        //Check Collisions de Players
        relativePoint = transform.InverseTransformPoint(nearestEnemy.GetPosition());
        LookForMovePosition(relativePoint, nearestEnemy.GetPosition());
        gridObject = grid.GetGridObject(target);

        if (target == new Vector3(0, 0, 0))
        {
            relativePoint = CheckForNewSpots(relativePoint);
            LookForMovePosition(relativePoint, nearestEnemy.GetPosition());
            gridObject = grid.GetGridObject(target);
        }

        canMove = CheckMoveRange(target, myPosition);

        //Estas a mas 2 casillas de cualquier Player por lo tanto te mueves 2 hacia el target
        if(!canMove)
        {
            SelectNewMovePosition(myPosition);
            gridObject = grid.GetGridObject(target);
            relativePointTarget = transform.InverseTransformPoint(target);
            //bool canMoveAgain = true;
            for (int i = 0; i < 6; i++)
            {
                if (!CheckCollisionsTarget() || gridObject.GetUnitGridCombat() != null || !CheckMoveRange(target, myPosition))
                {
                    Debug.Log("No estoy a Rango de ningun Player y mi posicion intermedia esta ocupada");
                    //De la nueva posicion dentro de mi rango, hay alguien en la casilla seleccionada (1era iteracion)
                    if (i == 1)
                    {
                        target.x = target.x - 10;
                    }
                    if (i == 2)
                    {
                        target.x = target.x + 10;
                        target.y = target.y - 10;
                    }
                    if (i == 3)
                    {
                        target.y = target.y + 10;
                        target.y = target.y + 10;
                    }
                    if (i == 4)
                    {
                        target.y = target.y - 10;
                        target.x = target.x + 10;
                    }
                    if (i == 5)
                    {
                        target = myPosition;
                        break;
                    }
                }
                else
                {
                    //Me muevo
                    break;
                }
            }
        }

        bool checkRange = true;
        //Solo entra si esta a rango
        for (int i = 0; i < 4 * gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count; i++)
        {
            int tries = 0;
            CheckCollisionsIA(nearestEnemy);
            if (gridObject.GetUnitGridCombat() == null && checkRange)
            {
                grid.GetGridObject(thisUnit.GetPosition()).ClearUnitGridCombat();
                thisUnit.MoveTo(target, () =>
                {
                    gridObject.SetUnitGridCombat(thisUnit);
                    if (gridCombatSystem.GetComponent<GridCombatSystem>().SeekEnemiesIA(thisUnit) == true)
                    {
                        thisUnit.AttackUnit(lookForEnemies(thisUnit));
                    }
                });
                //Debug.Log("Derecha = " + alreadyEnteredLeft + " / Izquierda = " + alreadyEnteredRight + " / Arriba = " + alreadyEnteredTop + " / Abajo = " + alreadyEnteredBot);
                break;
            }
            else
            {
                if (alreadyEnteredTop && alreadyEnteredBot && alreadyEnteredLeft && alreadyEnteredRight)
                {
                    nearestEnemy = FindNewEnemy(myPosition, nearestEnemy);
                    if (nearestEnemy != null)
                    {
                        //Comprobar si el newEnemy esta a rango de movimiento para hacer update de target
                        ResetPositions();
                    }
                    else
                    {
                        break;
                    }
                }

                if (tries <= 4)
                {
                    relativePoint = CheckForNewSpots(relativePoint);
                    LookForMovePosition(relativePoint, nearestEnemy.GetPosition());
                    gridObject = grid.GetGridObject(target);
                    checkRange = CheckMoveRange(target, myPosition);
                    tries++;
                }
                else
                {
                    if (!alreadyEnteredTop || !alreadyEnteredBot || !alreadyEnteredLeft || !alreadyEnteredRight)
                    {
                        Debug.Log("Estoy a Rango un Player y mi posicion esta ocupada o fuera de rango");
                        //Opcional cambiar target
                        SelectNewMovePosition(myPosition);
                        checkRange = CheckMoveRange(target, myPosition);
                    }
                }
            }
        }
    }

    private void LookForMovePositionInRange(Vector3 relativePoint)
    {
        if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredRight)    // X < 0, X > Y
        {
            //Si tu estás a la Derecha
            target.x = target.x - 10;
            alreadyEnteredRight = true;
        }
        if (relativePoint.y > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredBot)      // Y > 0, Y > X
        {
            //Si tu estás Abajo
            target.y = target.y - 10;
            alreadyEnteredBot = true;
        }
        if (relativePoint.y < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredTop)      // Y < 0, Y > X
        {
            //Si tu estás Encima
            target.y = target.y + 10;
            alreadyEnteredTop = true;
        }
        if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredLeft)     // X > 0, X > Y
        {
            //Si tu estás a la Izquierda
            target.x = target.x + 10;
            alreadyEnteredLeft = true;
        }


    }

    private void LookForMovePosition(Vector3 relativePoint, Vector3 enemyPosition)
    {
        if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredRight)    // X < 0, X > Y
        {
            //Derecha
            target.x = enemyPosition.x - 10;
            target.y = enemyPosition.y;
            alreadyEnteredRight = true;
        }
        if (relativePoint.y > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredBot)      // Y > 0, Y > X
        {
            //Abajo
            target.x = enemyPosition.x;
            target.y = enemyPosition.y - 10;
            alreadyEnteredBot = true;
        }

        if (relativePoint.y < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredTop)      // Y < 0, Y > X
        {
            //Encima
            target.x = enemyPosition.x;
            target.y = enemyPosition.y + 10;
            alreadyEnteredTop = true;
        }
        if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredLeft)     // X > 0, X > Y
        {
            //Izquierda
            target.x = enemyPosition.x + 10;
            target.y = enemyPosition.y;
            alreadyEnteredLeft = true;
        }
    }

    private Vector3 CheckForNewSpots(Vector3 relativePoint)
    {
        int newRelativeMax = 30;
        int newRelativeMin = 15;

        if (!alreadyEnteredRight)
        {
            relativePoint.x = -newRelativeMax;
            relativePoint.y = -newRelativeMin;
            return relativePoint;
        }
        if (!alreadyEnteredBot)
        {
            relativePoint.x = newRelativeMin;
            relativePoint.y = newRelativeMax;
            return relativePoint;
        }
        if (!alreadyEnteredTop)
        {
            relativePoint.x = -newRelativeMin;
            relativePoint.y = -newRelativeMax;
            return relativePoint;
        }
        if (!alreadyEnteredLeft)
        {
            relativePoint.x = newRelativeMax;
            relativePoint.y = newRelativeMin;
            return relativePoint;
        }
        else
        {
            //No me puedo mover a ningun aliado
            return new Vector3(0, 0, 0);
        }
    }

    private void CheckCollisionsIA(UnitGridCombat enemyPosition)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        grid.GetXY(enemyPosition.GetPosition(), out int unitX, out int unitY);

        if (!GridPathfinding.instance.IsWalkable(unitX - 1, unitY))
        {
            alreadyEnteredRight = true;
        }
        if (!GridPathfinding.instance.IsWalkable(unitX, unitY - 1))
        {
            alreadyEnteredTop = true;
        }
        if (!GridPathfinding.instance.IsWalkable(unitX, unitY + 1))
        {
            alreadyEnteredBot = true;
        }
        if (!GridPathfinding.instance.IsWalkable(unitX + 1, unitY))
        {
            alreadyEnteredLeft = true;
        }
    }

    private bool CheckCollisionsTarget()
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        grid.GetXY(target, out int unitX, out int unitY);
        
        return GridPathfinding.instance.IsWalkable(unitX, unitY);    
    }

    private bool CheckMoveRange(Vector3 target, Vector3 myPosition)
    {
        return Mathf.Abs(Vector3.Distance(myPosition, target)) <= maxMoveDistanceInt;
    }

    private void SelectNewMovePosition(Vector3 myPosition)
    {
        Vector3 directionOfTravel = target - myPosition;
        Vector3 finalDirection = directionOfTravel.normalized * maxMoveDistanceInt;
        target = myPosition + finalDirection;
    }

    public void ResetPositions()
    {
        alreadyEnteredRight = false;
        alreadyEnteredLeft = false;
        alreadyEnteredTop = false;
        alreadyEnteredBot = false;
    }
}