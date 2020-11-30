using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Transform target;
    float tLX, tLY, bRX, bRY;
    public GameObject map;

    void Start()
    {
        //Escoge el mapa y crea los bordes para que la camara no salga hacia fuera
        Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();
        float cameraSize = Camera.main.orthographicSize;

        tLX = map.transform.position.x + cameraSize;
        tLY = map.transform.position.y - cameraSize;
        bRX = map.transform.position.x + config.NumTilesWide - cameraSize;
        bRY = map.transform.position.y - config.NumTilesWide + cameraSize;
    }


    void Update()
    {
        //Update de la camara para que no se salga de los bounds ya calculados
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX), 
            Mathf.Clamp(target.position.y, bRY, tLY), 
            transform.position.z
        );
    }

    /*
    public void SetBound(GameObject map)
    {
        Tiled2Unity.TiledMap config = map.GetComponent<Tiled2Unity.TiledMap>();
        float cameraSize = Camera.main.orthographicSize;

        tLX = map.transform.position.x + cameraSize;
        tLY = map.transform.position.y - cameraSize;
        bRX = map.transform.position.x + config.NumTilesWide - cameraSize;
        bRY = map.transform.position.y - config.NumTilesWide + cameraSize;
    }
    */
}
