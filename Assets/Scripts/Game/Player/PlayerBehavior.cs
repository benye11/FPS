using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    //public GameObject bulletPrefab;
    //[Header("Label")] adds label in your unity IDE
    [Header("Visuals")]
    public Camera playerCamera;
    [Header("Gameplay")]
    public GameObject pistol;
    public GameObject assaultRifle;
    public GameObject sniperRifle;
    public GameObject GameOverPanel;
    public int initialHealth = 30;
    public int initialAmmo = 12;
    private int health = 50;
    private Text GameOverText;
    private int[] ammoArray;
    private float[] shootTimerArray;
    private float[] shootIntervalArray;
    private int[] gunDamageArray;
    private float[] bulletSpeedArray;
    private bool[] hasWeapon;
    private int[] ammoCrateDivider;
    private int gunSelection = 0;
    //this allows us to not tweak the value on accident from IDE but allow classes to attach it.
    // Start is called before the first frame update
    public float hurtDuration = 0.5f;
    public float knockbackForce = 50f;
    private int killcount;
    private bool invulnerable;
    private bool killed = false;

    public bool Killed { get { return killed; }}
    public int Health { get { return health; }}
    public int Ammo { get { return ammoArray[gunSelection]; }}
    void Start()
    {
        //start off with a pistol
        //0 is pistol, 1 is assaultRifle, 2 is sniper
        ammoArray = new int[] {initialAmmo, 0, 0};
        shootTimerArray = new float[] {0f, 0f, 0f};
        shootIntervalArray = new float[] {1f, 0.5f, 2f};
        bulletSpeedArray = new float[] {30f, 40f, 50f};
        gunDamageArray = new int[] {5, 10, 30};
        hasWeapon = new bool[] {true, false, false};
        ammoCrateDivider = new int[] {1, 2, 4};
        killed = false;
        killcount = 0;
        invulnerable = false;
        health = initialHealth;
        gunSelection = 0;
        GameOverText = GameOverPanel.transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!killed) {
        if (Input.GetKeyDown("1")) {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown("2")) {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown("3")) {
            SwitchWeapon(2);
        }
            shootTimerArray[gunSelection] -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && shootTimerArray[gunSelection] <= 0) {
            if (ammoArray[gunSelection] > 0) {
                ammoArray[gunSelection]--;
            shootTimerArray[gunSelection] = shootIntervalArray[gunSelection];
            //instantiation takes some time for unity to fetch object and create.
            //good developer practice utilizes object pooling
            //pooling manager will preload a certain amount of objects which will be made inactive.
            //instead of instantiating, we will get bullets from the pool and activate it.
            GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true, gunDamageArray[gunSelection], bulletSpeedArray[gunSelection]);
            //bulletObject.transform.Rotate(90, 0, 0);
            //ObjectPoolingManager.Instance.GetBullet(); //since it's a static, this will be okay without reference
            //bulletObject.transform.rotation = playerCamera.transform.rotation;
            bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
            bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }
        }

        if (killed && Input.GetKeyDown("space")) {
            SceneManager.LoadScene("Menu");
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
        if (!killed) {
        if (otherCollider.GetComponent<BulletBehavior>() != null) {
            BulletBehavior bullet = otherCollider.GetComponent<BulletBehavior>();
            if (bullet.ShotByPlayer == false) {
                health -= bullet.damage;
                bullet.gameObject.SetActive(false);
            }
        }
        else if (otherCollider.GetComponent<AmmoCrate>() != null) {
            AmmoCrate ammoCrate = otherCollider.GetComponent<AmmoCrate>();
            ammoArray[gunSelection] += (ammoCrate.ammo/ammoCrateDivider[gunSelection]);
            //Destroy(ammoCrate.gameObject);
            AmmoCrateSpawner.Instance.SubtractAmmoCrate();
            ammoCrate.gameObject.SetActive(false);
            //ammoCrate.gameObject.SetActive(false); //implement this
        }
        else if (otherCollider.GetComponent<HealthKit>() != null) {
            HealthKit healthKit = otherCollider.GetComponent<HealthKit>();
            health += healthKit.health;
            HealthKitSpawner.Instance.SubtractHealthKit();
            healthKit.gameObject.SetActive(false);
        }
        else if (otherCollider.GetComponent<AssaultRifle>() != null) {
            AddWeapon(1);
            AssaultRifle assaultRifle = otherCollider.GetComponent<AssaultRifle>();
            assaultRifle.gameObject.SetActive(false);
        }
        else if (otherCollider.GetComponent<SniperRifle>() != null) {
            AddWeapon(2);
            SniperRifle sniperRifle = otherCollider.GetComponent<SniperRifle>();
            sniperRifle.gameObject.SetActive(false);
        }
        else if (otherCollider.GetComponent<Enemy>() != null) {
            if (invulnerable == false) {
            Enemy enemy = otherCollider.GetComponent<Enemy>();
            if (enemy.CheckAlive()) {
                health -= enemy.damage;
                invulnerable = true;
                GameObject hazard = enemy.gameObject;
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                //GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce); //since we use CharacterController, we don't have a normal rigidbody
                GetComponent<ForceReceiver>().AddForce(knockbackDirection, knockbackForce);
                StartCoroutine(HurtRoutine());
            }
            }
        }

        if (health <= 0) {
            if (killed == false) {
                killed = true;
                OnKill();
            }
        }
        }
    }

    //this starts a new thread
    IEnumerator HurtRoutine() {
        yield return new WaitForSeconds(hurtDuration);
        invulnerable = false;
    }

    private void OnKill() {
        //gameObject.SetActive(false);
        GameOverPanel.SetActive(true);
        GameOverText.text = "Game Over! You died.\n" + "Enemies Killed: " + EnemySpawner.Instance.KillCount + "\nPress 'space' to restart";
        //GetComponent<Rigidbody>().enabled = false;
        GetComponent<CameraMovement>().DisableMovement();
        GetComponent<PlayerMovement>().DisableMovement();
    }

    private void SwitchWeapon(int selection) {
        if (selection == 0 && hasWeapon[selection] && selection != gunSelection) {
            //pistol
            gunSelection = 0;
            pistol.SetActive(true);
            assaultRifle.SetActive(false);
            sniperRifle.SetActive(false);
            GameController.Instance.UpdateGunTextColor(0);
        }
        else if (selection == 1 && hasWeapon[selection] && selection != gunSelection) {
            //assaultrifle
            gunSelection = 1;
            pistol.SetActive(false);
            assaultRifle.SetActive(true);
            sniperRifle.SetActive(false);
            GameController.Instance.UpdateGunTextColor(1);
        }
        else if (selection == 2 && hasWeapon[selection] && selection != gunSelection) {
            //sniper
            gunSelection = 2;
            pistol.SetActive(false);
            assaultRifle.SetActive(false);
            sniperRifle.SetActive(true);
            GameController.Instance.UpdateGunTextColor(2);
        }
    }

    private void AddWeapon(int selection) {
        //make it so the UI appears
        //allow player to switch to weapon
        hasWeapon[selection] = true;
    }
}
