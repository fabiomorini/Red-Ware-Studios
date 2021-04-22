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

    public TMP_Text KnightTextLvl;
    public TMP_Text ArcherTextLvl;
    public TMP_Text HealerTextLvl;
    public TMP_Text TankTextLvl;
    public TMP_Text MageTextLvl;
    public TMP_Text KnightTextExp;
    public TMP_Text ArcherTextExp;
    public TMP_Text HealerTextExp;
    public TMP_Text TankTextExp;
    public TMP_Text MageTextExp;

    public GameObject cuartel;
    public GameObject infoUI;

    public GameObject characterManager;
    private CHARACTER_MNG charManager;

    //Animation
    public Animator CuartelAnimator;

    private Soldier soldier;
    private Ability ability;
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

    private enum Soldier { KNIGHT, ARCHER, HEALER, PALADIN, WIZARD };
    private enum Ability { ONE, TWO };

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

        ArcherText.SetText("x " + ArcherCounter);
        KnightText.SetText("x " + knightCounter);
        HealerText.SetText("x " + HealerCounter);
        TankText.SetText("x " + TankCounter);
        MageText.SetText("x " + MageCounter);

        SetExpText();
        SetAbilityText();
        coinsText.SetText(charManager.coins + "");
    }

    private void Update()
    {
        SetAbilityText();
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
        if (charManager.meleeLevel == 1)
        {
            KnightTextLvl.SetText("Lvl 1");
            KnightTextExp.SetText(charManager.meleeExp + " / " + charManager.level2Exp + " XP");
        }
        else if (charManager.meleeLevel == 2)
        {
            KnightTextLvl.SetText("Lvl 2");
            KnightTextExp.SetText(charManager.meleeExp + " / " + charManager.level3Exp + " XP");
        }
        else
        {
            KnightTextLvl.SetText("Lvl 3");
            KnightTextExp.SetText("MAX XP");
        }
        ///
        if (charManager.archerLevel == 1)
        {
            ArcherTextLvl.SetText("Lvl 1");
            ArcherTextExp.SetText(charManager.archerExp + " / " + charManager.level2Exp + " XP");
        }
        else if (charManager.archerLevel == 2)
        {
            ArcherTextLvl.SetText("Lvl 2");
            ArcherTextExp.SetText(charManager.archerExp + " / " + charManager.level3Exp + " XP");
        }
        else
        {
            ArcherTextLvl.SetText("Lvl 3");
            ArcherTextExp.SetText("MAX XP");
        }
        ///
        if (charManager.healerLevel == 1)
        {
            HealerTextLvl.SetText("Lvl 1");
            HealerTextExp.SetText(charManager.healerExp + " / " + charManager.level2Exp + " XP");
        }
        else if (charManager.healerLevel == 2)
        {
            HealerTextLvl.SetText("Lvl 2");
            HealerTextExp.SetText(charManager.healerExp + " / " + charManager.level3Exp + " XP");
        }
        else
        {
            HealerTextLvl.SetText("Lvl 3");
            HealerTextExp.SetText("MAX XP");
        }
        ///
        if (charManager.tankLevel == 1)
        {
            TankTextLvl.SetText("Lvl 1");
            TankTextExp.SetText(charManager.tankExp + " / " + charManager.level2Exp + " XP");
        }
        else if (charManager.tankLevel == 2)
        {
            TankTextLvl.SetText("Lvl 2");
            TankTextExp.SetText(charManager.tankExp + " / " + charManager.level3Exp + " XP");
        }
        else
        {
            TankTextLvl.SetText("Lvl 3");
            TankTextExp.SetText("MAX XP");
        }
        ///
        if (charManager.mageLevel == 1)
        {
            MageTextLvl.SetText("Lvl 1");
            MageTextExp.SetText(charManager.mageExp + " / " + charManager.level2Exp + " XP");
        }
        else if (charManager.mageLevel == 2)
        {
            MageTextLvl.SetText("Lvl 2");
            MageTextExp.SetText(charManager.mageExp + " / " + charManager.level3Exp + " XP");
        }
        else
        {
            MageTextLvl.SetText("Lvl 3");
            MageTextExp.SetText("MAX XP");
        }
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
            abilityDesc.SetText("Strike the enemy with a double sword thrust dealing 0 damage");
        }
        else if (soldier == Soldier.KNIGHT && ability == Ability.TWO)
        {
            abilityName.SetText("Justice’s Execute");
            abilityDesc.SetText("Thrust with a critic attack that ignores the opponent's armour and deals 0 damage");
        }
        else if(soldier == Soldier.ARCHER && ability == Ability.ONE)
        {
            abilityName.SetText("Bolt of Precision");
            abilityDesc.SetText("Shoot a long range arrow that never fail");
        }
        else if (soldier == Soldier.ARCHER && ability == Ability.TWO)
        {
            abilityName.SetText("Wind Rush");
            abilityDesc.SetText("Hide from the enemies, you can move this unit wherever you want");
        }
        else if (soldier == Soldier.HEALER && ability == Ability.ONE)
        {
            abilityName.SetText("Hex of Nature");
            abilityDesc.SetText("Heal a unit in your range with an increased effect (+0)");
        }
        else if (soldier == Soldier.HEALER && ability == Ability.TWO)
        {
            abilityName.SetText("Divine Grace");
            abilityDesc.SetText("This unit will heal by (0) every ally on the team");
        }
        else if (soldier == Soldier.PALADIN && ability == Ability.ONE)
        {
            abilityName.SetText("Overload");
            abilityDesc.SetText("The definitive tank, this unit will have it's armour increased by 0 for a turn");
        }
        else if (soldier == Soldier.PALADIN && ability == Ability.TWO)
        {
            abilityName.SetText("Whirlwind");
            abilityDesc.SetText("An attack in all four directions with a range of 1 cell that deals 0 damage");
        }
        else if (soldier == Soldier.WIZARD && ability == Ability.ONE)
        {
            abilityName.SetText("Fireburst");
            abilityDesc.SetText("Spawns a fire in a cell that deals 0 damage instantly and -0% damage each of the following 0 turns");
        }
        else if (soldier == Soldier.WIZARD && ability == Ability.TWO)
        {
            abilityName.SetText("Shatter");
            abilityDesc.SetText("Use all your energy and blow-up 3 cells in each direction and dealing 0 damage to every opponent hit");
        }
    }
}
