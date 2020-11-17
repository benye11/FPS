using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCrateSpawner : MonoBehaviour
{
    private static AmmoCrateSpawner instance;
    public static AmmoCrateSpawner Instance { get { return instance; }}
    [Header("References")]
    public GameObject spawnPositions;
    [Header("Attributes")]
    public float spawnTimer = 20f;
    [Tooltip("Game won't generate more than number of available spawn positions")]
    public float MaxSpawnedAmmoCrates = 4;
    private int numberOfSpawnPositions;
    private float timer;
    private int currentSpawnedAmmoCrateCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timer = spawnTimer;
        numberOfSpawnPositions = spawnPositions.GetComponentsInChildren<Transform>().Length;
        if (MaxSpawnedAmmoCrates > numberOfSpawnPositions) {
            MaxSpawnedAmmoCrates = numberOfSpawnPositions;
        }
        currentSpawnedAmmoCrateCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && currentSpawnedAmmoCrateCount < MaxSpawnedAmmoCrates) {
            ObjectPoolingManager.Instance.GetAmmoCrate();
            currentSpawnedAmmoCrateCount += 1;
            timer = spawnTimer;
        }
    }

    public void SubtractAmmoCrate() {
        currentSpawnedAmmoCrateCount -= 1;
    }
}
