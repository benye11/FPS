using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemy : Enemy
{
    [Header("Attributes")]
    public float shootingInterval = 4f;
    public float shootingDistance = 3f;
    public float RagdollDuration = 5f;
    public float bulletSpeed = 35f;
    public int bulletDamage = 5;
    private float shootingTimer;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        //agent = GetComponent<NavMeshAgent>();
        shootingTimer = Random.Range(0, shootingInterval); //so that all enemies won't shoot at same time in the beginning
    }

    protected override void OnEnable() {
        base.OnEnable();
        shootingTimer = Random.Range(0, shootingInterval);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (ToggleShooting) {
            //Debug.Log("shooting");
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance) {
            shootingTimer = shootingInterval;
            GameObject bullet = ObjectPoolingManager.Instance.GetBullet(false, bulletDamage, bulletSpeed);
            bullet.transform.position = transform.position;
            bullet.transform.forward = (player.transform.position - transform.position).normalized;
            Debug.Log("shot bullet at player");
            //agent.SetDestination(player.transform.position);
        }
        }
    }

    //base.OnKill() runs the OnKill from parent before overriding
    protected override void OnKill() {
        base.OnKill();
        agent.enabled = false;
        this.enabled = false; //shooting script disabled
        transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, transform.localEulerAngles.z);
        EnemySpawner.Instance.SubtractEnemyCount();
        StartCoroutine(RemoveRagdoll());
    }

    IEnumerator RemoveRagdoll() {
        yield return new WaitForSeconds(RagdollDuration);
        gameObject.SetActive(false);
    }
}
