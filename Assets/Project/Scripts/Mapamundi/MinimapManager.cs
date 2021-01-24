using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public GameObject boundaryL1;
    public GameObject boundaryL2;
    public GameObject boundaryL3;

    private CHARACTER_MNG characterManager;

    private void Update()
    {
        characterManager = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>();
        SetBoundariesFalse();
    }
    private void SetBoundariesFalse()
    {
        if (characterManager.VictoryL1)
        {
            boundaryL1.SetActive(false);
        }
        if (characterManager.VictoryL2)
        {
            boundaryL2.SetActive(false);
        }
        if (characterManager.VictoryL3)
        {
            boundaryL3.SetActive(false);
        }
    }
}
