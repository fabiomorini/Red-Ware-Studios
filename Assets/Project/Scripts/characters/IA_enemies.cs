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


    UPDATE{
    
    //inputs
    //ataques
    //movimientos
    
    
    }



     */

    private UnitGridCombat unitGridCombat;
    private State state;
    private float attackRangeMelee = 15;
    private float attackRangeRanged = 30;
    private float attackRangeHealer = 0;
    private float rangeHealer = 34; // curar
    private GameObject gridCombatSystem;
    private int enemiesCount;
    private bool isClose; // para ver si hay un enemigo a una casilla
    private int maxMoveDistance = 5;

    bool alreadyEnteredRight = false;
    bool alreadyEnteredLeft = false;
    bool alreadyEnteredTop = false;
    bool alreadyEnteredBot = false;

    private enum State
    {
        Normal,
        Waiting
    }

    private void Awake()
    {
        state = State.Normal;
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


    public void WalkToEnemy(UnitGridCombat nearestEnemy, UnitGridCombat thisUnit)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        Vector3 target = new Vector3(0, 0, 0);
        Vector3 myPosition = thisUnit.GetPosition();
        Vector3 relativePoint;
        relativePoint = transform.InverseTransformPoint(nearestEnemy.GetPosition());
        target = LookForMovePosition(relativePoint, target, nearestEnemy.GetPosition(), myPosition);

        GridCombatSystem.GridObject gridObject = grid.GetGridObject(target);

        for (int i = 0; i < 4; i++)
        {
            if (gridObject.GetUnitGridCombat() == null)
            {
                grid.GetGridObject(thisUnit.GetPosition()).ClearUnitGridCombat();
                gridObject.SetUnitGridCombat(thisUnit);

                thisUnit.MoveTo(target, () =>
                {
                    gridCombatSystem.GetComponent<GridCombatSystem>().UpdateValidMovePositions();
                });
                break;
            }
            else
            {
                relativePoint = CheckForNewSpots(relativePoint);
                target = LookForMovePosition(relativePoint, target, nearestEnemy.GetPosition(), myPosition);
                gridObject = grid.GetGridObject(target);
            }
        }
    }

    private Vector3 LookForMovePosition(Vector3 relativePoint, Vector3 target, Vector3 enemyPosition, Vector3 myPosition)
    {
        Debug.Log(relativePoint);
        if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredRight)    // X < 0, X > Y
        {
            //Derecha
            target.x = enemyPosition.x - 10;
            target.y = enemyPosition.y;
            alreadyEnteredRight = true;
            return target;
        }
        if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y) && !alreadyEnteredLeft)     // X > 0, X > Y
        {
            //Izquierda
            target.x = enemyPosition.x + 10;
            target.y = enemyPosition.y;
            alreadyEnteredLeft = true;
            return target;
        }
        if (relativePoint.y > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredBot)
        {
            //Abajo
            target.x = enemyPosition.x;
            target.y = enemyPosition.y - 10;
            alreadyEnteredBot = true;
            return target;
        }
        if (relativePoint.y < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y) && !alreadyEnteredTop)
        {
            //Encima
            target.x = enemyPosition.x;
            target.y = enemyPosition.y + 10;
            alreadyEnteredTop = true;
            return target;
        }
        else
        {
            return new Vector3(myPosition.x, myPosition.y, 0);
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

    public void ResetPositions()
    {
        alreadyEnteredRight = false;
        alreadyEnteredLeft = false;
        alreadyEnteredTop = false;
        alreadyEnteredBot = false;
    }
}