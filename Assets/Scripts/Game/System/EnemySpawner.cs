using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private static EnemySpawner instance;
    public static EnemySpawner Instance { get { return instance; }}
    [Header("References")]
    public GameObject spawnPositions;
    [Header("Attributes")]
    public float spawnTimer = 20f;
    [Tooltip("Game won't generate more than number of available spawn positions")]
    public float MaxSpawnedEnemies = 3;
    private int numberOfSpawnPositions;
    private float timer;
    private int currentSpawnedEnemyCount = 0;
    private int killCount = 0;
    public int KillCount { get { return killCount; }}
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timer = spawnTimer;
        numberOfSpawnPositions = spawnPositions.GetComponentsInChildren<Transform>().Length;
        currentSpawnedEnemyCount = 0;
        killCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && currentSpawnedEnemyCount < MaxSpawnedEnemies) {
            ObjectPoolingManager.Instance.GetShootingEnemy();
            currentSpawnedEnemyCount += 1;
            timer = spawnTimer;
            Debug.Log("spawned enemy");
        }
    }

    public int GetSpawnedEnemyCount() {
        return currentSpawnedEnemyCount;
    }

    public void SubtractEnemyCount() {
        currentSpawnedEnemyCount--;
        killCount += 1;
    }
}
