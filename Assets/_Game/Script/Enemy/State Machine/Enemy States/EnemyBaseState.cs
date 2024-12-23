using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void OnEnter(EnemyStateManager context, Enemy enemy);
    public abstract void OnUpdate(EnemyStateManager context, Enemy enemy);
    public abstract void OnFixedUpdate(EnemyStateManager context, Enemy enemy);
    public abstract void OnExit(EnemyStateManager context, Enemy enemy);
}
