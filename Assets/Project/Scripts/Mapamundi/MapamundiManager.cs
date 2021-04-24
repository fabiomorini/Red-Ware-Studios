using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapamundiManager : MonoBehaviour
{
    private GameObject CombatHandler;
    public GameObject UI;

    [HideInInspector] public bool N1;
    [HideInInspector] public bool N2;
    [HideInInspector] public bool N3;
    [HideInInspector] public bool N4;
    [HideInInspector] public bool N5;
    [HideInInspector] public bool N6;
    [HideInInspector] public bool N7;
    [HideInInspector] public bool N8;
    [HideInInspector] public bool N9;
    [HideInInspector] public bool N10;

    public Button Nv1;
    public Button Nv2;
    public Button Nv3;
    public Button Nv4;
    public Button Nv5;
    public Button Nv6;
    public Button Nv7;
    public Button Nv8;
    public Button Nv9;
    public Button Nv10;

    public Button ReadyToBattle;
    public Button NotReadyToBattle;
    public GameObject UIReadyCheck;

    [HideInInspector] public int maxL1 = 3;
    [HideInInspector] public int maxL2 = 3;
    [HideInInspector] public int maxL3 = 4;
    [HideInInspector] public int maxL4 = 5;
    [HideInInspector] public int maxL5 = 6;
    [HideInInspector] public int maxL6 = 7;
    [HideInInspector] public int maxL7 = 7;
    [HideInInspector] public int maxL8 = 7;
    [HideInInspector] public int maxL9 = 7;
    [HideInInspector] public int maxL10 = 7;


    //scroll levels
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public GameObject point4;
    public GameObject scrollObject;
    Vector3 mousePos;
    public Slider sliderLevels;

    private void Start()
    {
        Nv1.interactable = true;
        Nv2.interactable = true;
        Nv3.interactable = true;
        Nv4.interactable = true;
        Nv5.interactable = true;
        Nv6.interactable = true;
        Nv7.interactable = true;
        Nv8.interactable = true;
        Nv9.interactable = true;
        Nv10.interactable = true;
        sliderLevels.maxValue = scrollObject.transform.position.y + 398 + 60;
        sliderLevels.minValue = scrollObject.transform.position.y;
        sliderLevels.value = scrollObject.transform.position.y;
    }

    private void Update()
    {
        
        CombatHandler = GameObject.FindWithTag("characterManager");
        CombatHandler.GetComponent<CHARACTER_MNG>().NumOfAllies();
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL1)
        {
            Nv2.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL2)
        {
            Nv3.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL3)
        {
            Nv4.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL4)
        {
            Nv5.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL5)
        {
            Nv6.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL6)
        {
            Nv7.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL7)
        {
            Nv8.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL8)
        {
            Nv9.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL9)
        {
            Nv10.interactable = true;
        }
        if (CombatHandler.GetComponent<CHARACTER_MNG>().VictoryL10)
        {
            //EndGame
        }

        //CheckSliderValue();

        if (Input.GetAxis("Mouse ScrollWheel") == 0f) { CheckSliderValue(); }
        else { CheckScrollMouse(); }
    }
    private void CheckSliderValue()
    {
        scrollObject.transform.position = new Vector3(scrollObject.transform.position.x, sliderLevels.value, 0);
    }

    public void CheckScrollMouse()
    {
        mousePos = Input.mousePosition;
        if (mousePos.x > point1.transform.position.x && mousePos.x < point2.transform.position.x && mousePos.y < point1.transform.position.y && mousePos.y > point2.transform.position.y)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f && point4.transform.position.y + scrollObject.transform.position.y < 1380.0f) // forward
            {
                scrollObject.transform.Translate(Vector3.up * 20);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && point3.transform.position.y + scrollObject.transform.position.y > 1330.0f) // backwards
            {
                scrollObject.transform.Translate(Vector3.down * 20);
            }
        }
        sliderLevels.value = scrollObject.transform.position.y;
    }

    public void ReadyCheckYes()
    {
        if (N1)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel1");
        }
        else if (N2)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel2");
        }
        else if (N3)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel3");
        }
        else if (N4)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel4");
        }
        else if (N5)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel5");
        }
        else if (N6)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel6");
        }
        else if (N7)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel7");
        }
        else if (N8)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel8");
        }
        else if (N9)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel9");
        }
        else if (N10)
        {
            UIReadyCheck.SetActive(false);
            SceneManager.LoadScene("Nivel10");
        }
    }

    public void ReadyCheckNo()
    {
        UIReadyCheck.SetActive(false);
    }

    public void Nivel1()
    {
        N1 = true;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel2()
    {
        N1 = false;
        N2 = true;
        N3 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL2)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel2");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel3()
    {
        N1 = false;
        N2 = false;
        N3 = true;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL3)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel3");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel4()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = true;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel5()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = true;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel6()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = false;
        N6 = true;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel7()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = true;
        N8 = false;
        N9 = false;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel8()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = true;
        N9 = false;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel9()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = true;
        N10 = false;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }

    public void Nivel10()
    {
        N1 = false;
        N2 = false;
        N3 = false;
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = true;

        if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies >= 1)
        {
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL1)
            {
                //Menu de Seleccion de Personaje
                UI.SetActive(true);
            }
            else
            {
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMeleeFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMelee;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfArcherFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfRanged;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealerFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfHealer;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTankFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfTank;
                CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMageFight = CombatHandler.GetComponent<CHARACTER_MNG>().numberOfMage;
                //SceneManager.LoadScene("Nivel1");
                UIReadyCheck.SetActive(true);
            }
        }
    }
}
