using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Toggle On/Off")]
    public bool ToggleShooting = true;
    public bool ToggleChasing = true;
    public bool ToggleMoving = true;
    public float chasingInterval = 2f;
    public float chasingDistance = 15f;
    public int health = 15;
    public int damage = 5;
    public float knockbackForce = 20f;
    protected PlayerBehavior player;
    protected UnityEngine.AI.NavMeshAgent agent;
    public UnityEngine.AI.NavMeshAgent Agent { get { return agent; }}
    public PlayerBehavior Player { get { return player; }}
    protected float chasingTimer;
    protected bool reachedDestination = false;
    protected Vector3 randomDestination;
    protected bool killed = false;
    public bool Killed { get { return killed; } }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerBehavior>();
        //Debug.Log("player: " + player.name);
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //if (ToggleChasing && ToggleMoving) {
        //agent.SetDestination(player.transform.position);
        //}
        /*
        else if (ToggleMoving) {
            //SetValidRandomDestination();
            reachedDestination = false;
        }*/
    }
    
    //not working because it's protected maybe
    protected virtual void OnEnable() {/*
        Debug.Log("enabled is called");
        agent.enabled = true;
        if (agent.enabled) {
            Debug.Log("agent is enabled"); //this not getting called.
        }
        agent.SetDestination(player.transform.position);
        */
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (ToggleMoving) {
            //Debug.Log("moving");
            float distance = Vector3.Distance(transform.position, player.transform.position);
        /*
            float randomDistance = Vector3.Distance(transform.position, randomDestination);
        if (randomDistance <= 10f) {
            reachedDestination = true;
        }*/
        if (ToggleChasing) {
            chasingTimer -= Time.deltaTime;
            if (chasingTimer <= 0 && distance <= chasingDistance) {
                chasingTimer = chasingInterval;
                //reachedDestination = true;
                agent.SetDestination(player.transform.position);
            }
            // need to debug
            /*
            else if (distance >= chasingDistance) {
                if (reachedDestination) {
                    //SetValidRandomDestination();
                    reachedDestination = false;
                }
            }*/
        }
        //need to debug
        /*
        else if (!ToggleChasing) {
            if (reachedDestination) {
                //SetValidRandomDestination();
                reachedDestination = false;
            }
        }*/
        }
    }

    void OnTriggerEnter(Collider otherCollider) {
        if (!killed) {
        GameObject hazard = null;
        if (otherCollider.GetComponent<BulletBehavior>() != null) {
            BulletBehavior bullet = otherCollider.GetComponent<BulletBehavior>();
            bullet.gameObject.SetActive(false);
            if (bullet.ShotByPlayer == true) {
                health -= bullet.damage;
                hazard = bullet.gameObject;
                bullet.gameObject.SetActive(false);
            }
        }
        /*
        if (hazard != null) {
            Vector3 hurtDirection = -1f * (transform.position - hazard.transform.position).normalized;
            Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
            //GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce); //since we use CharacterController, we don't have a normal rigidbody
            GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);
        }*/

        if (health <= 0) {
            if (killed == false) {
                killed = true;
                OnKill();
            }
        }
        }
    }

    /*
    //for now, random doesn't work.
    void SetValidRandomDestination() {
        Debug.Log("setting RandomDestination");
        randomDestination = MyTerrain.Instance.GetRandomPositionFromMyTerrain();
        bool check = agent.SetDestination(randomDestination);
        while (!check) {
            randomDestination = MyTerrain.Instance.GetRandomPositionFromMyTerrain();
            check = agent.SetDestination(randomDestination);
        }
    }*/

    //protected means it can only be used by classes that extend this class. virtual means we can override it
    protected virtual void OnKill() {

    }

    public UnityEngine.AI.NavMeshAgent GetAgent() {
        return agent;
    }

    //returns true if alive, i.e. killed is false
    public bool CheckAlive() {
        return !killed;
    }
}
