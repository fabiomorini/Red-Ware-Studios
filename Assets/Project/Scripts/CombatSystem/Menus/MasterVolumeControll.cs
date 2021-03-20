using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeControll : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float masterVolume = 1.0f;

    //Update is called once per frame
    void Update()
    {
        AudioListener.volume = masterVolume;
    }

    // Method that is called by slider game object. 
    //This method takes vol value passed by slider and sets it as musicValue
    public void SetVolume(float vol)
    {
        masterVolume = vol;
    }
}