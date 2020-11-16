using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; }}

    [Header("Game")]
    public PlayerBehavior player;

    [Header("UI")]
    public Text healthText;
    public Text ammoText;
    public Text enemyText;
    public Text EnemiesKilledText;
    private int enemyCount = 0;
    // Start is called before the first frame update
    void Awake()
    {
        enemyCount = 0;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + player.Health;
        ammoText.text = "Ammo: " + player.Ammo;
        enemyCount = EnemySpawner.Instance.GetSpawnedEnemyCount();
        EnemiesKilledText.text = "Enemies Killed: " + EnemySpawner.Instance.KillCount;
        if (enemyCount == 0) {
            enemyText.text = "No enemies";
        }
        else if (enemyCount == 1) {
            enemyText.text = "Enemy: 1 ";
        }
        else {
            enemyText.text = "Enemies: " + enemyCount;
        }
    }
}
