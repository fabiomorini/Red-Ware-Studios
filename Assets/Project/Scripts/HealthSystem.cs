using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private int health;
    private int healthMax;

    public HealthSystem(int healthMax){
        this.healthMax = healthMax;
        health = healthMax;
    }

    public void Damage(int amount) {
        health -= amount;
        if (health < 0) {
            health = 0;
        }
        if (health <= 0) {
            Die();
        }
    }

    public void Die() {
        //die
    }

}

