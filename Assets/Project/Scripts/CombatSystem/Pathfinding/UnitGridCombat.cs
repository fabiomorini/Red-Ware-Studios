using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridCombat : MonoBehaviour {

    [SerializeField] private Team team;
    private Team enemyTeam;
    private CHARACTER_PREFS characterPrefs;
    [HideInInspector]
    public int level = 1;
    private GameObject characterManager;
    private GameObject selectedGameObject;
    private GameObject gridCombatSystem;
    private GridCombatSystem sceneCombatSystem;
    private MovePositionPathfinding movePosition;
    public int damageAmount = 1;
    private HealthSystem healthSystem;
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int curHealth;
    private UnitGridCombat unitGridCombat;
    // 17 = 1 casilla, 34 = 2, en diagonal no es exacto;
    [HideInInspector]
    public bool imDead = false;
    [HideInInspector]
    public float attackRangeMelee = 5;
    [HideInInspector]
    public float attackRangeRanged = 30;
    [HideInInspector]
    public float attackRangeHealer = 0;
    [HideInInspector]
    public float rangeHealer = 30; // curar

    public enum Team {
        Blue,
        Red
    }

    private void Awake() {
        characterPrefs = GetComponent<CHARACTER_PREFS>();
        selectedGameObject = transform.Find("SelectedArrow").gameObject;
        movePosition = GetComponent<MovePositionPathfinding>();
        healthSystem = new HealthSystem(3);
        gridCombatSystem = GameObject.FindWithTag("CombatHandler");
        sceneCombatSystem = GameObject.FindWithTag("CombatHandler").GetComponent<GridCombatSystem>();
        characterManager = GameObject.FindWithTag("characterManager");
    }

    private void Update()
    {
        maxHealth = healthSystem.MaxHealth;
        curHealth = healthSystem.CurrentHealth;
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            onReachedPosition();
        });
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Team GetTeam() {
        return team;
    }

    public bool IsEnemy(UnitGridCombat unitGridCombat) {
        return unitGridCombat.GetTeam() != team;
    }

    public void AttackUnit(UnitGridCombat unitGridCombat){
        GetComponent<IMoveVelocity>().Disable();
        unitGridCombat.Damage(this, damageAmount);
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Damage(UnitGridCombat Attacker,float damage){
        healthSystem.Damage(damageAmount);
        if(healthSystem.IsDead()){
            if(Attacker.GetTeam() == Team.Blue) 
            {
                imDead = true;
                sceneCombatSystem.enemiesTeamKo.Insert(0, unitGridCombat);
                for(int i = 0; i < sceneCombatSystem.enemiesTeamList.Count; i++)
                {
                    if(!sceneCombatSystem.enemiesTeamList[i].imDead)
                        sceneCombatSystem.newEnemiesTeamList.Add(sceneCombatSystem.enemiesTeamList[i]);
                }
                sceneCombatSystem.enemiesTeamList.Clear();
                sceneCombatSystem.enemiesTeamList = new List<UnitGridCombat>(sceneCombatSystem.newEnemiesTeamList);
                sceneCombatSystem.newEnemiesTeamList.Clear();
                sceneCombatSystem.enemiesTeamKo.Clear();
            }
            else if(Attacker.GetTeam() == Team.Red)
            {
                imDead = true;
                sceneCombatSystem.alliesTeamKO.Insert(0, unitGridCombat);
                for (int i = 0; i < sceneCombatSystem.alliesTeamList.Count; i++)
                {
                    if (!sceneCombatSystem.alliesTeamList[i].imDead)
                        sceneCombatSystem.newAlliesTeamList.Add(sceneCombatSystem.alliesTeamList[i]);
                }
                sceneCombatSystem.alliesTeamList.Clear();
                sceneCombatSystem.alliesTeamList = new List<UnitGridCombat>(sceneCombatSystem.newAlliesTeamList);
                sceneCombatSystem.newAlliesTeamList.Clear();
                sceneCombatSystem.alliesTeamKO.Clear();
            }
            Destroy(gameObject);
        }
    }

    public void HealAlly(UnitGridCombat unitGridCombat)
    {
        GetComponent<IMoveVelocity>().Disable();
        unitGridCombat.Heal(this, damageAmount);
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Heal(UnitGridCombat Attacker, int damage)
    {
        healthSystem.Heal(damageAmount);
    }

    public bool CanAttackUnit(UnitGridCombat unitGridCombat) {
        if(gameObject.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.MELEE)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeMelee;
        }
        else if (gameObject.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.RANGED)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeRanged;
        }
        else
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeHealer;
        }
    }
    public bool CanHealUnit(UnitGridCombat unitGridCombat)
    {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= rangeHealer;
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void setSelectedActive()
    {
        selectedGameObject.SetActive(true);
    }

    public void setSelectedFalse()
    {
        selectedGameObject.SetActive(false);
    }

    // Temporal
    /*private void healthUIShow(){
        for(int i = 0; i <= 2; i++){
            healthUI1.SetActive(false);
            healthUI2.SetActive(false);
            healthUI3.SetActive(false);
        }
        if(healthSystem.CurrentHealth == 3.0f)
        {
            healthUI3.SetActive(true);
        }
        else if (healthSystem.CurrentHealth == 2.0f)
        {
            healthUI2.SetActive(true);
        }
        else if(healthSystem.CurrentHealth == 1.0f)
        {
            healthUI1.SetActive(true);
        }
    }*/
}
