using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class IA_enemies : MonoBehaviour
{
    /*
     MELEE: 
     -tiene que ir por el enemigo más cerca
     -atacar si está a rango
     -si está bajo de vida se retira hacia atrás 

     RANGED:
     -moverse hasta rango de ataque 
     
     HEALER
     -
     */

    private float attackRangeMelee = 15;
    private float attackRangeRanged = 30;
    private float attackRangeHealer = 0;
    private float rangeHealer = 34; // curar
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

    public UnitGridCombat lookForEnemies(UnitGridCombat thisUnit) // lookForEnemies a una casilla
    {
        enemiesCount = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count;
        Vector3 myPosition = thisUnit.GetPosition();
        for (int i = 0; i <= enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            if (gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i] != null)
            {
                float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i].GetPosition());
                if (distance <= attackRangeMelee)
                {
                    return gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
                }
            }

        }
        return null;
    }

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
        
        CheckCollisions(nearestEnemy);
        relativePoint = transform.InverseTransformPoint(nearestEnemy.GetPosition());
        LookForMovePosition(relativePoint, nearestEnemy.GetPosition(), myPosition);
        gridObject = grid.GetGridObject(target);

        if (target == new Vector3(0, 0, 0))
        {
            relativePoint = CheckForNewSpots(relativePoint);
            LookForMovePosition(relativePoint, nearestEnemy.GetPosition(), myPosition);
            gridObject = grid.GetGridObject(target);
        }

        canMove = CheckMoveRange(target, myPosition);

        if(!canMove)
        {
            SelectNewMovePosition(myPosition);
            if (!CheckCollisionsTarget())
            {
                
            }
            gridObject = grid.GetGridObject(target);
            //calcular new target y asignarlo a gridobject
        }

        for (int i = 0; i < 4 * gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count; i++)
        {
            CheckCollisions(nearestEnemy);
            if (gridObject.GetUnitGridCombat() == null)
            {
                grid.GetGridObject(thisUnit.GetPosition()).ClearUnitGridCombat();
                gridObject.SetUnitGridCombat(thisUnit);
                thisUnit.MoveTo(target, () =>
                {
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
                        Debug.Log("No hay enemigos a los que pueda acercarme");
                        break;
                    }
                }
                relativePoint = CheckForNewSpots(relativePoint);
                LookForMovePosition(relativePoint, nearestEnemy.GetPosition(), myPosition);
                gridObject = grid.GetGridObject(target);
            }
        }
    }

    private void LookForMovePosition(Vector3 relativePoint, Vector3 enemyPosition, Vector3 myPosition)
    {
        if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredRight)    // X < 0, X > Y
        {
            //Derecha
            Debug.Log("1");
            target.x = enemyPosition.x - 10;
            target.y = enemyPosition.y;
            alreadyEnteredRight = true;
        }

        if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredLeft)     // X > 0, X > Y
        {
            //Izquierda
            Debug.Log("2");
            target.x = enemyPosition.x + 10;
            target.y = enemyPosition.y;
            alreadyEnteredLeft = true;
        }

        if (relativePoint.y > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredBot)      // Y > 0, Y > X
        {
            //Abajo
            Debug.Log("3");
            target.x = enemyPosition.x;
            target.y = enemyPosition.y - 10;
            alreadyEnteredBot = true;
        }

        if (relativePoint.y < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredTop)      // Y < 0, Y > X
        {
            //Encima
            Debug.Log("4");
            target.x = enemyPosition.x;
            target.y = enemyPosition.y + 10;
            alreadyEnteredTop = true;
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
        if (!alreadyEnteredLeft)
        {
            relativePoint.x = newRelativeMax;
            relativePoint.y = newRelativeMin;
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
        else
        {
            //No me puedo mover a ningun aliado
            return new Vector3(0, 0, 0);
        }
    }

    private void CheckCollisions(UnitGridCombat enemyPosition)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        grid.GetXY(enemyPosition.GetPosition(), out int unitX, out int unitY);

        if (!GridPathfinding.instance.IsWalkable(unitX - 1, unitY))
        {
            alreadyEnteredRight = true;
        }
        if (!GridPathfinding.instance.IsWalkable(unitX + 1, unitY))
        {
            alreadyEnteredLeft = true;
        }
        if (!GridPathfinding.instance.IsWalkable(unitX, unitY - 1))
        {
            alreadyEnteredTop = true;
        }
        if (!GridPathfinding.instance.IsWalkable(unitX, unitY + 1))
        {
            alreadyEnteredBot = true;
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
        Debug.Log(target + "Casilla a la que me quiero mover");
        Debug.Log(myPosition + "Mi posicion");
        Debug.Log(Vector3.Distance(myPosition, target));
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