using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public override void OnEnter(EnemyStateManager context, Enemy enemy)
    {

    }

    public override void OnUpdate(EnemyStateManager context, Enemy enemy)
    {
        if (enemy.Target == null)
            return;

        // Check distance to target
        float distanceToTarget = Vector2.Distance(enemy.Target.transform.position, enemy.Rb.position);

        if (distanceToTarget <= enemy.AttackRange)
        {
            context.SwitchState(context.AttackState);
        }
    }

    public override void OnFixedUpdate(EnemyStateManager context, Enemy enemy)
    {
        Rigidbody2D rb = enemy.Rb;

        if (enemy.Target == null || rb == null)
            return;

        // Convert target position to Vector2
        Vector2 targetPosition = enemy.Target.transform.position;

        // Calculate the direction to the target
        Vector2 direction = (targetPosition - rb.position).normalized;

        // Apply force towards the target
        rb.AddForce(direction * enemy.Acceleration, ForceMode2D.Force);

        // Clamp the velocity to maxSpeed
        if (rb.velocity.magnitude > enemy.MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * enemy.MaxSpeed;
        }
    }

    public override void OnExit(EnemyStateManager context, Enemy enemy)
    {

    }
}
