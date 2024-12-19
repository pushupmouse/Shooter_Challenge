using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private Enemy enemy;

    public static List<Enemy> Enemies = new List<Enemy>();

    private void Start()
    {
        SpawnEnemies(2);
    }

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Enemy enemyObject = ObjectPoolManager.Instance.SpawnObject<Enemy>(enemy.gameObject, new Vector2(i, i), Quaternion.identity, PoolType.GameObject);
            Enemies.Add(enemyObject);
        }
    }

    public Enemy GetClosestEnemy(Vector2 position, float maxRange)
    {
        Enemy closestEnemy = null;
        foreach (Enemy enemy in Enemies)
        {
            if (enemy.IsDead) continue;
            if (Vector2.Distance(position, enemy.transform.position) <= maxRange)
            {
                if (closestEnemy == null)
                {
                    closestEnemy = enemy;
                }
                else
                {
                    if (Vector2.Distance(position, enemy.transform.position) < Vector2.Distance(position, closestEnemy.transform.position))
                    {
                        closestEnemy = enemy;
                    }
                }
            }
        }

        return closestEnemy;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            SpawnEnemies(3);
        }
    }
}
