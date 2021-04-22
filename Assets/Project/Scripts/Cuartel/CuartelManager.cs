using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CuartelManager : MonoBehaviour
{
    private int knightCounter = 0;
    private int ArcherCounter = 0;
    private int HealerCounter = 0;
    private int TankCounter = 0;
    private int MageCounter = 0;
    [SerializeField] private int knightPrice = 100;
    [SerializeField] private int archerPrice = 130;
    [SerializeField] private int healerPrice = 140;
    [SerializeField] private int tankPrice = 150;
    [SerializeField] private int magePrice = 160;

    public TMP_Text coinsText;
    public TMP_Text KnightText;
    public TMP_Text ArcherText;
    public TMP_Text HealerText;
    public TMP_Text TankText;
    public TMP_Text MageText;

    public GameObject cuartel;
    public GameObject infoUI;

    public GameObject characterManager;
    private CHARACTER_MNG charManager;

    //Animation
    public Animator CuartelAnimator;

    private Soldier soldier;
    private Ability ability;
    private Window window;
    public TMP_Text soldierText;

    public GameObject knightImage;
    public GameObject ArcherImage;
    public GameObject HealerImage;
    public GameObject TankImage;
    public GameObject MageImage;

    public GameObject knightArrow;
    public GameObject ArcherArrow;
    public GameObject HealerArrow;
    public GameObject TankArrow;
    public GameObject MageArrow;

    public TMP_Text price;
    public TMP_Text abilityName;
    public TMP_Text abilityDesc;

    public TMP_Text Health;
    public TMP_Text Armour;
    public TMP_Text Attack;
    public TMP_Text Crit;
    public TMP_Text AttackRange;

    public TMP_Text levelText;
    public TMP_Text expText;

    public TMP_Text sinergy;
    public GameObject StatWindow;
    public GameObject ExpWindow;
    public GameObject SynWindow;

    private enum Soldier { KNIGHT, ARCHER, HEALER, PALADIN, WIZARD };
    private enum Ability { ONE, TWO };
    private enum Window { STAT, EXP, SINERGY };

    private void Start()
    {
        if (GameObject.FindWithTag("characterManager") == null)
        {
            Instantiate(characterManager);
        }
        characterManager = GameObject.FindWithTag("characterManager");
        charManager = characterManager.GetComponent<CHARACTER_MNG>();
        knightCounter = charManager.numberOfMelee;
        ArcherCounter = charManager.numberOfRanged;
        HealerCounter = charManager.numberOfHealer;
        TankCounter = charManager.numberOfTank;
        MageCounter = charManager.numberOfMage;

        soldier = Soldier.KNIGHT;
        price.SetText("" + knightPrice);
        window = Window.STAT;

        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
        HealerText.SetText("x " + HealerCounter);
        TankText.SetText("x " + TankCounter);
        MageText.SetText("x " + MageCounter);

        SetExpText();
        SetAbilityText();
        coinsText.SetText(charManager.coins + "");
        SetWindow();
    }

    private void Update()
    {
        SetAbilityText();
        SetWindow();
    }

    public void UseQuartel() 
    {
        StartCoroutine(OpenAnimCuartel());
    }

    private IEnumerator OpenAnimCuartel()
    {
        CuartelAnimator.Play("transitionCuartel");
        SoundManager.PlaySound("openCuartel");
        yield return new WaitForSeconds(1.60f);
        cuartel.SetActive(true);
    }

    public void CloseQuartel()
    {
        CuartelAnimator.Play("closeCuartel");
        SoundManager.PlaySound("closeCuartel");
        cuartel.SetActive(false);
    }

    //New UI Cuartel
    public void SetKnight()
    {
        soldier = Soldier.KNIGHT;
        soldierText.SetText("Knight");

        knightImage.SetActive(true);
        ArcherImage.SetActive(false);
        HealerImage.SetActive(false);
        TankImage.SetActive(false);
        MageImage.SetActive(false);

        price.SetText(""+ knightPrice);

        knightArrow.SetActive(true);
        ArcherArrow.SetActive(false);
        HealerArrow.SetActive(false);
        TankArrow.SetActive(false);
        MageArrow.SetActive(false);
    }
    public void SetArcher()
    {
        soldier = Soldier.ARCHER;
        soldierText.SetText("Archer");

        knightImage.SetActive(false);
        ArcherImage.SetActive(true);
        HealerImage.SetActive(false);
        TankImage.SetActive(false);
        MageImage.SetActive(false);

        price.SetText("" + archerPrice);

        knightArrow.SetActive(false);
        ArcherArrow.SetActive(true);
        HealerArrow.SetActive(false);
        TankArrow.SetActive(false);
        MageArrow.SetActive(false);
    }
    public void SetHealer()
    {
        soldier = Soldier.HEALER;
        soldierText.SetText("Priest");

        knightImage.SetActive(false);
        ArcherImage.SetActive(false);
        HealerImage.SetActive(true);
        TankImage.SetActive(false);
        MageImage.SetActive(false);

        price.SetText("" + healerPrice);

        knightArrow.SetActive(false);
        ArcherArrow.SetActive(false);
        HealerArrow.SetActive(true);
        TankArrow.SetActive(false);
        MageArrow.SetActive(false);
    }
    public void SetPaladin()
    {
        soldier = Soldier.PALADIN;
        soldierText.SetText("Paladin");

        knightImage.SetActive(false);
        ArcherImage.SetActive(false);
        HealerImage.SetActive(false);
        TankImage.SetActive(true);
        MageImage.SetActive(false);

        price.SetText("" + tankPrice);

        knightArrow.SetActive(false);
        ArcherArrow.SetActive(false);
        HealerArrow.SetActive(false);
        TankArrow.SetActive(true);
        MageArrow.SetActive(false);
    }
    public void SetWizard()
    {
        soldier = Soldier.WIZARD;
        soldierText.SetText("Wizard");

        knightImage.SetActive(false);
        ArcherImage.SetActive(false);
        HealerImage.SetActive(false);
        TankImage.SetActive(false);
        MageImage.SetActive(true);

        price.SetText("" + magePrice);

        knightArrow.SetActive(false);
        ArcherArrow.SetActive(false);
        HealerArrow.SetActive(false);
        TankArrow.SetActive(false);
        MageArrow.SetActive(true);
    }
    public void BuySoldier()
    {
        if(soldier == Soldier.KNIGHT && characterManager.GetComponent<CHARACTER_MNG>().coins >= knightPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= knightPrice;
            knightCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMelee++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
        else if (soldier == Soldier.ARCHER && characterManager.GetComponent<CHARACTER_MNG>().coins >= archerPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= archerPrice;
            ArcherCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfRanged++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
        else if (soldier == Soldier.HEALER && characterManager.GetComponent<CHARACTER_MNG>().coins >= healerPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= healerPrice;
            HealerCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfHealer++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
        else if (soldier == Soldier.PALADIN && characterManager.GetComponent<CHARACTER_MNG>().coins >= tankPrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= tankPrice;
            TankCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfTank++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
        else if (soldier == Soldier.WIZARD && characterManager.GetComponent<CHARACTER_MNG>().coins >= magePrice)
        {
            SoundManager.PlaySound("buyAlly");
            characterManager.GetComponent<CHARACTER_MNG>().coins -= magePrice;
            MageCounter++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfMage++;
            characterManager.GetComponent<CHARACTER_MNG>().numberOfAllies++;
        }
        else
        {
            //play not enough money sound
        }

        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
        HealerText.SetText("x " + HealerCounter);
        TankText.SetText("x " + TankCounter);
        MageText.SetText("x " + MageCounter);

        coinsText.SetText(charManager.coins + "");
    }

    private void SetExpText()
    {
        if(soldier == Soldier.KNIGHT)
        {
            if(charManager.meleeLevel == 1)
            {
                levelText.SetText("Level 1 / 3");
                expText.SetText(charManager.meleeExp + " / " + charManager.level2Exp + " XP");
            }
            else if (charManager.meleeLevel == 2)
            {
                levelText.SetText("Level 2 / 3");
                expText.SetText(charManager.meleeExp + " / " + charManager.level3Exp + " XP");
            }
            else if (charManager.meleeLevel == 3)
            {
                levelText.SetText("Level 3 / 3");
                expText.SetText("MAX XP");
            }
        }
        if (soldier == Soldier.ARCHER)
        {
            if (charManager.archerLevel == 1)
            {
                levelText.SetText("Level 1 / 3");
                expText.SetText(charManager.archerExp + " / " + charManager.level2Exp + " XP");
            }
            else if (charManager.archerLevel == 2)
            {
                levelText.SetText("Level 2 / 3");
                expText.SetText(charManager.archerExp + " / " + charManager.level3Exp + " XP");
            }
            else if (charManager.archerLevel == 3)
            {
                levelText.SetText("Level 3 / 3");
                expText.SetText("MAX XP");
            }
        }
        if (soldier == Soldier.HEALER)
        {
            if (charManager.healerLevel == 1)
            {
                levelText.SetText("Level 1 / 3");
                expText.SetText(charManager.healerExp + " / " + charManager.level2Exp + " XP");
            }
            else if (charManager.healerLevel == 2)
            {
                levelText.SetText("Level 2 / 3");
                expText.SetText(charManager.healerExp + " / " + charManager.level3Exp + " XP");
            }
            else if (charManager.healerLevel == 3)
            {
                levelText.SetText("Level 3 / 3");
                expText.SetText("MAX XP");
            }
        }
        if (soldier == Soldier.PALADIN)
        {
            if (charManager.tankLevel == 1)
            {
                levelText.SetText("Level 1 / 3");
                expText.SetText(charManager.tankExp + " / " + charManager.level2Exp + " XP");
            }
            else if (charManager.tankLevel == 2)
            {
                levelText.SetText("Level 2 / 3");
                expText.SetText(charManager.tankExp + " / " + charManager.level3Exp + " XP");
            }
            else if (charManager.tankLevel == 3)
            {
                levelText.SetText("Level 3 / 3");
                expText.SetText("MAX XP");
            }
        }
        if (soldier == Soldier.WIZARD)
        {
            if (charManager.mageLevel == 1)
            {
                levelText.SetText("Level 1 / 3");
                expText.SetText(charManager.mageExp + " / " + charManager.level2Exp + " XP");
            }
            else if (charManager.mageLevel == 2)
            {
                levelText.SetText("Level 2 / 3");
                expText.SetText(charManager.mageExp + " / " + charManager.level3Exp + " XP");
            }
            else if (charManager.mageLevel == 3)
            {
                levelText.SetText("Level 3 / 3");
                expText.SetText("MAX XP");
            }
        }
    }

    private void SetWindow()
    {
        if(window == Window.EXP)
        {
            SetExpText();
            StatWindow.SetActive(false);
            ExpWindow.SetActive(true);
            SynWindow.SetActive(false);
        }
        else if(window == Window.STAT)
        {
            SetStatText();
            StatWindow.SetActive(true);
            ExpWindow.SetActive(false);
            SynWindow.SetActive(false);
        }
        else if(window == Window.SINERGY)
        {
            SetSinergyText();
            StatWindow.SetActive(false);
            ExpWindow.SetActive(false);
            SynWindow.SetActive(true);
        }
    }

    private void SetStatText()
    {
        if (soldier == Soldier.KNIGHT)
        {
            Health.SetText("Health: 80/85/90");
            Armour.SetText("Armour: 15/17/20");
            Attack.SetText("Attack: 25/30/35");
            Crit.SetText("Crit Rate: 5%");
            AttackRange.SetText("Attack range: 1");
        }
        if (soldier == Soldier.ARCHER)
        {
            Health.SetText("Health: 60/65/70");
            Armour.SetText("Armour: 10/12/15");
            Attack.SetText("Attack: 20/25/30");
            Crit.SetText("Crit Rate: 5%");
            AttackRange.SetText("Attack range: 4");
        }
        if (soldier == Soldier.HEALER)
        {
            Health.SetText("Health: 50/55/60");
            Armour.SetText("Armour: 5/7/10");
            Attack.SetText("Attack: 10/15/20");
            Crit.SetText("Crit Rate: 5%");
            AttackRange.SetText("Attack range: 3");
        }
        if (soldier == Soldier.PALADIN)
        {
            Health.SetText("Health: 85/90/95");
            Armour.SetText("Armour: 25/27/30");
            Attack.SetText("Attack: 15/20/25");
            Crit.SetText("Crit Rate: 5%");
            AttackRange.SetText("Attack range: 1");
        }
        if (soldier == Soldier.WIZARD)
        {
            Health.SetText("Health: 50/55/60");
            Armour.SetText("Armour: 10/12/15");
            Attack.SetText("Attack: 25/30/35");
            Crit.SetText("Crit Rate: 5%");
            AttackRange.SetText("Attack range: 3");
        }
    }

    private void SetSinergyText()
    {
        if (soldier == Soldier.KNIGHT)
        {
            sinergy.SetText("(2) - Increase crit rate by 5% \n\n(4) - For each opponent killed by a knight, you gain 1 inspiration point");
        }
        if (soldier == Soldier.ARCHER)
        {
            sinergy.SetText("(2) - Archers won't miss and their damage increases by 3 \n\n(4) - The attacks will ricochet to the nearest enemy with -50% damage");
        }
        if (soldier == Soldier.HEALER)
        {
            sinergy.SetText("(2) - Each healing will also restore 5 hp to the priest \n\n(4) - Every attack will damage all opponents");
        }
        if (soldier == Soldier.PALADIN)
        {
            sinergy.SetText("(2) - All the allies receive +15% armour \n\n(4) - The Paladins wear a spiked armor that reflect the 25% of the incoming damage");
        }
        if (soldier == Soldier.WIZARD)
        {
            sinergy.SetText("(2) - Increase the Fireburst duration by 2 turns \n\n(4) - The wizards gain 50% chance of burning the opponent when attacking dealing 8 damage each turn for 2 turns");
        }

    }

    public void SetWindowStat()
    {
        window = Window.STAT;
        SetWindow();
    }
    public void SetWindowExp()
    {
        window = Window.EXP;
        SetWindow();
    }
    public void SetWindowSinergy()
    {
        window = Window.SINERGY;
        SetWindow();
    }

    public void SetAbilityOne()
    {
        ability = Ability.ONE;
    }

    public void SetAbilityTwo()
    {
        ability = Ability.TWO;
    }

    public void SetAbilityText()
    {
        if(soldier == Soldier.KNIGHT && ability == Ability.ONE)
        {
            abilityName.SetText("Double Slash");
            abilityDesc.SetText("Strike the enemy with a double sword thrust with the knight's base attack damage");
        }
        else if (soldier == Soldier.KNIGHT && ability == Ability.TWO)
        {
            abilityName.SetText("Justice’s Execute");
            abilityDesc.SetText("Thrust a critic attack which deals 80 damage");
        }
        else if(soldier == Soldier.ARCHER && ability == Ability.ONE)
        {
            abilityName.SetText("Bolt of Precision");
            abilityDesc.SetText("Shoot a long range arrow that never fail dealing +40% damage");
        }
        else if (soldier == Soldier.ARCHER && ability == Ability.TWO)
        {
            abilityName.SetText("Wind Rush");
            abilityDesc.SetText("Hide from the enemies, you can move this unit wherever you want");
        }
        else if (soldier == Soldier.HEALER && ability == Ability.ONE)
        {
            abilityName.SetText("Hex of Nature");
            abilityDesc.SetText("Heal a unit in your range with an increased effect (+60 hp)");
        }
        else if (soldier == Soldier.HEALER && ability == Ability.TWO)
        {
            abilityName.SetText("Divine Grace");
            abilityDesc.SetText("This unit will heal by the base amount to each ally on the team");
        }
        else if (soldier == Soldier.PALADIN && ability == Ability.ONE)
        {
            abilityName.SetText("Overload");
            abilityDesc.SetText("The definitive tank, this unit will have it's armour increased by +75% for a turn");
        }
        else if (soldier == Soldier.PALADIN && ability == Ability.TWO)
        {
            abilityName.SetText("Whirlwind");
            abilityDesc.SetText("An attack in all four directions with a range of 1 cell that deals base damage");
        }
        else if (soldier == Soldier.WIZARD && ability == Ability.ONE)
        {
            abilityName.SetText("Fireburst");
            abilityDesc.SetText("Spawns a fire in a cell that deals 21 damage instantly and 8 damage each of the following 5 turns");
        }
        else if (soldier == Soldier.WIZARD && ability == Ability.TWO)
        {
            abilityName.SetText("Shatter");
            abilityDesc.SetText("Use all your energy and blow-up the enemy cell and the ones nearby dealing -35% damage to every opponent hit");
        }
    }
}
