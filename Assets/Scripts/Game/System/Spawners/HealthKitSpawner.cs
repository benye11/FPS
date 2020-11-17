using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKitSpawner : MonoBehaviour
{
private static HealthKitSpawner instance;
    public static HealthKitSpawner Instance { get { return instance; }}
    [Header("References")]
    public GameObject spawnPositions;
    [Header("Attributes")]
    public float spawnTimer = 40f;
    [Tooltip("Game won't generate more than number of available spawn positions")]
    public float MaxSpawnedHealthKits = 3;
    private int numberOfSpawnPositions;
    private float timer;
    private int currentSpawnedHealthKitCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timer = spawnTimer;
        numberOfSpawnPositions = spawnPositions.GetComponentsInChildren<Transform>().Length;
        if (MaxSpawnedHealthKits > numberOfSpawnPositions) {
            MaxSpawnedHealthKits = numberOfSpawnPositions;
        }
        currentSpawnedHealthKitCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && currentSpawnedHealthKitCount < MaxSpawnedHealthKits) {
            ObjectPoolingManager.Instance.GetHealthKit();
            currentSpawnedHealthKitCount += 1;
            timer = spawnTimer;
        }
    }

    public void SubtractHealthKit() {
        currentSpawnedHealthKitCount -= 1;
    }
}
