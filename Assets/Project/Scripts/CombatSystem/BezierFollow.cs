using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField] private Transform route;
    private float tParam;
    private Vector2 arrowPos;
    private float speed;
    private bool coroutineAllowed;

    void Start()
    {
        tParam = 0f;
        speed = 3f;
        coroutineAllowed = true;
    }


    void Update()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute());
        }
    }

    private IEnumerator GoByTheRoute()
    {
        coroutineAllowed = false;
        Vector2 p0 = route.GetChild(0).position;
        Vector2 p1 = route.GetChild(1).position;
        Vector2 p2 = route.GetChild(2).position;
        Vector2 p3 = route.GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speed;
            arrowPos = Mathf.Pow(1 - tParam, 3) * p0 +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 +
                Mathf.Pow(tParam, 3) * p3;

            transform.position = arrowPos;
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
        tParam = 0f;
    }
}
