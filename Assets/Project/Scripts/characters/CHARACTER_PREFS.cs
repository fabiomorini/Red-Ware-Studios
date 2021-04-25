using System;
using UnityEngine;

public class CHARACTER_PREFS : MonoBehaviour {

    public Tipo tipo;
    public Level level;

    public enum Tipo
    {
        MELEE,
        RANGED, 
        HEALER,
        TANK,
        MAGE,
        DUMMY
    }
    public enum Level
    {
        NIVEL1, 
        NIVEL2, 
        NIVEL3,
    }

    public Vector3 GetPosition() 
    {
        return transform.position;
    }

    public Tipo getType()
    {
        return tipo;
    }

    public Level Getlevel()
    {
        return level;
    }
}
