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
    public GameObject healthKitPrefab;
    public Transform enemySpawnPositions;
    public Transform ammoCrateSpawnPositions;
    public Transform healthKitSpawnPositions;
    [Header("Number of Pooled Objects")]
    public int bulletAmount = 30;
    public int shootingEnemyAmount = 10;
    public int ammoCrateAmount = 5;
    public int healthKitAmount = 3;
    private List<GameObject> bullets;
    private List<GameObject> shootingEnemies;
    private List<GameObject> ammoCrates;
    private List<GameObject> healthKits;
    private Queue<Vector3> ammoCratePositionQueue;
    private Queue<Vector3> enemyPositionQueue;
    private Queue<Vector3> healthKitPositionQueue;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        bullets = new List<GameObject>(bulletAmount);
        shootingEnemies = new List<GameObject>(shootingEnemyAmount);
        ammoCrates = new List<GameObject>(ammoCrateAmount);
        healthKits = new List<GameObject>(healthKitAmount);
        ammoCratePositionQueue = new Queue<Vector3>();
        healthKitPositionQueue = new Queue<Vector3>();
        enemyPositionQueue = new Queue<Vector3>();
        foreach (Transform ammoCrateSpawn in ammoCrateSpawnPositions) {
            ammoCratePositionQueue.Enqueue(new Vector3(ammoCrateSpawn.position.x, Terrain.activeTerrain.SampleHeight(ammoCrateSpawn.position), ammoCrateSpawn.position.z));
            //ammoCratePositionQueue.Enqueue(ammoSpawn.position);
        }
        foreach (Transform healthKitSpawn in healthKitSpawnPositions) {
            healthKitPositionQueue.Enqueue(new Vector3(healthKitSpawn.position.x, Terrain.activeTerrain.SampleHeight(healthKitSpawn.position), healthKitSpawn.position.z));
            //healthKitPositionQueue.Enqueue(ammoSpawn.position);
        }
        foreach (Transform enemySpawn in enemySpawnPositions) {
            enemyPositionQueue.Enqueue(new Vector3(enemySpawn.position.x, Terrain.activeTerrain.SampleHeight(enemySpawn.position), enemySpawn.position.z));
            //enemyPositionQueue.Enqueue(enemySpawn.position);
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
        for (int i = 0; i < healthKitAmount; i++) {
            GameObject healthKitObject = Instantiate(healthKitPrefab);
            healthKitObject.transform.SetParent(transform.Find("healthkits"));
            healthKitObject.SetActive(false);
            healthKits.Add(healthKitObject);
        }
        for (int i = 0; i < shootingEnemyAmount; i++) {
            GameObject shootingEnemyObject = Instantiate(shootingEnemyPrefab);
            shootingEnemyObject.transform.SetParent(transform.Find("enemies"));
            shootingEnemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            shootingEnemyObject.SetActive(false);
            shootingEnemies.Add(shootingEnemyObject);
        }
    }

    public GameObject GetBullet(bool shotByPlayer, int damage, float bulletSpeed) {
        //Debug.Log("Grabbing a bullet");
        foreach (GameObject bullet in bullets) {
            if (!bullet.activeInHierarchy) {
                bullet.SetActive(true);
                bullet.GetComponent<BulletBehavior>().damage = damage;
                bullet.GetComponent<BulletBehavior>().ShotByPlayer = shotByPlayer;
                bullet.GetComponent<BulletBehavior>().speed = bulletSpeed;
                return bullet;
            }
        }
        GameObject bulletObject = Instantiate(bulletPrefab);
        bulletObject.transform.SetParent(transform);
        bulletObject.GetComponent<BulletBehavior>().damage = damage;
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

    public GameObject GetHealthKit() {
        foreach (GameObject healthKit in healthKits) {
            if (!healthKit.activeInHierarchy) {
                healthKit.SetActive(true);
                healthKit.transform.position = healthKitPositionQueue.Dequeue();
                healthKitPositionQueue.Enqueue(healthKit.transform.position);
                return healthKit;
            }
        }
        GameObject healthKitObject = Instantiate(healthKitPrefab);
        healthKitObject.transform.SetParent(transform.Find("ammocrates"));
        healthKitObject.transform.position = healthKitPositionQueue.Dequeue();
        healthKitPositionQueue.Enqueue(healthKitObject.transform.position);
        healthKits.Add(healthKitObject);
        return healthKitObject;
    }

    public GameObject GetShootingEnemy() {
        foreach (GameObject shootingEnemy in shootingEnemies) {
            if (!shootingEnemy.activeInHierarchy) {
                shootingEnemy.SetActive(true);
                shootingEnemy.transform.position = enemyPositionQueue.Dequeue(); //this needs to come first
                shootingEnemy.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                enemyPositionQueue.Enqueue(shootingEnemy.transform.position);
                //shootingEnemy.GetComponent<ShootingEnemy>().Agent.SetDestination(shootingEnemy.GetComponent<ShootingEnemy>().Player.transform.position);
                return shootingEnemy;
            }
        }
        GameObject shootingEnemyObject = Instantiate(shootingEnemyPrefab);
        shootingEnemyObject.transform.position = enemyPositionQueue.Dequeue();
        shootingEnemyObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
        shootingEnemyObject.transform.SetParent(transform.Find("enemies"));
        enemyPositionQueue.Enqueue(shootingEnemyObject.transform.position);
        //shootingEnemyObject.GetComponent<ShootingEnemy>().Agent.SetDestination(shootingEnemyObject.GetComponent<ShootingEnemy>().Player.transform.position);
        shootingEnemies.Add(shootingEnemyObject);
        return shootingEnemyObject;
    }
}
