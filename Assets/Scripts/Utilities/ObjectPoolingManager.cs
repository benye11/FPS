using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; }}
    public GameObject bulletPrefab;
    public GameObject shootingEnemyPrefab;
    public GameObject spawnPositions;
    public int bulletAmount = 20;
    public int enemyAmount = 5;
    private List<GameObject> bullets;
    private List<GameObject> enemies;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        bullets = new List<GameObject>(bulletAmount);
        enemies = new List<GameObject>(enemyAmount);
        for (int i = 0; i < bulletAmount; i++) {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.SetParent(transform);
            bulletObject.SetActive(false);
            bullets.Add(bulletObject);
            GameObject enemyObject = Instantiate(shootingEnemyPrefab);
            //enemyObject.transform()
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
}
