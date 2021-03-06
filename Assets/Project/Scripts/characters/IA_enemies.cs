﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;

public class IA_enemies : MonoBehaviour
{
    private GameObject gridCombatSystem;
    private int enemiesCount;
    private int alliesCount;
    private int maxMoveDistanceInt = 23;

    [HideInInspector] public UnitGridCombat needHealing;


    private GridCombatSystem.GridObject gridObject;
    private Vector3 target = new Vector3(0, 0, 0);
    private Vector3 targetIntermedio = new Vector3(0, 0, 0);

    private bool enemyOutOfRange = false;

    private void Awake()
    {
        gridCombatSystem = GameObject.FindGameObjectWithTag("CombatHandler");
    }
    //Atacas a alguien a melee
    public UnitGridCombat lookForEnemies(UnitGridCombat thisUnit) // lookForEnemies a una casilla
    {
        enemiesCount = gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList.Count;
        Vector3 myPosition = thisUnit.GetPosition();
        float distance;
        for (int i = 0; i <= enemiesCount; i++) // para comparar mi posición con la posición de todos los personajes del equipo del jugador
        {
            //El primer Player a rango, lo atacas
            distance = Vector3.Distance(myPosition, gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i].GetPosition());
            if (distance <= 11 && (thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE || thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK))
            {
                return gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
            }
            else if (distance <= 31 && (thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE || thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER))
            {
                return gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
            }
            else if (distance <= 41 && (thisUnit.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED))
            {
                return gridCombatSystem.GetComponent<GridCombatSystem>().alliesTeamList[i];
            }
        }
        return null;
    }


    public UnitGridCombat CheckAlliesHealth(UnitGridCombat thisUnit)
    {
        bool willBeHealing = false;
        alliesCount = gridCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList.Count;
        Vector3 myPosition = thisUnit.GetPosition();
        float minHealth = 9999;
        for (int i = 0; i < alliesCount; i++)
        {
            if ((gridCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i].curHealth <= gridCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i].maxHealth / 2) && (gridCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i].curHealth < minHealth))
            {
                willBeHealing = true;
                minHealth = gridCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i].curHealth;
                needHealing = gridCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i];
            }
        }

        if (willBeHealing)
        {
            return needHealing;
        }
        else
        {
            return null;
        }
    }

    //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //BUCLE DE LA IA
    //Sabemos que no estas a Rango de Ataque y te mueves
    //Buscas el Player mas cercano y lo pasas a WalkToEnemy
    public void lookForEnemiesDist(UnitGridCombat thisUnit) // lookForEnemies de más distancia
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
        CheckRange(nearestEnemy, thisUnit);
    }
    //la misma función de antes pero para aliados de la IA
    public void lookForAlliesDist(UnitGridCombat thisUnit) // lookForAllies de más distancia
    {
        CheckRange(needHealing, thisUnit);
    }

    //Comporbamos si el Enemigo mas Cercano esta a Rango de Movimiento o Hay que hacer un Target Intermedio
    private void CheckRange(UnitGridCombat nearestEnemy, UnitGridCombat thisUnit)
    {
        target = CheckTargetRange(nearestEnemy.GetPosition(), thisUnit);
        //Debug.Log(target);
        if (enemyOutOfRange)
        {
            //No Estoy en Rango de Movimiento
            WalkToEnemyOutOfRange(thisUnit);
        }
        else 
        {
            //Estoy en Rango de Movimiento
            WalkToEnemyInRange(thisUnit);
        }
    }

    //Moverse hacia un enemigo al target intermedio (Movimiento fuera del Rango de Movimiento)
    private void WalkToEnemyOutOfRange(UnitGridCombat thisUnit)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;

        SelectNewMovePosition(thisUnit.GetPosition()); //Se calcula el targetIntermedio
        gridObject = grid.GetGridObject(targetIntermedio);
        if (CheckCollisionsTargetIntermedio() && gridObject.GetUnitGridCombat() == null)
        {
            if (thisUnit.GetComponent<CHARACTER_PREFS>().tipo != CHARACTER_PREFS.Tipo.DUMMY)
            {
                grid.GetGridObject(thisUnit.GetPosition()).ClearUnitGridCombat();
                thisUnit.MoveTo(targetIntermedio, () =>
                {
                    gridObject.SetUnitGridCombat(thisUnit);
                    if (gridCombatSystem.GetComponent<GridCombatSystem>().SeekEnemiesIA(thisUnit) == true)
                    {
                        thisUnit.AttackUnit(lookForEnemies(thisUnit));
                    }
                });
            }
        }
        //Buscar nuevo target Intermedio y me muevo
        else
        {
            enemyOutOfRange = false; //El target intermedio se calcula para que siempre este a rango de movimiento, por lo tanto esto va a ser siempre false en esta parte del codigo 
            targetIntermedio = CheckTargetRange(targetIntermedio, thisUnit);
            gridObject = grid.GetGridObject(targetIntermedio);
            if (CheckCollisionsTargetIntermedio() && gridObject.GetUnitGridCombat() == null && CheckMoveRange(targetIntermedio, thisUnit.GetPosition()))
            {
                if (thisUnit.GetComponent<CHARACTER_PREFS>().tipo != CHARACTER_PREFS.Tipo.DUMMY)
                {
                    grid.GetGridObject(thisUnit.GetPosition()).ClearUnitGridCombat();
                    thisUnit.MoveTo(targetIntermedio, () =>
                    {
                        gridObject.SetUnitGridCombat(thisUnit);
                        if (gridCombatSystem.GetComponent<GridCombatSystem>().SeekEnemiesIA(thisUnit) == true)
                        {
                            thisUnit.AttackUnit(lookForEnemies(thisUnit));
                        }
                    });
                }
            }
            else //Busca un segundo target intermedio y sus adyacentes para moverse a una posicion
            {
                targetIntermedio = CheckTargetRange(targetIntermedio, thisUnit);
                gridObject = grid.GetGridObject(targetIntermedio);
                if (CheckCollisionsTargetIntermedio() && gridObject.GetUnitGridCombat() == null && CheckMoveRange(targetIntermedio, thisUnit.GetPosition()))
                {
                    if (thisUnit.GetComponent<CHARACTER_PREFS>().tipo != CHARACTER_PREFS.Tipo.DUMMY)
                    {
                        grid.GetGridObject(thisUnit.GetPosition()).ClearUnitGridCombat();
                        thisUnit.MoveTo(targetIntermedio, () =>
                        {
                            gridObject.SetUnitGridCombat(thisUnit);
                            if (gridCombatSystem.GetComponent<GridCombatSystem>().SeekEnemiesIA(thisUnit) == true)
                            {
                                thisUnit.AttackUnit(lookForEnemies(thisUnit));
                            }
                        });
                    }
                }
            }
        }
    }

    //Me muevo a la posicion dentro del rango de movimiento
    private void WalkToEnemyInRange(UnitGridCombat thisUnit)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        GridPathfinding gridPathfinding = GameHandler_GridCombatSystem.Instance.gridPathfinding;
        if (CheckCollisionsTargetIntermedio() && gridObject.GetUnitGridCombat() == null)
        {
            if (thisUnit.GetComponent<CHARACTER_PREFS>().tipo != CHARACTER_PREFS.Tipo.DUMMY)
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
            }
        }
        else
        {
            Debug.Log("Aqui no deberia entrar nunca");
        }
    }

    //Busco un target al que moverme cerca de mi posicion
    private Vector3 CheckTargetRange(Vector3 Objective, UnitGridCombat thisUnit)
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        Vector3 myPosition = thisUnit.GetPosition();
        Vector3 targetRight = new Vector3(0, 0, 0);
        Vector3 targetLeft = new Vector3(0, 0, 0);
        Vector3 targetTop = new Vector3(0, 0, 0);
        Vector3 targetBot = new Vector3(0, 0, 0);

        targetRight.x = Objective.x + 10;
        targetRight.y = Objective.y;

        targetLeft.x = Objective.x - 10;
        targetLeft.y = Objective.y;

        targetTop.x = Objective.x;
        targetTop.y = Objective.y + 10;

        targetBot.x = Objective.x;
        targetBot.y = Objective.y - 10;

        gridObject = grid.GetGridObject(targetRight);
        if (CheckMoveRange(targetRight, myPosition) && CheckCollisionsTarget() && gridObject.GetUnitGridCombat() == null)
        {
            Debug.Log("Derecha");
            enemyOutOfRange = false;
            return targetRight;
        }
        gridObject = grid.GetGridObject(targetLeft);
        if (CheckMoveRange(targetLeft, myPosition) && CheckCollisionsTarget() && gridObject.GetUnitGridCombat() == null)
        {
            Debug.Log("Izquierda");
            enemyOutOfRange = false;
            return targetLeft;
        }
        gridObject = grid.GetGridObject(targetTop);
        if (CheckMoveRange(targetTop, myPosition) && CheckCollisionsTarget() && gridObject.GetUnitGridCombat() == null)
        {
            Debug.Log("Arriba");
            enemyOutOfRange = false;
            return targetTop;
        }
        gridObject = grid.GetGridObject(targetBot);
        if (CheckMoveRange(targetBot, myPosition) && CheckCollisionsTarget() && gridObject.GetUnitGridCombat() == null)
        {
            Debug.Log("Abajo");
            enemyOutOfRange = false;
            return targetBot;
        }


        //Debug.Log(CheckMoveRange(targetRight, myPosition) + " CheckRange");
        //Debug.Log(CheckCollisionsTarget() + " CheckCollision");
        //Debug.Log((gridObject.GetUnitGridCombat() == null) + " UnitGridCombat");

        float distTop = CheckMoveOutOfRange(targetTop, myPosition);
        float distBot = CheckMoveOutOfRange(targetBot, myPosition);
        float distRight = CheckMoveOutOfRange(targetRight, myPosition);
        float distLeft = CheckMoveOutOfRange(targetLeft, myPosition);

        if (distTop < distBot && distTop < distRight && distTop < distLeft)
        {
            Debug.Log("ArribaOut");
            enemyOutOfRange = true;
            return targetTop;
        }
        if (distBot <= distTop && distBot <= distRight && distBot <= distLeft)
        {
            Debug.Log("AbajoOut");
            enemyOutOfRange = true;
            return targetBot;
        }
        if (distRight <= distBot && distRight <= distTop && distRight <= distLeft)
        {
            Debug.Log("DerechaOut");
            enemyOutOfRange = true;
            return targetRight;
        }
        if (distLeft < distBot && distLeft < distRight && distLeft < distTop)
        {
            Debug.Log("IzquierdaOut");
            enemyOutOfRange = true;
            return targetLeft;
        }

        Debug.Log("Else");
        return new Vector3(0, 0, 0);

    }

    private bool CheckMoveRange(Vector3 target, Vector3 myPosition)
    {
        return Mathf.Abs(Vector3.Distance(myPosition, target)) <= maxMoveDistanceInt + 1;
    }

    private float CheckMoveOutOfRange(Vector3 target, Vector3 myPosition)
    {
        return Mathf.Abs(Vector3.Distance(myPosition, target));
    }

    private bool CheckCollisionsTarget()
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        grid.GetXY(target, out int unitX, out int unitY);

        return GridPathfinding.instance.IsWalkable(unitX, unitY);
    }

    private bool CheckCollisionsTargetIntermedio()
    {
        Grid<GridCombatSystem.GridObject> grid = GameHandler_GridCombatSystem.Instance.GetGrid();
        grid.GetXY(targetIntermedio, out int unitX, out int unitY);

        return GridPathfinding.instance.IsWalkable(unitX, unitY);
    }

    private void SelectNewMovePosition(Vector3 myPosition)
    {
        Vector3 directionOfTravel = target - myPosition;
        Vector3 finalDirection = directionOfTravel.normalized * maxMoveDistanceInt;
        targetIntermedio = myPosition + finalDirection;
        //Debug.Log(targetIntermedio);
    }
}