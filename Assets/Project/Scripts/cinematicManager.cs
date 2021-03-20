using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cinematicManager : MonoBehaviour
{

    public GameObject spaceKey;
    public GameObject loadScene;

    void Start()
    {
        StartCoroutine(ToolTipWaitTime());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceKey.SetActive(false);
            loadScene.GetComponent<FadeScenes>().PlayLevel();
        }
    }

    private IEnumerator ToolTipWaitTime()
    {
        yield return new WaitForSeconds(1f);
        spaceKey.SetActive(true);
    }
}
