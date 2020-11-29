using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGridCombat : MonoBehaviour {

    [SerializeField] private Team team;

    private Character_Base characterBase;
    private GameObject selectedGameObject;
    private MovePositionPathfinding movePosition;
    private State state;

    public enum Team {
        Blue,
        Red
    }

    private enum State {
        Normal,
        Moving,
        Attacking
    }

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<MovePositionPathfinding>();
        state = State.Normal;
    }


    private void Update() {
        switch (state) {
            case State.Normal:
                break;
            case State.Moving:
                break;
            case State.Attacking:
                break;
        }
    }

    public void SetSelectedVisible(bool visible) {
        selectedGameObject.SetActive(visible);
    }

    public void MoveTo(Vector3 targetPosition, Action onReachedPosition) {
        state = State.Moving;
        movePosition.SetMovePosition(targetPosition + new Vector3(1, 1), () => {
            state = State.Normal;
            onReachedPosition();
        });
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Team GetTeam() {
        return team;
    }

    public bool IsEnemy(UnitGridCombat unitGridCombat) {
        return unitGridCombat.GetTeam() != team;
    }

}
