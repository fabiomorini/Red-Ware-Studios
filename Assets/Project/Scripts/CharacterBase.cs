using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    //Animation controller: atacar, moverse etc..

    //Recoger la posición del este jugador
    public Vector3 GetPosition(){
        return transform.position;
    }
}
