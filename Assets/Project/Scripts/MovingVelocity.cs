using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingVelocity : MonoBehaviour, IMoveVelocity {

    [SerializeField] private float moveSpeed;

    private Vector3 velocityVector;
    private Rigidbody2D rigidbody2D;
    private CharacterBase characterBase;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        characterBase = GetComponent<CharacterBase>();
    }

    public void SetVelocity(Vector3 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate() {
        rigidbody2D.velocity = velocityVector * moveSpeed;
    }

    public void Disable() {
        this.enabled = false;
        rigidbody2D.velocity = Vector3.zero;
    }

    public void Enable() {
        this.enabled = true;
    }

}
