using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UnitGridCombat : MonoBehaviour {

    [SerializeField] private Team team;
    private CHARACTER_PREFS characterPrefs;

    [HideInInspector] public int maxHealth;
    [HideInInspector] public int curHealth;
    [HideInInspector] public bool imDead = false;
    [HideInInspector] public int damageAmount;
    [HideInInspector] public int level = 1; //temporal

    private int attackRangeMelee = 10;
    private int attackRangeRanged = 40;
    private int attackRangeHealer = 30;
    private int attackRangeTank = 10;
    private int attackRangeMage = 30;

    private int rangeHeal = 30;
    private int healAmount = 20;

    // Feedback
    public GameObject slashAnim;
    public GameObject healAnim;
    public SpriteRenderer playerSprite;
    [HideInInspector] public bool animEnded = true;

    [HideInInspector] public GameObject selectedGameObject;
    private GridCombatSystem sceneCombatSystem;
    private MovePositionPathfinding movePosition;
    private HealthSystem healthSystem;
    private HealthBar healthBar;

    public enum Team {
        Blue,
        Red
    }

    private void Awake() {
        healthBar = GetComponentInChildren<HealthBar>();
        characterPrefs = GetComponent<CHARACTER_PREFS>();
        selectedGameObject = transform.Find("SelectedArrow").gameObject;

        SetHealth();
        healthSystem = new HealthSystem(maxHealth);
        curHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(curHealth);
        healthBar.SetHealthNumber(curHealth);
        movePosition = GetComponent<MovePositionPathfinding>();
        sceneCombatSystem = GameObject.FindWithTag("CombatHandler").GetComponent<GridCombatSystem>();
    }

    private void Update()
    {
        curHealth = healthSystem.CurrentHealth;
        healthBar.SetHealth(curHealth);
        healthBar.SetHealthNumber(curHealth);
        animEnded = true;
    }

    private void SetHealth()
    {
        if(characterPrefs.getType() == CHARACTER_PREFS.Tipo.MELEE)
        {
            maxHealth = 80;
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.RANGED)
        {
            maxHealth = 60;
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.HEALER)
        {
            maxHealth = 50;
            healAmount = 20;
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.TANK)
        {
            maxHealth = 95;
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.MAGE)
        {
            maxHealth = 50;
        }
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

    public void Damage(UnitGridCombat Attacker,int damage){
        //Temporal
        if(Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            damageAmount = 25;
            healthSystem.Damage(damageAmount);
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            damageAmount = 20;
            healthSystem.Damage(damageAmount);
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            damageAmount = 10;
            healthSystem.Damage(damageAmount);
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            damageAmount = 15;
            healthSystem.Damage(damageAmount);
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            damageAmount = 25;
            healthSystem.Damage(damageAmount);
        }

        if (healthSystem.IsDead()){
            if(Attacker.GetTeam() == Team.Blue) 
            {
                imDead = true;
                sceneCombatSystem.CheckIfDead();
                for(int i = 0; i < sceneCombatSystem.enemiesTeamList.Count; i++)
                {
                    if(!sceneCombatSystem.enemiesTeamList[i].imDead)
                        sceneCombatSystem.newEnemiesTeamList.Add(sceneCombatSystem.enemiesTeamList[i]);
                }
                CleanListIA();
            }
            else if(Attacker.GetTeam() == Team.Red)
            {
                imDead = true;
                sceneCombatSystem.CheckIfDead();
                sceneCombatSystem.allydeads += 1;
                for (int i = 0; i < sceneCombatSystem.alliesTeamList.Count; i++)
                {
                    if (!sceneCombatSystem.alliesTeamList[i].imDead)
                        sceneCombatSystem.newAlliesTeamList.Add(sceneCombatSystem.alliesTeamList[i]);
                }
                CleanListAlly();
            }
        }
        StartCoroutine(FeedbackAttack());
    }

    private IEnumerator FeedbackAttack()
    {
        animEnded = false;
        slashAnim.SetActive(true);
        SoundManager.PlaySound("Attack");
        yield return new WaitForSeconds(0.5f);
        slashAnim.SetActive(false);
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        playerSprite.color = Color.white;
        animEnded = true;
        if (imDead)
            Destroy(gameObject);
        sceneCombatSystem.CheckIfGameIsOver();
    }

    private IEnumerator FeedbackHealing(int healAmount)
    {
        animEnded = false;
        healAnim.SetActive(true);
        SoundManager.PlaySound("Healing");
        yield return new WaitForSeconds(1.1f);
        healAnim.SetActive(false);
        playerSprite.color = Color.green;
        healthSystem.Heal(healAmount);
        yield return new WaitForSeconds(0.3f);
        playerSprite.color = Color.white;
        animEnded = true;
    }

    private void CleanListIA()
    {
        sceneCombatSystem.enemiesTeamList.Clear();
        sceneCombatSystem.enemiesTeamList = new List<UnitGridCombat>(sceneCombatSystem.newEnemiesTeamList);
        sceneCombatSystem.newEnemiesTeamList.Clear();
    }

    private void CleanListAlly()
    {
        sceneCombatSystem.alliesTeamList.Clear();
        sceneCombatSystem.alliesTeamList = new List<UnitGridCombat>(sceneCombatSystem.newAlliesTeamList);
        sceneCombatSystem.newAlliesTeamList.Clear();
    }

    public void HealAlly(UnitGridCombat unitGridCombat)
    {
        unitGridCombat.Heal(this, healAmount);
    }

    public void Heal(UnitGridCombat Attacker, int healAmount)
    {
        StartCoroutine(FeedbackHealing(healAmount));
    }

    public bool CanAttackUnit(UnitGridCombat unitGridCombat) {
        if(gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeMelee;
        }
        else if (gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeRanged;
        }
        else if (gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeHealer;
        }
        else if (gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeMage;
        }
        else // Tank
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeTank;
        }
    }
    public bool CanHealUnit(UnitGridCombat unitGridCombat)
    {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= rangeHeal;
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
}
