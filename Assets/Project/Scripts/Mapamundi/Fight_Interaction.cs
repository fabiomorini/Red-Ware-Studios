using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight_Interaction : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other) {
      
        //Aparece UI de batalla
        //Por ahora simplemente enseña que funciona
        if (other.tag == "Player")
        {
            Debug.Log("Fight");
        }

    }
}
