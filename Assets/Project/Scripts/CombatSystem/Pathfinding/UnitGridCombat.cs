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
    [HideInInspector] public bool isOverloaded = false;
    [HideInInspector] public float damageAmount;
    private int defense;
    [HideInInspector] public int overloadIndex;

    private int attackRangeMelee = 11;
    private int attackRangeRanged = 41;
    private int attackRangeHealer = 31;
    private int attackRangeTank = 11;
    private int attackRangeMage = 31;

    private bool attackedByMelee = false;
    private bool attackedByArcher = false;
    private bool attackedByHealer = false;
    private bool attackedByTank = false;
    private bool attackedByMage = false;

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
    private CHARACTER_MNG characterManager;

    public enum Team {
        Blue,
        Red
    }

    private void Awake() {
        isOverloaded = false;
        healthBar = GetComponentInChildren<HealthBar>();
        characterPrefs = GetComponent<CHARACTER_PREFS>();
        selectedGameObject = transform.Find("SelectedArrow").gameObject;
        characterManager = GameObject.FindGameObjectWithTag("characterManager").GetComponent<CHARACTER_MNG>();

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

    private void SetHealth() // para balancear
    {
        if(characterPrefs.getType() == CHARACTER_PREFS.Tipo.MELEE)
        {
            if(characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                maxHealth = 80;
                defense = 15;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                maxHealth = 85;
                defense = 17;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                maxHealth = 90;
                defense = 20;
            }
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.RANGED)
        {
            if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                maxHealth = 60;
                defense = 10;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                maxHealth = 65;
                defense = 12;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                maxHealth = 70;
                defense = 15;
            }
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.HEALER)
        {
            if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                maxHealth = 50;
                healAmount = 20;
                defense = 5;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                maxHealth = 55;
                healAmount = 25;
                defense = 7;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                maxHealth = 60;
                healAmount = 30;
                defense = 10;
            }

        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.TANK)
        {
            if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                maxHealth = 85;
                defense = 25;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                maxHealth = 90;
                defense = 27;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                maxHealth = 95;
                defense = 30;
            }
        }
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.MAGE)
        {
            if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                maxHealth = 50;
                defense = 10;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                maxHealth = 55;
                defense = 12;
            }
            else if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                maxHealth = 60;
                defense = 15;
            }
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
        unitGridCombat.Damage(this);
        GetComponent<IMoveVelocity>().Enable();
    }

    public void Damage(UnitGridCombat Attacker){
        if(Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            attackedByMelee = true;
            attackedByArcher = false;
            attackedByHealer = false;
            attackedByTank = false;
            attackedByMage = false;

            if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 25.0f + ((25.0f / 100.0f) * 55.0f);
                else damageAmount = 25.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 30.0f + ((30.0f / 100.0f) * 20.0f);
                else damageAmount = 30.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 35.0f + ((35.0f / 100.0f) * 20.0f);
                else damageAmount = 35.0f;
            }

        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            attackedByMelee = false;
            attackedByArcher = true;
            attackedByHealer = false;
            attackedByTank = false;
            attackedByMage = false;
            if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 20.0f + ((20.0f / 100.0f) * 20.0f);
                else damageAmount = 20.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 25.0f + ((25.0f / 100.0f) * 20.0f);
                else damageAmount = 25.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 30.0f + ((30.0f / 100.0f) * 20.0f);
                else damageAmount = 30.0f;
            }
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER)
        {
            attackedByMelee = false;
            attackedByArcher = false;
            attackedByHealer = true;
            attackedByTank = false;
            attackedByMage = false;
            if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 10.0f + ((10.0f / 100.0f) * 20.0f);
                else damageAmount = 10.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 15.0f + ((15.0f / 100.0f) * 20.0f);
                else damageAmount = 15.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 20.0f + ((20.0f / 100.0f) * 20.0f);
                else damageAmount = 20.0f;
            }
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            attackedByMelee = false;
            attackedByArcher = false;
            attackedByHealer = false;
            attackedByTank = true;
            attackedByMage = false;
            if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 15.0f + ((15.0f / 100.0f) * 20.0f);
                else damageAmount = 15.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 20.0f + ((20.0f / 100.0f) * 20.0f);
                else damageAmount = 20.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 25.0f + ((25.0f / 100.0f) * 20.0f);
                else damageAmount = 25.0f;
            }
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MAGE)
        {
            attackedByMelee = false;
            attackedByArcher = false;
            attackedByHealer = false;
            attackedByTank = false;
            attackedByMage = true;
            if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 25.0f + ((25.0f / 100.0f) * 20.0f);
                else damageAmount = 25.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 30.0f + ((30.0f / 100.0f) * 20.0f);
                else damageAmount = 30.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                if (sceneCombatSystem.inspiredAttack) damageAmount = 35.0f + ((35.0f / 100.0f) * 20.0f);
                else damageAmount = 35.0f;
            }
        }
        float dmg;
        dmg = RandomDamage(damageAmount);
        healthSystem.Damage((int)dmg);

        if (healthSystem.IsDead()){
            if(Attacker.GetTeam() == Team.Blue) 
            {
                if(characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
                {
                    if (attackedByMelee)
                    {
                        characterManager.meleeExp += 5;
                    }
                    if (attackedByArcher)
                    {
                        characterManager.archerExp += 5;
                    }
                    if (attackedByHealer)
                    {
                        characterManager.healerExp += 5;
                    }
                    if (attackedByTank)
                    {
                        characterManager.tankExp += 5;
                    }
                    if (attackedByMage)
                    {
                        characterManager.mageExp += 5;
                    }
                }
                if (characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
                {
                    if (attackedByMelee)
                    {
                        characterManager.meleeExp += 25;
                    }
                    if (attackedByArcher)
                    {
                        characterManager.archerExp += 25;
                    }
                    if (attackedByHealer)
                    {
                        characterManager.healerExp += 25;
                    }
                    if (attackedByTank)
                    {
                        characterManager.tankExp += 25;
                    }
                    if (attackedByMage)
                    {
                        characterManager.mageExp += 25;
                    }
                }

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

                if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.MELEE)
                {
                    sceneCombatSystem.numberOfMeleeLeft--;
                }
                if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.RANGED)
                {
                    sceneCombatSystem.numberOfRangedLeft--;
                }
                if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.HEALER)
                {
                    sceneCombatSystem.numberOfHealerLeft--;
                }
                if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.TANK)
                {
                    sceneCombatSystem.numberOfTankLeft--;
                }
                if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.MAGE)
                {
                    sceneCombatSystem.numberOfMageLeft--;
                }

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

    float RandomDamage(float damageAmount)
    {
        int randomNum = UnityEngine.Random.Range(-2, 2);
        damageAmount = damageAmount + randomNum;
        randomNum = UnityEngine.Random.Range(0, 100);
        if (randomNum <= 5)
        {
            damageAmount = damageAmount + (damageAmount / 100.0f) * 20.0f;
            //feedback
        }
        else if(randomNum >= 95)
        {
            damageAmount = 0;
            //feedback
        }

        if(this.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.TANK && sceneCombatSystem.overload && isOverloaded)
        {
            damageAmount = damageAmount - ((damageAmount / 100.0f) * 75.0f);
        }
        else
        {
            damageAmount = damageAmount - ((damageAmount / 100.0f) * defense);
        }

        return damageAmount;
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
        if (sceneCombatSystem.hexOfNature)
        {
            unitGridCombat.Heal(this, 60);
            sceneCombatSystem.hexOfNature = false;
            sceneCombatSystem.inspiration -= 3;
        }
        else unitGridCombat.Heal(this, healAmount);
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
