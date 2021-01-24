using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public GameObject boundaryL1;
    public GameObject boundaryL2;
    public GameObject boundaryL3;
    [HideInInspector] public bool L1;
    [HideInInspector] public bool L2;
    [HideInInspector] public bool L3;
    //public GameObject player;
    //bool hasSpawned = false;
    //public GameObject CinemachineFollow;
    private CHARACTER_MNG characterManager;

    private void Update()
    {
        characterManager = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>();
        //SpawnPlayer();
        SetBoundariesFalse();
        //CinemachineFollow.transform.position = player.transform.position;
        //Debug.Log(CinemachineFollow.transform.position);
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

    /*private void SpawnPlayer()
    {
        
        if (!hasSpawned)
        {
            CinemachineFollow.transform.position = new Vector3(characterManager.GetComponent<CHARACTER_MNG>().playerPosition.position.x, 
                                                               characterManager.GetComponent<CHARACTER_MNG>().playerPosition.position.y, 0);
            Instantiate(player, new Vector3(CinemachineFollow.transform.position.x, CinemachineFollow.transform.position.y, 0),
                                            Quaternion.identity);

            hasSpawned = true;
        }
    }*/
}
