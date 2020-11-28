using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridPathfindingSystem;


public class UpdatePositionPathfinding : MonoBehaviour
{
    private Action onReachedTargetPosition;
    private List<Vector3> pathVectorList;
    private int pathIndex = -1;

    public void SetMovePosition(Vector3 movePosition, Action onReachedTargetPosition) {
        this.onReachedTargetPosition = onReachedTargetPosition;
        pathVectorList = GridPathfinding.instance.GetPathRouteWithShortcuts(transform.position, movePosition).pathVectorList;
        //Debug
        Debug.Log("##########");
        foreach (Vector3 vec in pathVectorList) {
            Debug.Log(vec);}

        if (pathVectorList.Count > 0) {
            //Hay que remover el primer índice para que no vaya hacia atrás
            //pathVectorList.RemoveAt(0);
        }
        if (pathVectorList.Count > 0) {
            pathIndex = 0;
        } else {
            pathIndex = -1;
        }
    }

    private void Update() {
        if (pathIndex != -1) {
            // Mueve el jugador hacia la próxima posición del tablero
            Vector3 nextPathPosition = pathVectorList[pathIndex];
            Vector3 moveVelocity = (nextPathPosition - transform.position).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveVelocity);

            float reachedPathPositionDistance = 1f;
            if (Vector3.Distance(transform.position, nextPathPosition) < reachedPathPositionDistance) {
                pathIndex++;
                if (pathIndex >= pathVectorList.Count) {
                    // End of path
                    pathIndex = -1;
                    onReachedTargetPosition();
                }
            }
        } else {
            // Idle
            GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
        }
    }

}