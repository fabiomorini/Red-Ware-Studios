using System;
using UnityEngine;

public class CHARACTER_PREFS : MonoBehaviour {

    [HideInInspector]
    public int level;

    /*
     tipo
     nivel
     armadura
     arma
    */

    public enum tipos
    {
        melee,
        ranged, 
        healer
    }
    public Vector3 GetPosition() {
        return transform.position;
    }
}
