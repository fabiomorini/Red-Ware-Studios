using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStateUpdater : MonoBehaviour
{
    [SerializeField] private Team team;

    private CharacterBase characterBase;
    private HealthSystem healthSystem;
    private GameObject selectedGameObject;
    private UpdatePositionPathfinding movePosition;
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
        characterBase = GetComponent<CharacterBase>();
        selectedGameObject = transform.Find("Selected").gameObject;
        movePosition = GetComponent<UpdatePositionPathfinding>();
        //SetSelectedVisible(false);
        state = State.Normal;
        healthSystem = new HealthSystem(100);
        //init y actualizar la barra de vida
        //healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(10, 1.3f), Color.grey, Color.red, 1f, 10000, new World_Bar.Outline { color = Color.black, size = .5f });
        //healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    //Acortar la barra de vida cuando te atacan
    //private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        //healthBar.SetSize(healthSystem.GetHealthNormalized());
    //}

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

    //Hacer visible al personaje
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

    //Calcula la distancia entre 2 jugadores para poder atacar
    public bool CanAttackUnit(PlayerStateUpdater unitGridCombat) {
        return Vector3.Distance(GetPosition(), unitGridCombat.GetPosition()) < 50f;
    }

    public void AttackUnit(PlayerStateUpdater unitGridCombat, Action onAttackComplete) {
        state = State.Attacking;

        ShootUnit(unitGridCombat, () => {
            if (!unitGridCombat.IsDead()) {
                ShootUnit(unitGridCombat, () => {
                    if (!unitGridCombat.IsDead()) {
                        ShootUnit(unitGridCombat, () => {
                            if (!unitGridCombat.IsDead()) {
                                ShootUnit(unitGridCombat, () => {
                                    state = State.Normal;
                                    onAttackComplete();
                                });
                            } else { state = State.Normal; onAttackComplete(); }
                        });
                    } else { state = State.Normal; onAttackComplete(); }
                });
            } else { state = State.Normal; onAttackComplete(); }
        });
    }

    private void ShootUnit(PlayerStateUpdater unitGridCombat, Action onShootComplete) {
        GetComponent<IMoveVelocity>().Disable();
        //Direccion del ataque, se tendra que rehacer
        Vector3 attackDir = (unitGridCombat.GetPosition() - transform.position).normalized;
        //UtilsClass.ShakeCamera(.6f, .1f);
        //GameHandler_GridCombatSystem.Instance.ScreenShake();
    }

    public void Damage(PlayerStateUpdater attacker, int damageAmount) {
        //Sangre
        //Vector3 bloodDir = (GetPosition() - attacker.GetPosition()).normalized;
        //Blood_Handler.SpawnBlood(GetPosition(), bloodDir);
        //Numero de vida restado (visual)
        //DamagePopup.Create(GetPosition(), damageAmount, false);

        healthSystem.Damage(damageAmount);
        if (healthSystem.IsDead()) {
            //Si esta muerto, spawn del cadaver
            //FlyingBody.Create(GameAssets.i.pfEnemyFlyingBody, GetPosition(), bloodDir);
            Destroy(gameObject);
        } else {
            //Si lo atacamos pero no lo matamos
            // Knockback
            //transform.position += bloodDir * 5f;
        }
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public Team GetTeam() {
        return team;
    }

    public bool IsEnemy(PlayerStateUpdater unitGridCombat) {
        return unitGridCombat.GetTeam() != team;
    }

}
