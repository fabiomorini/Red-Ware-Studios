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

    /* private void TurnIA(UnitGridCombat unitGridCombat)
     {
         if (unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.melee)
         {
             switch(state)
             {
                 case State.Normal:
                     if(){
                         lookForEnemies();
                     }
                     break;


                 case State.Waiting:
                     break;
             }
         }
         else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.ranged)
         {

         }
         else if (unitGridCombat.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.healer)
         {

         }
     }*/


    public UnitGridCombat lookForEnemies(UnitGridCombat thisUnit) // lookForEnemies a una casilla
    {
        Debug.Log(thisUnit);
        //Debug.Log("1");
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
        //Debug.Log("2");
        enemiesCount = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count;
        float minDist = 9999.0f; // para encontrar el enemigo más cerca
        Vector3 myPosition = thisUnit.GetPosition();
        UnitGridCombat nearestEnemy = null;
        for (int i = 0; i < enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            //Debug.Log("4");
            float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i].GetPosition());
            if (distance < minDist)
            {
                //Debug.Log("5");
                minDist = distance;
                nearestEnemy = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
                //Debug.Log(nearestEnemy.GetPosition());
            }
        }
        WalkToEnemy(nearestEnemy, thisUnit);
        return nearestEnemy;
    }


    public void WalkToEnemy(UnitGridCombat nearestEnemy, UnitGridCombat thisUnit)
    {
        //Debug.Log(nearestEnemy);
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        Vector3 target = new Vector3(0, 0, 0);
        Vector3 myPosition = thisUnit.GetPosition();
        Vector3 enemyPosition = nearestEnemy.GetPosition();

        Vector3 relativePoint;

        relativePoint = transform.InverseTransformPoint(nearestEnemy.GetPosition());
        if (relativePoint.x < 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y))
        {
            //Derecha
            target.x = enemyPosition.x - 10;
            target.y = enemyPosition.y;
        }
        if (relativePoint.x > 0f && Mathf.Abs(relativePoint.x) > Mathf.Abs(relativePoint.y))
        {
            //Izquierda
            target.x = enemyPosition.x + 10;
            target.y = enemyPosition.y;
        }
        if (relativePoint.y > 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y))
        {
            //Abajo
            target.x = enemyPosition.x;
            target.y = enemyPosition.y - 10;
        }
        if (relativePoint.y < 0 && Mathf.Abs(relativePoint.x) < Mathf.Abs(relativePoint.y))
        {
            //Encima
            target.x = enemyPosition.x;
            target.y = enemyPosition.y + 10;
        }
       //Debug.Log(target.x);
       //Debug.Log(target.y);

        GridCombatSystem.GridObject gridObject = grid.GetGridObject(target);

        if (gridObject.GetUnitGridCombat() == null)
        {
            thisUnit.MoveTo(target, () =>
            {
                if (gridCombatSystem.GetComponent<GridCombatSystem>().SeekEnemiesIA(thisUnit) == true)
                {
                    unitGridCombat.AttackUnit(lookForEnemies(unitGridCombat));
                }
            });

        }
        else
        {
            //Ocupado
            //Debug.Log("10");
        }

    }
}