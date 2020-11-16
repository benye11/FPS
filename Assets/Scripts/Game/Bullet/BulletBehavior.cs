using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 8f;
    public float lifeDuration = 2f;
    public int damage = 5;
    private float lifeTimer;
    private bool shotByPlayer;
    public bool ShotByPlayer { get { return shotByPlayer;} set {shotByPlayer = value; } } //value is a specific keyword in setter
    void OnEnable() {
        lifeTimer = lifeDuration;
    }
    /*
    void Start() //only called it's instantiated. so for pooling, we can't use this.
    {
        lifeTimer = lifeDuration;
        //Destroy(gameObject, lifeDuration);
    }*/

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifeTimer -=  Time.deltaTime;
        if (lifeTimer <= 0) {
            gameObject.SetActive(false);
        }
    }
}
