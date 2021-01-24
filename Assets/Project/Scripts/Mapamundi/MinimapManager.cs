using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapManager : MonoBehaviour
{
    public GameObject boundaryL1;
    public GameObject boundaryL2;
    public GameObject boundaryL3;
    public GameObject player;
    bool hasSpawned = false;
    public Transform CinemachineFollow;
    private CHARACTER_MNG characterManager;

    private void Update()
    {
        characterManager = GameObject.FindWithTag("characterManager").GetComponent<CHARACTER_MNG>();
        SpawnPlayer();
        SetBoundariesFalse();
        CinemachineFollow.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        Debug.Log(CinemachineFollow.position);
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

    private void SpawnPlayer()
    {
        if (!hasSpawned)
        {
            Instantiate(player);
            player.transform.position = characterManager.GetComponent<CHARACTER_MNG>().playerPosition;
            hasSpawned = true;
        }
    }
}
