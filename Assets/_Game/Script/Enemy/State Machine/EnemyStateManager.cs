using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    private Enemy enemy;
    private EnemyBaseState currentState;

    public EnemyChaseState ChaseState = new EnemyChaseState();
    public EnemyAttackState AttackState = new EnemyAttackState();

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        SwitchState(ChaseState);
    }

    private void Update()
    {
        currentState.OnUpdate(this, enemy);
    }

    private void FixedUpdate()
    {
        currentState.OnFixedUpdate(this, enemy);
    }

    public void SwitchState(EnemyBaseState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this, enemy);
        }

        currentState = newState;

        currentState.OnEnter(this, enemy);
    }
}
