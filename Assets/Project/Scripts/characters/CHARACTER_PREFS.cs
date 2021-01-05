using System;
using UnityEngine;

public class CHARACTER_PREFS : MonoBehaviour {

    [HideInInspector]
    public int level;

    /*
     nivel
     armadura
     arma
    */

    [HideInInspector]
    public Tipo tipo;

    public enum Tipo
    {
        MELEE,
        RANGED, 
        HEALER
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Tipo getType()
    {
        return tipo;
    }
}
