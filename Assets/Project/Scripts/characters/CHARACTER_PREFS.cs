using System;
using UnityEngine;

public class CHARACTER_PREFS : MonoBehaviour {

    [HideInInspector]
    public int level;
    public Tipo tipo;
    /*
     nivel
     armadura
     arma
    */

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
