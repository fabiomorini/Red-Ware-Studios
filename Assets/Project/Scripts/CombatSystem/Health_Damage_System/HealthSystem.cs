using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float MaxHealth;
    public float CurrentHealth;
    private bool isDead;

    private void Start() {
        CurrentHealth = MaxHealth;
        isDead = false;
    }
    private void healthCheck(){
        if(CurrentHealth <= 0){
            CurrentHealth = 0;
            Destroy(gameObject);
        }
        if(CurrentHealth > MaxHealth){
            CurrentHealth = MaxHealth;
        }
    }
    public void Damage(float damage){
        CurrentHealth -= damage;
        healthCheck();
    }
}
