using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GridPathfindingSystem {

    public class UnitMovement {

        private List<PathNode> pathList;
        private int curX = 7;
        private int curY = 0;
        private float speed = 30f;
        public delegate Vector3 DelGetPosition();
        private DelGetPosition getPosition;
        public delegate void DelSetPosition(Vector3 set);
        private DelSetPosition setPosition;
        public delegate void PathCallback(GridPathfinding.UnitMovementCallbackType cType, object obj);
        private PathCallback pathCallback;
        private GridPathfinding.UnitMovementCallbackType callbackType;
        private object obj;
        private bool isMoving = false;
        private MapPos finalPos;
        private MapPos mapPos;
        private LastMoveTo lastMoveTo;
        public bool destroyed = false;
        private Vector3 lastDir = Vector3.zero;

        public void Setup(MapPos mapPos, DelGetPosition getPosition, DelSetPosition setPosition, float speed) {
            this.mapPos = mapPos;
            this.getPosition = getPosition;
            this.setPosition = setPosition;
            this.speed = speed;
        }
        // Update is called once per frame
        public void Update(float deltaTime) {
            if (pathList == null || (pathList != null && pathList.Count <= 0)) {
                return;
            }
            Vector3 pos = getPosition();
            isMoving = true;
            PathNode currPath = pathList[0];
            Vector3 currPos;
            float distanceCheck = 2f;
            if (pathList.Count == 1) { // Last Pos
                distanceCheck = 1f;
                currPos = new Vector3(currPath.xPos * 10 + finalPos.offsetX, currPath.yPos * 10 + finalPos.offsetY);
            } else {
                currPos = new Vector3(currPath.xPos * 10, currPath.yPos * 10);
            }

            // Move to path
            Vector3 dir = (currPos - pos).normalized;
            lastDir = dir;
            float moveAmount = Mathf.Min(deltaTime * speed, Vector3.Distance(pos, currPos));
            Vector3 dirAmt = (dir * moveAmount);
            setPosition(new Vector3(pos.x + dirAmt.x, pos.y + dirAmt.y));
            pos = getPosition();

            if (pathList.Count > 1) { //Not last pos
                MapPos prevPos = new MapPos(mapPos.x, mapPos.y);
                if (mapPos.offsetX > 0 || mapPos.offsetY > 0) {
                    // Last target had an offset
                    if (Mathf.RoundToInt(pos.x / 10f) == mapPos.x && Mathf.RoundToInt(pos.y / 10f) == mapPos.y) {
                        // Rounded position equals base MapPos without Offset
                        mapPos.x = Mathf.RoundToInt(pos.x / 10f);
                        mapPos.y = Mathf.RoundToInt(pos.y / 10f);
                        mapPos.offsetX = 0f;
                        mapPos.offsetY = 0f;
                    }
                } else {
                    mapPos.x = Mathf.RoundToInt(pos.x / 10f);
                    mapPos.y = Mathf.RoundToInt(pos.y / 10f);
                }

                if (prevPos.x != mapPos.x || prevPos.y != mapPos.y) {
                    //Event_Speaker.Broadcast(Event_Trigger.Unit_Moved, mapPos);
                    //unit.OnUnitMoved(prevPos, mapPos);
                }
            }

            if (Vector3.Distance(pos, currPos) < distanceCheck) {
                // Next path
                pathList.RemoveAt(0);
                if (pathList.Count == 0) {
                    // Final destination reached
                    setPosition(currPos);
                    pathList = null;

                    mapPos.offsetX = finalPos.offsetX;
                    mapPos.offsetY = finalPos.offsetY;
                    mapPos.straightToOffset = finalPos.straightToOffset;
                    if (finalPos.straightToOffset) {
                        mapPos.x = finalPos.x;
                        mapPos.y = finalPos.y;
                    }
                    PathComplete();
                }
            }
        }
        public Vector3 GetLastDir() {
            return lastDir;
        }
        public void SetSpeed(float spd) {
            speed = spd;
        }
        public void PathComplete() {
            if (destroyed) return;
            isMoving = false;
            if (pathCallback != null) {
                pathCallback(callbackType, obj);
            }
        }
        public void OnPathComplete(List<PathNode> path, MapPos _mapPos) {
            if (destroyed) return;
            MapPos previousFinalPos = finalPos;
            finalPos = _mapPos;

            if (path.Count > 1 && (previousFinalPos == null || (previousFinalPos.offsetX == 0 && previousFinalPos.offsetY == 0))) {
                if (Vector3.Distance(getPosition(), new Vector3(path[1].xPos * 10, path[1].yPos * 10)) <
                    Vector3.Distance(new Vector3(path[0].xPos * 10, path[0].yPos * 10), new Vector3(path[1].xPos * 10, path[1].yPos * 10))) {
                    //Currently closer, skip first position
                    path.RemoveAt(0);
                }
            }
            pathList = path;
        }

        public bool MoveTo(MapPos _mapPos, GridPathfinding.UnitMovementCallbackType _callbackType = GridPathfinding.UnitMovementCallbackType.Simple, object _obj = null, PathCallback callback = null) {
            lastMoveTo = new LastMoveTo(_mapPos, _callbackType, _obj, callback);
            callbackType = _callbackType;
            obj = _obj;
            pathCallback = callback;
            curX = mapPos.x;
            curY = mapPos.y;
            return false;
        }
        public bool MoveTo(List<MapPos> _mapPos, GridPathfinding.UnitMovementCallbackType _callbackType, object _obj, PathCallback callback) {
            lastMoveTo = new LastMoveTo(_mapPos, _callbackType, _obj, callback);
            callbackType = _callbackType;
            obj = _obj;
            pathCallback = callback;
            curX = mapPos.x;
            curY = mapPos.y;
            return false;
        }
        public bool MoveTo(LastMoveTo _lastMoveTo) {
            return MoveTo(_lastMoveTo.mapPos, _lastMoveTo.callbackType, _lastMoveTo.obj, _lastMoveTo.callback);
        }
        public void DestroySelf() {
            destroyed = true;
        }
    }

}