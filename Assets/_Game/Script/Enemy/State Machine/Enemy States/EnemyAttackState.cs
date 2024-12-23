using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer = 0f; // Timer to track elapsed time for firing
    private float attackCooldown;

    public override void OnEnter(EnemyStateManager context, Enemy enemy)
    {
        attackCooldown = 1 / enemy.AttackSpeed;

        Rigidbody2D rb = enemy.Rb;
        if (rb != null)
        {
            rb.velocity = Vector2.zero;

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public override void OnUpdate(EnemyStateManager context, Enemy enemy)
    {
        attackTimer += Time.deltaTime;

        if (enemy.Target == null)
            return;

        // Calculate the distance to the target
        float distanceToTarget = Vector2.Distance(enemy.Target.transform.position, enemy.Rb.position);

        GameObject target = enemy.Target.gameObject;
        var position = enemy.transform.position;

        Fire(enemy, target, position);

        // Check if the target has left the attack range
        if (distanceToTarget > enemy.AttackRange)
        {
            // Switch back to the ChaseState
            context.SwitchState(context.ChaseState);
        }
    }

    public override void OnFixedUpdate(EnemyStateManager context, Enemy enemy)
    {
        
    }

    public override void OnExit(EnemyStateManager context, Enemy enemy)
    {
        Rigidbody2D rb = enemy.Rb;
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Fire(Enemy enemy, GameObject target, Vector2 position)
    {
        // If enough time has passed since the last fire, fire again
        if (attackTimer >= attackCooldown)
        {
            if (target == null)
            {
                return;
            }

            Vector2 shootDirection = ((Vector2)target.transform.position - (Vector2)position).normalized;

            // Spawn the bullet using object pool
            Bullet bulletObject = ObjectPoolManager.Instance.SpawnObject<Bullet>(enemy.BulletPF.gameObject, position, Quaternion.identity.normalized, PoolType.GameObject);

            if (bulletObject != null)
            {
                // Initialize the bullet with the direction
                bulletObject.OnInit(shootDirection, enemy.gameObject.layer);

                // Calculate the angle to rotate the bullet
                float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;

                // Rotate the bullet to face the target (the bullet's up is aligned to the target direction)
                bulletObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
            }

            attackTimer = 0f; // Reset timer
        }

    }
}
