﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //MENU
    public static AudioClip hoverMenuSound, clickMenuSound, returnSound, playSound;
    //MINIMAPA
    public static AudioClip ambientSound, walkingSound, openCuartelSound, closeCuartelSound, buyAllySound, zeroMoneySound, levelSoundSound, playLevelSound, openInfoSound, closeInfoSound;
    //NIVEL
    //  -MINIMENU
    public static AudioClip hoverButtonSound, clickButtonSound, openMenuSound, closeMenuSound;
    //  -BATALLA
    public static AudioClip attackSound, attackDeathSound, walkingBattleSound, turnSwapSound, healingSound, lightningSound, healerBasic, arrowHit, boltOfPrecision;
    //  -ENDMENU
    public static AudioClip victorySound, lostSound;

    static AudioSource audioSrc;

    public void Start()
    {
        hoverMenuSound = Resources.Load<AudioClip>("HoverMenu");
        clickMenuSound = Resources.Load<AudioClip>("ClickMenu");
        returnSound = Resources.Load<AudioClip>("Return");
        playSound = Resources.Load<AudioClip>("Play");
        
        ambientSound = Resources.Load<AudioClip>("Ambient");
        walkingSound = Resources.Load<AudioClip>("Walking");
        openCuartelSound = Resources.Load<AudioClip>("OpenCuartel");
        closeCuartelSound = Resources.Load<AudioClip>("CloseCuartel");
        buyAllySound = Resources.Load<AudioClip>("BuyAlly");
        levelSoundSound = Resources.Load<AudioClip>("LevelSound");
        playLevelSound = Resources.Load<AudioClip>("PlayLevel");
        openInfoSound = Resources.Load<AudioClip>("PaperOpen");
        closeInfoSound = Resources.Load<AudioClip>("PaperClosed");

        hoverButtonSound = Resources.Load<AudioClip>("HoverMainMenu");
        clickButtonSound = Resources.Load<AudioClip>("ClickButton");
        openMenuSound = Resources.Load<AudioClip>("OpenMenu");
        closeMenuSound = Resources.Load<AudioClip>("CloseMenu");

        attackSound = Resources.Load<AudioClip>("Attack");
        attackDeathSound = Resources.Load<AudioClip>("AttackDeath");
        walkingBattleSound = Resources.Load<AudioClip>("WalkingBattle");
        turnSwapSound = Resources.Load<AudioClip>("TurnSwap");
        healingSound = Resources.Load<AudioClip>("Healing");
        //magicAttackSound = Resources.Load<AudioClip>("MagicAttack");
        //fireSound = Resources.Load<AudioClip>("FireAttack");
        //arrowAttackSound = Resources.Load<AudioClip>("ArrowHit");
        //missedHitSound = Resources.Load<AudioClip>("MissedAttack");
        //missedHitSound = Resources.Load<AudioClip>("Critical");
        

        victorySound = Resources.Load<AudioClip>("Victory");
        lostSound = Resources.Load<AudioClip>("Lost");
        healingSound = Resources.Load<AudioClip>("Healing");
        lightningSound = Resources.Load<AudioClip>("Lightning");
        healerBasic = Resources.Load<AudioClip>("HealerBasicAttack");
        arrowHit = Resources.Load<AudioClip>("ArrowHit");
        boltOfPrecision = Resources.Load<AudioClip>("Bolt");

        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "Play":
                audioSrc.PlayOneShot(playSound);
                break;
            case "Return":
                audioSrc.PlayOneShot(returnSound);
                break;
            case "HoverMenu":
                audioSrc.PlayOneShot(hoverMenuSound);
                break;
            case "ClickMenu":
                audioSrc.PlayOneShot(clickMenuSound);
                break;
            case "ambient":
                audioSrc.PlayOneShot(ambientSound);
                break;
            case "walking":
                audioSrc.PlayOneShot(walkingSound);
                break;
            case "openCuartel":
                audioSrc.PlayOneShot(openCuartelSound);
                break;
            case "closeCuartel":
                audioSrc.PlayOneShot(closeCuartelSound);
                break;
            case "buyAlly":
                audioSrc.PlayOneShot(buyAllySound);
                break;
            case "PaperOpen":
                audioSrc.PlayOneShot(openInfoSound);
                break;
            case "PaperClosed":
                audioSrc.PlayOneShot(closeInfoSound);
                break;
            case "levelSound":
                audioSrc.PlayOneShot(levelSoundSound);
                break;
            case "playLevel":
                audioSrc.PlayOneShot(playLevelSound);
                break;
            case "HoverMainMenu":
                audioSrc.PlayOneShot(hoverButtonSound);
                break;
            case "clickButton":
                audioSrc.PlayOneShot(clickButtonSound);
                break;
            case "openMenu":
                audioSrc.PlayOneShot(openMenuSound);
                break;
            case "closeMenu":
                audioSrc.PlayOneShot(closeMenuSound);
                break;
            case "Attack":
                audioSrc.PlayOneShot(attackSound);
                break;
            case "AttackDeath":
                audioSrc.PlayOneShot(attackDeathSound);
                break;
            case "WalkingBattle":
                audioSrc.PlayOneShot(walkingBattleSound);
                break;
            case "turnSwap":
                audioSrc.PlayOneShot(turnSwapSound);
                break;
            case "victory":
                audioSrc.PlayOneShot(victorySound);
                break;
            case "lost":
                audioSrc.PlayOneShot(lostSound);
                break;
            case "Healing":
                audioSrc.PlayOneShot(healingSound);
                break;
            case "Lightning":
                audioSrc.PlayOneShot(lightningSound);
                break;
            case "HealerBasicAttack":
                audioSrc.PlayOneShot(healerBasic);
                break;
            case "ArrowHit":
                audioSrc.PlayOneShot(arrowHit);
                break;
            case "Bolt":
                audioSrc.PlayOneShot(boltOfPrecision);
                break;
        }
    }
}
