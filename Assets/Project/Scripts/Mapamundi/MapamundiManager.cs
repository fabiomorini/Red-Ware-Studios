using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapamundiManager : MonoBehaviour
{
    private GameObject CombatHandler;
    public GameObject UI;
    public Animator animator;

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
    [HideInInspector] public int maxL3 = 3;
    [HideInInspector] public int maxL4 = 4;
    [HideInInspector] public int maxL5 = 4;
    [HideInInspector] public int maxL6 = 4;
    [HideInInspector] public int maxL7 = 5;
    [HideInInspector] public int maxL8 = 5;
    [HideInInspector] public int maxL9 = 5;
    [HideInInspector] public int maxL10 = 5;


    //scroll levels
    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public GameObject point4;
    public GameObject scrollObject;
    Vector3 mousePos;
    public Slider sliderLevels;


    public GameObject circle1;
    public GameObject circle2;
    public GameObject circle3;
    public GameObject circle4;
    public GameObject circle5;
    public GameObject circle6;
    public GameObject circle7;
    public GameObject circle8;
    public GameObject circle9;
    public GameObject circle10;

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

        CheckSliderValue();

        //if (Input.GetAxis("Mouse ScrollWheel") == 0f) { CheckSliderValue(); }
        //else { CheckScrollMouse(); }
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

    public IEnumerator ReadyCheckYesRoutine()
    {
        if (N1)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel1");
        }
        else if (N2)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel2");
        }
        else if (N3)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel3");
        }
        else if (N4)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel4");
        }
        else if (N5)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel5");
        }
        else if (N6)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel6");
        }
        else if (N7)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel7");
        }
        else if (N8)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel8");
        }
        else if (N9)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel9");
        }
        else if (N10)
        {
            UIReadyCheck.SetActive(false);
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("Nivel10");
        }
    }

    public void ReadyCheckYes()
    {
        StartCoroutine(ReadyCheckYesRoutine());
    }

    public void ReadyCheckNo()
    {
        UIReadyCheck.SetActive(false);
    }

    public void HoverL1Enter()
    {
        Nv1.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle1.SetActive(true);
    }

    public void HoverL1Exit()
    {
        Nv1.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle1.SetActive(false);
    }

    public void HoverL2Enter()
    {
        Nv2.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle2.SetActive(true);
    }

    public void HoverL2Exit()
    {
        Nv2.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle2.SetActive(false);
    }

    public void HoverL3Enter()
    {
        Nv3.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle3.SetActive(true);
    }

    public void HoverL3Exit()
    {
        Nv3.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle3.SetActive(false);
    }

    public void HoverL4Enter()
    {
        Nv4.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle4.SetActive(true);
    }

    public void HoverL4Exit()
    {
        Nv4.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle4.SetActive(false);
    }

    public void HoverL5Enter()
    {
        Nv5.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle5.SetActive(true);
    }

    public void HoverL5Exit()
    {
        Nv5.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle5.SetActive(false);
    }

    public void HoverL6Enter()
    {
        Nv6.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle6.SetActive(true);
    }

    public void HoverL6Exit()
    {
        Nv6.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle6.SetActive(false);
    }

    public void HoverL7Enter()
    {
        Nv7.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle7.SetActive(true);
    }

    public void HoverL7Exit()
    {
        Nv7.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle7.SetActive(false);
    }

    public void HoverL8Enter()
    {
        Nv8.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle8.SetActive(true);
    }

    public void HoverL8Exit()
    {
        Nv8.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle8.SetActive(false);
    }

    public void HoverL9Enter()
    {
        Nv9.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle9.SetActive(true);
    }

    public void HoverL9Exit()
    {
        Nv9.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle9.SetActive(false);
    }

    public void HoverL10Enter()
    {
        Nv10.GetComponent<Image>().color = new Color32(239, 73, 107, 130);
        circle10.SetActive(true);
    }

    public void HoverL10Exit()
    {
        Nv10.GetComponent<Image>().color = new Color32(239, 73, 107, 255);
        circle10.SetActive(false);
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
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = false;

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
        N4 = false;
        N5 = false;
        N6 = false;
        N7 = false;
        N8 = false;
        N9 = false;
        N10 = false;

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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL4)
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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL5)
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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL6)
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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL7)
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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL8)
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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL9)
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
            if (CombatHandler.GetComponent<CHARACTER_MNG>().numberOfAllies > maxL10)
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
