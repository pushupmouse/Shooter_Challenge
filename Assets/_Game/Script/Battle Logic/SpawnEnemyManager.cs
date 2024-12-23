using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyManager : Singleton<SpawnEnemyManager>
{
    [SerializeField] private float minimumDistance;
    [SerializeField] private float maximumDistance;
    [SerializeField] private float spawnInterval = 1f; // Time in seconds between spawns
    [SerializeField] private Transform mapTransform;
    [SerializeField] private Enemy enemyPrefab;

    public static List<Enemy> Enemies = new List<Enemy>();

    private float spawnTimer = 0f; // Timer to track spawn intervals
    private PlayerController player;

    private void Update()
    {
        if (player != null)
        {
            // Update the timer
            spawnTimer += Time.deltaTime;

            // Check if the spawn interval has elapsed
            if (spawnTimer >= spawnInterval)
            {
                SpawnEnemyAtRandomPosition();
                spawnTimer = 0f; // Reset the timer
            }
        }
        else
        {
            Debug.LogWarning("Player reference is not assigned!");
        }
    }

    // Method to spawn an enemy at a random position around the player
    private void SpawnEnemyAtRandomPosition()
    {
        if (mapTransform == null)
        {
            Debug.LogWarning("Map Transform is not assigned!");
            return;
        }

        // Calculate map bounds based on the map Transform
        Vector2 mapMinBounds = (Vector2)mapTransform.position - (Vector2)mapTransform.localScale / 2f;
        Vector2 mapMaxBounds = (Vector2)mapTransform.position + (Vector2)mapTransform.localScale / 2f;

        Vector2 playerPosition = player.transform.position;

        // Retry until a valid position within bounds is found
        for (int attempts = 0; attempts < 10; attempts++) // Try up to 10 times
        {
            // Generate a random distance within the specified range
            float randomDistance = Random.Range(minimumDistance, maximumDistance);

            // Generate a random angle in radians
            float randomAngle = Random.Range(0f, Mathf.PI * 2);

            // Calculate the offset based on the random angle and distance
            float offsetX = Mathf.Cos(randomAngle) * randomDistance;
            float offsetY = Mathf.Sin(randomAngle) * randomDistance;

            // Calculate the new random position
            Vector2 randomPosition = new Vector2(playerPosition.x + offsetX, playerPosition.y + offsetY);

            // Check if the position is within map bounds
            if (randomPosition.x >= mapMinBounds.x && randomPosition.x <= mapMaxBounds.x &&
                randomPosition.y >= mapMinBounds.y && randomPosition.y <= mapMaxBounds.y)
            {
                // Spawn the enemy at the valid position
                SpawnEnemy(randomPosition);
                return; // Exit the method once a valid position is found
            }
        }

        Debug.LogWarning("Failed to find a valid spawn position within bounds.");
    }

    public void SpawnEnemy(Vector2 position)
    {
        Enemy enemyObject = ObjectPoolManager.Instance.SpawnObject<Enemy>(enemyPrefab.gameObject, position, Quaternion.identity, PoolType.GameObject);

        enemyObject.SetTarget(player);

        Enemies.Add(enemyObject);
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

    public void AssignPlayer(PlayerController player)
    {
        this.player = player;
    }
}
