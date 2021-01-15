using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;
    public int MaxHealth;
    public int CurrentHealth;
    public bool isDead;
    HealthBar healthBar;


    public HealthSystem(int MaxHealth) {
        this.MaxHealth = MaxHealth;
        CurrentHealth = MaxHealth;
    }
    public void Damage(int DamageAmount){
        CurrentHealth -= DamageAmount;
        if (CurrentHealth < 0) {
            CurrentHealth = 0;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (CurrentHealth <= 0) {
            Die();
        }
    }
    public void Heal(int HealAmount)
    {
        CurrentHealth += HealAmount;
        if (CurrentHealth > 3)
        {
            CurrentHealth = 3;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead() {
        return CurrentHealth <= 0;
    }
    public void Die() {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
