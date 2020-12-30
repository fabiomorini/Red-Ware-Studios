using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private float attackRangeMelee = 17;
    private float attackRangeRanged = 34;
    private float attackRangeHealer = 0;
    private float rangeHealer = 34; // curar
    private GameObject gridCombatSystem;
    private int enemiesCount;

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
        enemiesCount = gridCombatSystem.GetComponent<GridCombatSystem>().blueTeamList.Count;
        Vector3 myPosition = thisUnit.GetPosition();
        for (int i = 0; i <= enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().blueTeamList[i].GetPosition());
            if (distance <= attackRangeMelee)
            {
                return gridCombatSystem.GetComponent<GridCombatSystem>().blueTeamList[i];
            }
        }
        return null;
    }


    public UnitGridCombat lookForEnemiesDist(UnitGridCombat thisUnit) // lookForEnemies de más distancia
    {
        float minDist = 9999.0f; // para encontrar el enemigo más cerca
        Vector3 myPosition = thisUnit.GetPosition();
        UnitGridCombat nearestEnemy = null;
        for (int i = 0; i <= enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            float distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().blueTeamList[i].GetPosition());
            if (distance < minDist)
            {
                minDist = distance;
                nearestEnemy = gridCombatSystem.GetComponent<GridCombatSystem>().blueTeamList[i];
            }
        }
        return nearestEnemy;
    }








}
