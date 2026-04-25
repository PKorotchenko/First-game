using System;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawn Settings")]
    public float baseSpawnDelay = 1.6f;
    public float minSpawnDelay = 1.0f;
    public float maxSpawnDelay = 2.4f;
    public float spawnX = 11f;
    public float verticalRange = 4f;

    [Header("Difficulty")]
    public float difficultySpawnRate = 0.09f;

    private float nextSpawnTime;
    private float currentSpeed = 5f;
    private bool spawningEnabled;

    private void Start()
    {
        ResetSpawner();
    }

    private void Update()
    {
        if (!spawningEnabled || !GameManager.Instance || !GameManager.Instance.IsRunning)
            return;

        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
        }
    }

    public void ResetSpawner()
    {
        nextSpawnTime = Time.time + baseSpawnDelay;
    }

    public void EnableSpawning(bool enabled)
    {
        spawningEnabled = enabled;
        if (enabled)
        {
            nextSpawnTime = Time.time + baseSpawnDelay;
        }
    }

    public void UpdateDifficulty(float difficulty, float currentSpeed)
    {
        this.currentSpeed = currentSpeed;
        float spawnDelay = Mathf.Clamp(baseSpawnDelay - difficulty * difficultySpawnRate, minSpawnDelay, maxSpawnDelay);
        nextSpawnTime = Mathf.Max(nextSpawnTime, Time.time + spawnDelay);
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Length == 0)
        {
            return;
        }

        GameObject prefab = ChooseObstaclePrefab();
        if (prefab == null)
        {
            return;
        }

        Vector3 spawnPosition = new Vector3(spawnX, UnityEngine.Random.Range(-verticalRange, verticalRange), 0f);
        GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
        ObstacleBase obstacle = instance.GetComponent<ObstacleBase>();
        if (obstacle != null)
        {
            obstacle.SetSpeed(currentSpeed);
        }

        float delay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
        nextSpawnTime = Time.time + delay;
    }

    private GameObject ChooseObstaclePrefab()
    {
        float difficulty = GameManager.Instance != null ? GameManager.Instance.Difficulty : 1f;
        float[] weights = new float[obstaclePrefabs.Length];

        for (int i = 0; i < weights.Length; i++)
        {
            float baseWeight = 1f;
            if (i == 1) // station
                baseWeight += difficulty * 0.2f;
            if (i == 2) // black hole
                baseWeight += Mathf.Min(difficulty * 0.12f, 1.5f);
            if (i == 3) // meteorite
                baseWeight += Mathf.Min(difficulty * 0.1f, 1.2f);

            weights[i] = baseWeight;
        }

        float totalWeight = weights.Sum();
        float randomValue = UnityEngine.Random.value * totalWeight;
        float cumulative = 0f;

        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            cumulative += weights[i];
            if (randomValue <= cumulative)
            {
                return obstaclePrefabs[i];
            }
        }

        return obstaclePrefabs[0];
    }
}
