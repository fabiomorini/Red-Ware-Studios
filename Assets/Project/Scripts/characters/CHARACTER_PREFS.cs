using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHARACTER_PREFS : MonoBehaviour {

    [HideInInspector] public int level;
    [SerializeField] private Tipo tipo;

    /*
     tipo
     nivel
     armadura
     arma
    */

    public enum Tipo
    {
        Melee,
        Ranged
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Tipo GetType()
    {
        return tipo;
    }
}
