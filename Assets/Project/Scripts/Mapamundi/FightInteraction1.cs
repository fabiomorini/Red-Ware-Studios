using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightInteraction1 : MonoBehaviour
{
    [SerializeField]

    private Text pressEText;
    private bool pressedCombatButton;
    private GameObject CombatHandler;

    // Use this for initialization
    private void Start()
    {
        CombatHandler = GameObject.FindWithTag("characterManager");
        pressEText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (pressedCombatButton && Input.GetKeyDown(KeyCode.E) && CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
            Combat();
        else if(CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies == 0)
        {
            Debug.Log("No tienes soldados");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pressEText.gameObject.SetActive(true);
            pressedCombatButton = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            pressEText.gameObject.SetActive(false);
            pressedCombatButton = false;
        }
    }

    private void Combat()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}