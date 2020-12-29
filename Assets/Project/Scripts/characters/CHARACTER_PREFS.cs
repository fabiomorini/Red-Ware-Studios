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

    [SerializeField]
    private Tipo tipo;

    public enum Tipo
    {
        melee,
        ranged, 
        healer
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Tipo getType()
    {
        return tipo;
    }
}
