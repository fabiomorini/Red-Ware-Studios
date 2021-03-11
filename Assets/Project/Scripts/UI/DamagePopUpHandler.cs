using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Destroy(gameObject, 2f);
    }
}
