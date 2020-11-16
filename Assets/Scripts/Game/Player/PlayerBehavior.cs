using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    //public GameObject bulletPrefab;
    //[Header("Label")] adds label in your unity IDE
    [Header("Visuals")]
    public Camera playerCamera;
    [Header("Gameplay")]
    public GameObject gun;
    public int initialHealth = 100;
    public int initialAmmo = 12;
    public int health = 100;
    public int Health { get { return health; }}
    private int ammo;
    public int Ammo { get { return ammo; }} //properties are open and close braces. behind the scenes, they have getter and setters.
    //this allows us to not tweak the value on accident from IDE but allow classes to attach it.
    // Start is called before the first frame update
    public float hurtDuration = 0.5f;
    public float knockbackForce = 50f;
    private bool invulnerable;
    private bool killed = false;
    public bool Killed { get { return killed; }}
    void Start()
    {
        invulnerable = false;
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            if (ammo > 0) {
                ammo--;
            //instantiation takes some time for unity to fetch object and create.
            //good developer practice utilizes object pooling
            //pooling manager will preload a certain amount of objects which will be made inactive.
            //instead of instantiating, we will get bullets from the pool and activate it.
            GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true);
            //bulletObject.transform.Rotate(90, 0, 0);
            //ObjectPoolingManager.Instance.GetBullet(); //since it's a static, this will be okay without reference
            //bulletObject.transform.rotation = playerCamera.transform.rotation;
            bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }
    }

    /*
    //overriding unity function to check for physical collisions.
    //However, since player uses Character Controller Component, we must use a different method
    void onCollisionEnter(Collision collision) {
        if (collision.gameObject.GetComponent<AmmoCrate>() != null) {
            AmmoCrate ammoCrate = collision.gameObject.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;

            //Destroy(ammoCrate.gameObject);
            ammoCrate.setActive(false);
        }
    }*/

    //but using void OnControllerColliderHit(ControllerColliderHit hit) might complicate things
    //Therefore, let's do ontriggerEnter
    //onTriggerEnter interacts between a rigidbody and collider trigger.
    //the trigger is the collider (on trigger)
    void OnTriggerEnter(Collider otherCollider) {
        GameObject hazard = null;
        if (otherCollider.GetComponent<BulletBehavior>() != null) {
            BulletBehavior bullet = otherCollider.GetComponent<BulletBehavior>();
            if (bullet.ShotByPlayer == false) {
                health -= bullet.damage;
                hazard = bullet.gameObject;
                bullet.gameObject.SetActive(false);
            }
        }
        else if (otherCollider.GetComponent<AmmoCrate>() != null) {
            AmmoCrate ammoCrate = otherCollider.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;
            Destroy(ammoCrate.gameObject);
            //ammoCrate.gameObject.SetActive(false); //implement this
        }
        else if (otherCollider.GetComponent<Enemy>() != null) {
            if (invulnerable == false) {
            Enemy enemy = otherCollider.GetComponent<Enemy>();
            hazard = enemy.gameObject;
            health -= enemy.damage;
            invulnerable = true;
            StartCoroutine(HurtRoutine());
            }
        }

        if (hazard != null) {
            Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
            Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
            //GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce); //since we use CharacterController, we don't have a normal rigidbody
            GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);
        }

        if (health <= 0) {
            
        }
    }

    //this starts a new thread
    IEnumerator HurtRoutine() {
        yield return new WaitForSeconds(hurtDuration);
        invulnerable = false;
    }

    private void OnKill() {
        
    }
}
