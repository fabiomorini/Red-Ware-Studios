using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridCombat : MonoBehaviour {

    [SerializeField] private Team team;
    private Team enemyTeam;
    private CHARACTER_PREFS characterPrefs;
    private GameObject characterManager;
    private GameObject selectedGameObject;
    private GameObject gridCombatSystem;
    private GridCombatSystem sceneCombatSystem;
    private MovePositionPathfinding movePosition;
    private State state;
    public float damageAmount = 1.0f;
    private HealthSystem healthSystem;
    private UnitGridCombat unitGridCombat;
    // 17 = 1 casilla, 34 = 2, en diagonal no es exacto;
    [HideInInspector]
    public bool imDead = false;
    public float attackRangeMelee = 17;
    public float attackRangeRanged = 34;
    public float attackRangeHealer = 0;
    public float rangeHealer = 34; // curar


    //Temporal para el prototipo
    public GameObject healthUI1;
    public GameObject healthUI2;
    public GameObject healthUI3;

    public enum Team {
        Blue,
        Red
    }

    private enum State {
        Normal,
        Moving,
        Attacking
    }

    private void Awake() {
        characterPrefs = GetComponent<CHARACTER_PREFS>();
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<MovePositionPathfinding>();
        state = State.Normal;
        healthSystem = new HealthSystem(3.0f);
        gridCombatSystem = GameObject.Find("CombatHandler");
        sceneCombatSystem = gridCombatSystem.GetComponent<GridCombatSystem>();
        characterManager = GameObject.FindWithTag("characterManager");
    }

    private void Update() {
        healthUIShow();
        switch (state) {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }

    public void SetSelectedVisible(bool visible) {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        state = State.Moving;
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            state = State.Normal;
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
        state = State.Attacking;
        unitGridCombat.Damage(this, damageAmount);
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Damage(UnitGridCombat Attacker,float damage){
        healthSystem.Damage(damageAmount);
        if(healthSystem.IsDead()){
            if(Attacker.GetTeam() == Team.Blue) 
            {
                gameObject.GetComponent<UnitGridCombat>().imDead = true;
                sceneCombatSystem.CurrentAliveRed -= 1;
                sceneCombatSystem.redTeamKO.Insert(0, unitGridCombat);
                for(int i = 0; i < sceneCombatSystem.redTeamList.Count; i++)
                {
                    if(!sceneCombatSystem.redTeamList[i].imDead)
                        sceneCombatSystem.newRedTeamList.Add(sceneCombatSystem.redTeamList[i]);
                }
                sceneCombatSystem.redTeamList.Clear();
                sceneCombatSystem.redTeamList = new List<UnitGridCombat>(sceneCombatSystem.newRedTeamList);
                sceneCombatSystem.redTeamKO.Clear();
                this.gameObject.SetActive(false);
            }
            if(Attacker.GetTeam() == Team.Red)
            {
                gridCombatSystem.GetComponent<GridCombatSystem>().CurrentAliveBlue -= 1;
                gameObject.GetComponent<UnitGridCombat>().imDead = true;
                characterManager.GetComponent<CHARACTER_MNG>().checkIfDead();
                gridCombatSystem.GetComponent<GridCombatSystem>().blueTeamKO.Insert(0, unitGridCombat);
                for (int i = 0; i < sceneCombatSystem.blueTeamList.Count; i++)
                {
                    if (!sceneCombatSystem.blueTeamList[i].imDead)
                        sceneCombatSystem.newBlueTeamList.Add(sceneCombatSystem.blueTeamList[i]);
                }
                sceneCombatSystem.blueTeamList.Clear();
                sceneCombatSystem.blueTeamList = new List<UnitGridCombat>(sceneCombatSystem.newBlueTeamList);
                sceneCombatSystem.blueTeamKO.Clear();
                this.gameObject.SetActive(false);
            }
        }
    }

    public void HealAlly(UnitGridCombat unitGridCombat)
    {
        GetComponent<IMoveVelocity>().Disable();
        state = State.Attacking;
        unitGridCombat.Heal(this, damageAmount);
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Heal(UnitGridCombat Attacker, float damage)
    {
        healthSystem.Heal(damageAmount);
    }

    public bool CanAttackUnit(UnitGridCombat unitGridCombat) {
        if(gameObject.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.melee)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeMelee;
        }
        else if (gameObject.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.ranged)
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

    // Temporal
    private void healthUIShow(){
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
    }
}
