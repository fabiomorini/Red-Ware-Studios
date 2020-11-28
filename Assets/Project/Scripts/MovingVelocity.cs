using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed = 50f;

    private Vector3 velocityVector;
    private Rigidbody2D rb;
    private CharacterBase characterBase;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        characterBase = GetComponent<CharacterBase>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rb.velocity = velocityVector * moveSpeed;

        //characterBase.PlayMoveAnim(velocityVector);
    }

    public void Disable() {
        this.enabled = false;
        rb.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}
