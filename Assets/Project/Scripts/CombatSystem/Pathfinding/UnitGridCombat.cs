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
    [HideInInspector] public int attackRangeHealer = 31;
    private int attackRangeTank = 11;
    private int attackRangeMage = 31;

    private bool attackedByMelee = false;
    private bool attackedByArcher = false;
    private bool attackedByHealer = false;
    private bool attackedByTank = false;
    private bool attackedByMage = false;

    private int rangeHeal = 30;
    private int healAmount = 20;

    // numero de veces en las que el fuego te haya dañado
    private int fireBurstIndex = 0;
    private int fireDamage;

    // Feedback
    public GameObject slashAnim;
    public GameObject healAnim;
    public GameObject lightningAnim;
    public GameObject overloadAnim;
    public GameObject explosionAnim;
    public GameObject arrowAnim;
    public GameObject boltAnim;

    public GameObject selectedBox;

    public SpriteRenderer playerSprite;
    [HideInInspector] public bool animEnded = true;

    [HideInInspector] public GameObject selectedGameObject;
    private GridCombatSystem sceneCombatSystem;
    private MovePositionPathfinding movePosition;
    private HealthSystem healthSystem;
    private HealthBar healthBar;
    private CHARACTER_MNG characterManager;

    //Arrow
    private Vector3 myPosArrow;
    private Vector3 attackerPosArrow;
    private Vector2 p0;
    private Vector2 p1;
    private Vector2 p2;
    private Vector2 p3;

    public enum Team {
        Blue,
        Red
    }

    public bool burning = false;
    public int burningIndex = 0;
    public int burnDamage = 0;

    public bool alreadyMoved = false;

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

        if(sceneCombatSystem.burstTurns >= 5)
        {
            fireBurstIndex = 0;
        }
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
        else if (characterPrefs.getType() == CHARACTER_PREFS.Tipo.DUMMY)
        {
            maxHealth = 20000;
            defense = 10;
        }
    }


    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        selectedBox.SetActive(false);
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            SetCorrectPosition();
            selectedBox.SetActive(true);
            onReachedPosition();
        });
        SoundManager.PlaySound("WalkingBattle");
    }

    public void SetCorrectPosition()
    {
        Vector3 myPosition = new Vector3(transform.position.x, transform.position.y, 0);

        int x = (int)myPosition.x;
        float lastDigitX = Mathf.Abs(myPosition.x) % 10;
        int lastDigitXInt = Mathf.Abs(x) % 10;

        if (lastDigitX >= 9 && lastDigitX < 10) x -= 4;
        else if (lastDigitX >= 0 && lastDigitX < 1) x += 5;
        else
        {
            switch (lastDigitXInt)
            {
                case 8:
                    x -= 3;
                    break;
                case 7:
                    x -= 2;
                    break;
                case 6:
                    x -= 1;
                    break;
                case 5:
                    x -= 0;
                    break;
                case 4:
                    x += 1;
                    break;
                case 3:
                    x += 2;
                    break;
                case 2:
                    x += 3;
                    break;
                case 1:
                    x += 4;
                    break;
            }
        }

        int y = (int)myPosition.y;
        float lastDigitY = Mathf.Abs(myPosition.y) % 10;
        int lastDigitYInt = Mathf.Abs(y) % 10;

        if (lastDigitY >= 9 && lastDigitY < 10) y -= 4;
        else if (lastDigitY >= 0 && lastDigitY < 1) y += 5;
        else
        {
            switch (lastDigitYInt)
            {
                case 8:
                    y -= 3;
                    break;
                case 7:
                    y -= 2;
                    break;
                case 6:
                    y -= 1;
                    break;
                case 5:
                    y -= 0;
                    break;
                case 4:
                    y += 1;
                    break;
                case 3:
                    y += 2;
                    break;
                case 2:
                    y += 3;
                    break;
                case 1:
                    y += 4;
                    break;
            }
        }

        Vector3 newPosition = new Vector3(x, y, 0);
        transform.position = newPosition;
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

    public void FireDamage()
    {
        if (fireBurstIndex == 0)
        {
            fireDamage = 21;
        }
        else if(fireBurstIndex > 0)
        {
            fireDamage = 8;
        }
        fireBurstIndex++;
        healthSystem.Damage(fireDamage);
        StartCoroutine(FireDamageFeedback());
    }

    public void BurnDamage()
    {
        burnDamage = 8;
        burningIndex++;
        healthSystem.Damage(burnDamage);
        StartCoroutine(FireDamageFeedback());
    }

    private IEnumerator FireDamageFeedback()
    {
        //SoundManager.PlaySound("burst");
        SoundManager.PlaySound("Fire");
        playerSprite.color = Color.red;
        if (healthSystem.IsDead())
        {
            imDead = true;
            sceneCombatSystem.CheckIfDead();
            if (GetTeam() == Team.Blue)
            {
                for (int i = 0; i < sceneCombatSystem.alliesTeamList.Count; i++)
                {
                    if (!sceneCombatSystem.alliesTeamList[i].imDead)
                        sceneCombatSystem.newAlliesTeamList.Add(sceneCombatSystem.alliesTeamList[i]);
                }
                CleanListAlly();
            }
            else if(GetTeam() == Team.Red) 
            { 
                characterManager.mageExp += 5;
                for (int i = 0; i < sceneCombatSystem.enemiesTeamList.Count; i++)
                {
                    if (!sceneCombatSystem.enemiesTeamList[i].imDead)
                        sceneCombatSystem.newEnemiesTeamList.Add(sceneCombatSystem.enemiesTeamList[i]);
                }
                CleanListIA();
            }
        }
        yield return new WaitForSeconds(0.7f);
        playerSprite.color = Color.white;
        if (imDead) Destroy(gameObject);
        sceneCombatSystem.CheckIfGameIsOver();
    }

    int attackID;
    public void Damage(UnitGridCombat Attacker){

        if(Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            attackID = 1;

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
                if (sceneCombatSystem.justicesExecute) damageAmount = 80;
                else damageAmount = 35.0f;
            }

        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED)
        {
            attackID = 2;

            attackedByMelee = false;
            attackedByArcher = true;
            attackedByHealer = false;
            attackedByTank = false;
            attackedByMage = false;
            if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
            {
                if (sceneCombatSystem.archer2Syn) damageAmount += 3;
                if (sceneCombatSystem.inspiredAttack) damageAmount = 20.0f + ((20.0f / 100.0f) * 20.0f);
                else damageAmount = 20.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL2)
            {
                if (sceneCombatSystem.archer2Syn) damageAmount += 3;
                if (sceneCombatSystem.inspiredAttack) damageAmount = 25.0f + ((25.0f / 100.0f) * 20.0f);
                else damageAmount = 25.0f;
            }
            else if (Attacker.GetComponent<CHARACTER_PREFS>().Getlevel() == CHARACTER_PREFS.Level.NIVEL3)
            {
                if (sceneCombatSystem.archer2Syn) damageAmount += 3;
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
            attackID = 3;

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
        dmg = RandomDamage(damageAmount, Attacker);
        sceneCombatSystem.DamagePopUp(this.GetPosition(), (int)dmg);
        healthSystem.Damage((int)dmg);
        if ((Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED) && (sceneCombatSystem.archer4Syn && sceneCombatSystem.enemiesTeamList.Count > 1))
        {
            if (Attacker.GetTeam() == Team.Blue)
            {
                UnitGridCombat newObjective = LookForClosestUnit(this);
                sceneCombatSystem.DamagePopUp(newObjective.GetPosition(), (int)dmg);
                dmg = dmg / 2;
                newObjective.healthSystem.Damage((int)dmg);
                StartCoroutine(FeedbackAttack(newObjective));
            }
        }
        if ((this.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK) && (sceneCombatSystem.tank4Syn))
        {
            if (Attacker.GetTeam() == Team.Red)
            {
                dmg = ((25 * dmg) / 100.0f);
                sceneCombatSystem.DamagePopUp(Attacker.GetPosition(), (int)dmg);
                Attacker.healthSystem.Damage((int)dmg);
            }
        }
        if ((Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER) && (sceneCombatSystem.healer4Syn))
        {
            if (Attacker.GetTeam() == Team.Blue)
            {
                for (int i = 0; i < sceneCombatSystem.enemiesTeamList.Count; i++)
                {
                    UnitGridCombat enemyToAttack = sceneCombatSystem.enemiesTeamList[i];
                    sceneCombatSystem.DamagePopUp(enemyToAttack.GetPosition(), (int)dmg);
                    enemyToAttack.healthSystem.Damage((int)dmg);
                    enemyToAttack.StartCoroutine(FeedbackAttack(enemyToAttack));
                }
                SoundManager.PlaySound("HealerBasicAttack");
            }
        }

            if (healthSystem.IsDead()){
            if(Attacker.GetTeam() == Team.Blue) 
            {
                if(characterPrefs.Getlevel() == CHARACTER_PREFS.Level.NIVEL1)
                {
                    if (attackedByMelee)
                    {
                        characterManager.meleeExp += 5;
                        sceneCombatSystem.inspiration++;
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

        if ((Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.HEALER) && (sceneCombatSystem.healer4Syn)) { }
        else
        {
            StartCoroutine(FeedbackAttack(Attacker));
        }
    }

    float RandomDamage(float damageAmount, UnitGridCombat Attacker)
    {
        int randomNum = UnityEngine.Random.Range(-2, 2);
        damageAmount = damageAmount + randomNum;
        randomNum = UnityEngine.Random.Range(0, 100);
        if (randomNum <= 5)
        {
            damageAmount = damageAmount + (damageAmount / 100.0f) * 20.0f;
            //feedback
            attackID = 5;
        }
        else if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE && sceneCombatSystem.melee2Syn)
        {
            if (randomNum <= 10)
            {
                damageAmount = damageAmount + (damageAmount / 100.0f) * 20.0f;
                //feedback
                attackID = 5;
            }
        }
        else if(randomNum > 95)
        {
            if (Attacker.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED && sceneCombatSystem.archer2Syn)
            {

            }
            else
            {
                damageAmount = 0;
                //feedback
                attackID = 6;
            }
        }

        if(this.GetComponent<CHARACTER_PREFS>().getType() == CHARACTER_PREFS.Tipo.TANK && sceneCombatSystem.overload && isOverloaded)
        {
            damageAmount = damageAmount - ((damageAmount / 100.0f) * 75.0f);
        }
        else
        {
            damageAmount = damageAmount - ((damageAmount / 100.0f) * defense);
        }

        if (sceneCombatSystem.boltOfPrecision)
        {
            damageAmount = damageAmount + ((damageAmount / 100.0f) * 40.0f);
        }

        if(sceneCombatSystem.tank2Syn)
        {
            if (Attacker.GetTeam() == Team.Red)
            {
                damageAmount = damageAmount - ((15 * damageAmount) / 100.0f);
            }
        }

        if(sceneCombatSystem.mage4Syn)
        {
            if (Attacker.GetTeam() == Team.Blue)
            {
                if (randomNum >= 50 || burning)
                {
                    burning = true;
                    Debug.Log(this + "Ardiendo");
                    StartCoroutine(FireDamageFeedback());
                }
            }
        }

        return damageAmount;
    }

    private IEnumerator FeedbackAttack(UnitGridCombat Attacker)
    {
        if (attackedByMelee || attackedByTank)
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

        else if (attackedByArcher && !sceneCombatSystem.boltOfPrecision)
        {
            animEnded = false;

            //LOGICA
            //Cambiamos los 2 primeros puntos de la curva al centro del arquero y el otro un poco mas arriba
            //Cambiamos los 2 ultimos puntos de la curva al centro del objetivo y el otro un poco mas arriba
            //Instanciamos una flecha en el inicio de la curva
            attackerPosArrow = Attacker.GetPosition();
            myPosArrow = this.GetPosition();
            //Pos0
            p0 = sceneCombatSystem.arrowRoute.GetChild(0).position;
            p0.x = attackerPosArrow.x;
            p0.y = attackerPosArrow.y;
            sceneCombatSystem.arrowRoute.GetChild(0).position = p0;
            //Pos1
            p1 = sceneCombatSystem.arrowRoute.GetChild(1).position;
            p1.x = attackerPosArrow.x;
            p1.y = attackerPosArrow.y + 3;
            sceneCombatSystem.arrowRoute.GetChild(1).position = p1;
            //Pos2
            p2 = sceneCombatSystem.arrowRoute.GetChild(2).position;
            p2.x = myPosArrow.x;
            p2.y = myPosArrow.y + 3;
            sceneCombatSystem.arrowRoute.GetChild(2).position = p2;
            //Pos3
            p3 = sceneCombatSystem.arrowRoute.GetChild(3).position;
            p3.x = myPosArrow.x;
            p3.y = myPosArrow.y;
            sceneCombatSystem.arrowRoute.GetChild(3).position = p3;

            sceneCombatSystem.arrowPrefab.GetComponent<BezierFollow>().route = sceneCombatSystem.arrowRoute;
            Instantiate(sceneCombatSystem.arrowPrefab, attackerPosArrow, Quaternion.Euler(new Vector3(0, 0, 90)));

            SoundManager.PlaySound("ArrowHit");
            //Seguimos con sonido y feedback
            yield return new WaitForSeconds(0.1f);
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            playerSprite.color = Color.white;
            animEnded = true;
            if (imDead)
                Destroy(gameObject);
            sceneCombatSystem.CheckIfGameIsOver();
        }

        else if (attackedByArcher && sceneCombatSystem.boltOfPrecision)
        {
            animEnded = false;
            boltAnim.SetActive(true);
            SoundManager.PlaySound("ArrowHit");
            SoundManager.PlaySound("Bolt");
            yield return new WaitForSeconds(1.2f);
            boltAnim.SetActive(false);
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.3f);
            playerSprite.color = Color.white;
            animEnded = true;
        }

        else if (attackedByMage)
        {
            animEnded = false;
            lightningAnim.SetActive(true);
            SoundManager.PlaySound("Lightning");
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(1.2f);
            lightningAnim.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            playerSprite.color = Color.white;
            animEnded = true;
            if (imDead)
                Destroy(gameObject);
            sceneCombatSystem.CheckIfGameIsOver();
        }

        else if (attackedByHealer && !sceneCombatSystem.healer4Syn)
        {
            animEnded = false;
            explosionAnim.SetActive(true);
            SoundManager.PlaySound("HealerBasicAttack");
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.9f);
            explosionAnim.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            playerSprite.color = Color.white;
            animEnded = true;
            if (imDead)
                Destroy(gameObject);
            sceneCombatSystem.CheckIfGameIsOver();
        }

        else if (attackedByHealer && sceneCombatSystem.healer4Syn)
        {
            Attacker.animEnded = false;
            Attacker.explosionAnim.SetActive(true);
            //SoundManager.PlaySound("HealerBasicAttack");
            Attacker.playerSprite.color = Color.red;
            yield return new WaitForSeconds(0.9f);
            Attacker.explosionAnim.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            Attacker.playerSprite.color = Color.white;
            Attacker.animEnded = true;
            if (Attacker.imDead)
                Destroy(Attacker.gameObject);
            sceneCombatSystem.CheckIfGameIsOver();
        }
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

    public void DoOverloadFeedback()
    {
        StartCoroutine(FeedbackOverload());
    }

    private UnitGridCombat LookForClosestUnit(UnitGridCombat thisUnit)
    {
        int enemiesCount = sceneCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList.Count;
        Vector3 myPosition = thisUnit.GetPosition();
        float minDistance = 9999;
        float distance = 0.0f;
        UnitGridCombat temporal = null;
        
        for (int i = 0; i < enemiesCount; i++)
        {
            distance = Vector3.Distance(myPosition, sceneCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i].GetPosition());
            if (minDistance > distance)
            {
                temporal = sceneCombatSystem.GetComponent<GridCombatSystem>().enemiesTeamList[i];
            }
        }
        return temporal;
    }

    public IEnumerator FeedbackOverload()
    {
        animEnded = false;
        overloadAnim.SetActive(true);
        //SoundManager.PlaySound("Healing");
        yield return new WaitForSeconds(1.6f);
        overloadAnim.SetActive(false);
        playerSprite.color = Color.blue;
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
        if (sceneCombatSystem.healer2Syn && GetTeam() == Team.Blue) Heal(5);
        if (sceneCombatSystem.hexOfNature)
        {
            unitGridCombat.Heal(60);
            sceneCombatSystem.hexOfNature = false;
            sceneCombatSystem.inspiration -= 3;
        }
        else if (sceneCombatSystem.divineGrace)
        {
            for (int i = 0; i < sceneCombatSystem.alliesTeamList.Count; i++)
            {
                sceneCombatSystem.alliesTeamList[i].Heal(healAmount);
            }
            sceneCombatSystem.divineGrace = false;
            sceneCombatSystem.inspiration -= 4;
        }
        else unitGridCombat.Heal(healAmount);
    }

    public void Heal(int healAmount)
    {
        StartCoroutine(FeedbackHealing(healAmount));
    }

    public bool CanAttackUnit(UnitGridCombat unitGridCombat) {
        if(gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.MELEE)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeMelee;
        }
        else if (gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.RANGED && sceneCombatSystem.boltOfPrecision)
        {
            return true;
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
        else if (gameObject.GetComponent<CHARACTER_PREFS>().tipo == CHARACTER_PREFS.Tipo.TANK)
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= attackRangeTank;
        }
        else //DUMMY
        {
            return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) <= 0;
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
