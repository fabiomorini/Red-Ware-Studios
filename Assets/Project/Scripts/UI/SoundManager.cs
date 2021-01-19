using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //MENU
    public AudioClip hoverMenuSound, clickMenuSound;
    //MINIMAPA
    public AudioClip ambientSound, walkingSound, openCuartelSound, closeCuartelSound, buyAllySound, zeroMoneySound, levelSoundSound, playLevelSound;
    //NIVEL
    //  -MINIMENU
    public AudioClip hoverButtonSound, clickButtonSound, openMenuSound, closeMenuSound;
    //  -BATALLA
    public AudioClip attackSound, attackDeathSound, walkingBattleSound, turnSwapSound;
    //  -ENDMENU
    public AudioClip victorySound, lostSound;

    static AudioSource audioSrc;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        hoverMenuSound = Resources.Load<AudioClip>("hoverMenu");
        clickMenuSound = Resources.Load<AudioClip>("clickMenu");
        ambientSound = Resources.Load<AudioClip>("ambient");
        walkingSound = Resources.Load<AudioClip>("walking");
        openCuartelSound = Resources.Load<AudioClip>("openCuartel");
        closeCuartelSound = Resources.Load<AudioClip>("closeCuartel");
        buyAllySound = Resources.Load<AudioClip>("buyAlly");
        levelSoundSound = Resources.Load<AudioClip>("levelSound");
        playLevelSound = Resources.Load<AudioClip>("playLevel");
        hoverButtonSound = Resources.Load<AudioClip>("hoverButton");
        clickButtonSound = Resources.Load<AudioClip>("clickButton");
        openMenuSound = Resources.Load<AudioClip>("openMenu");
        closeMenuSound = Resources.Load<AudioClip>("closeMenu");
        attackSound = Resources.Load<AudioClip>("attack");
        attackDeathSound = Resources.Load<AudioClip>("attackDeath");
        walkingBattleSound = Resources.Load<AudioClip>("walkingBattle");
        turnSwapSound = Resources.Load<AudioClip>("turnSwap");
        victorySound = Resources.Load<AudioClip>("victory");
        lostSound = Resources.Load<AudioClip>("lost");

        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    public void PlaySound (string clip)
    {
        switch (clip)
        {
            case "hoverMenu":
                audioSrc.PlayOneShot(hoverMenuSound);
                break;
            case "clickMenu":
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
            case "levelSound":
                audioSrc.PlayOneShot(levelSoundSound);
                break;
            case "playLevel":
                audioSrc.PlayOneShot(playLevelSound);
                break;
            case "hoverButton":
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
            case "attack":
                audioSrc.PlayOneShot(attackSound);
                break;
            case "attackDeath":
                audioSrc.PlayOneShot(attackDeathSound);
                break;
            case "walkingBattle":
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
        }
    }



}
