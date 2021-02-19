using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fullscreen : MonoBehaviour
{
    public void FullScreen(bool is_full) 
    {
        Screen.fullScreen = is_full;

        Debug.Log("Fullscreen is " + is_full);
    }
}
