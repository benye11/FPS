using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; }}
    [Header("Prefab and Object References")]
    public GameObject bulletPrefab;
    public GameObject shootingEnemyPrefab;
    public GameObject ammoCratePrefab;
    public Transform enemySpawnPositions;
    public Transform ammoSpawnPositions;
    [Header("Attributes")]
    public int bulletAmount = 20;
    public int shootingEnemyAmount = 5;
    public int ammoCrateAmount = 5;
    private List<GameObject> bullets;
    private List<GameObject> shootingEnemies;
    private List<GameObject> ammoCrates;
    private Queue<Vector3> ammoCratePositionQueue;
    private Queue<Vector3> enemyPositionQueue;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        bullets = new List<GameObject>(bulletAmount);
        shootingEnemies = new List<GameObject>(shootingEnemyAmount);
        ammoCrates = new List<GameObject>(ammoCrateAmount);
        ammoCratePositionQueue = new Queue<Vector3>();
        enemyPositionQueue = new Queue<Vector3>();
        foreach (Transform ammoSpawn in ammoSpawnPositions) {
            ammoCratePositionQueue.Enqueue(ammoSpawn.position);
        }
        foreach (Transform enemySpawn in enemySpawnPositions) {
            enemyPositionQueue.Enqueue(enemySpawn.position);
        }
        for (int i = 0; i < bulletAmount; i++) {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.SetParent(transform.Find("bullets"));
            bulletObject.SetActive(false);
            bullets.Add(bulletObject);
        }
        for (int i = 0; i < ammoCrateAmount; i++) {
            GameObject ammoCrateObject = Instantiate(ammoCratePrefab);
            ammoCrateObject.transform.SetParent(transform.Find("ammocrates"));
            ammoCrateObject.SetActive(false);
            ammoCrates.Add(ammoCrateObject);
        }
        for (int i = 0; i < shootingEnemyAmount; i++) {
            GameObject shootingEnemyObject = Instantiate(shootingEnemyPrefab);
            shootingEnemyObject.transform.SetParent(transform.Find("enemies"));
            shootingEnemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            shootingEnemyObject.SetActive(false);
            shootingEnemies.Add(shootingEnemyObject);
        }
    }

    public GameObject GetBullet(bool shotByPlayer) {
        //Debug.Log("Grabbing a bullet");
        foreach (GameObject bullet in bullets) {
            if (!bullet.activeInHierarchy) {
                bullet.SetActive(true);
                bullet.GetComponent<BulletBehavior>().ShotByPlayer = shotByPlayer;
                return bullet;
            }
        }
        GameObject bulletObject = Instantiate(bulletPrefab);
        bulletObject.transform.SetParent(transform);
        bulletObject.GetComponent<BulletBehavior>().ShotByPlayer = shotByPlayer;
        bullets.Add(bulletObject);
        return bulletObject;
    }

    public GameObject GetAmmoCrate() {
        foreach (GameObject ammoCrate in ammoCrates) {
            if (!ammoCrate.activeInHierarchy) {
                ammoCrate.SetActive(true);
                ammoCrate.transform.position = ammoCratePositionQueue.Dequeue();
                ammoCratePositionQueue.Enqueue(ammoCrate.transform.position);
                return ammoCrate;
            }
        }
        GameObject ammoCrateObject = Instantiate(ammoCratePrefab);
        ammoCrateObject.transform.SetParent(transform.Find("ammocrates"));
        ammoCrateObject.transform.position = ammoCratePositionQueue.Dequeue();
        ammoCratePositionQueue.Enqueue(ammoCrateObject.transform.position);
        ammoCrates.Add(ammoCrateObject);
        return ammoCrateObject;
    }

    public GameObject GetShootingEnemy() {
        foreach (GameObject shootingEnemy in shootingEnemies) {
            if (!shootingEnemy.activeInHierarchy) {
                shootingEnemy.SetActive(true);
                shootingEnemy.transform.position = enemyPositionQueue.Dequeue();
                enemyPositionQueue.Enqueue(shootingEnemy.transform.position);
                shootingEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                return shootingEnemy;
            }
        }
        GameObject shootingEnemyObject = Instantiate(shootingEnemyPrefab);
        shootingEnemyObject.transform.SetParent(transform.Find("enemies"));
        shootingEnemyObject.transform.position = enemyPositionQueue.Dequeue();
        enemyPositionQueue.Enqueue(shootingEnemyObject.transform.position);
        shootingEnemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        shootingEnemies.Add(shootingEnemyObject);
        return shootingEnemyObject;
    }
}
