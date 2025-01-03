using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShootWeapon : MonoBehaviour
{
    public GameObject ammoPrefab;
    public AudioSource reloadSFX;
    public Text ammoCount;

    public float speed = 50; // bullet speed
    int fireCount = 3;
    GameObject projectile;
    Animator animState;
    GameObject player;
    int maxAmmoInClip;
    int ammoInClip;
    bool reloading;
    bool canShoot;
    Rigidbody rb;

    Color originalColor;

    int bulletcounter = 0; // Limit firing to only 1 bullet every "bulletcounter" fixed frames

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0) {
        player = GameObject.FindGameObjectWithTag("Player");

      //  ammoCount = GameObject.Find("Player Canvas").transform.Find("Ammo2")
      //  .GetComponent<Text>();

        ammoCount = GameObject.Find("PlayerCanvas").transform.Find("Ammo2")
        .GetComponent<Text>();
        Debug.Log("This is the ammocount" + ammoCount);

        if (player.name.Contains("Jack"))
        {
            speed *= 3;
            ammoInClip = 30;
        }  else // Fiona doesn't reload
        {
            ammoInClip = 999999999;
            ammoCount.gameObject.SetActive(false);
        }
        maxAmmoInClip = ammoInClip;

        ammoCount.text = (ammoInClip + "/" + maxAmmoInClip);

        animState = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !player.name.Contains("Fiona")
                && ammoInClip != maxAmmoInClip && animState.GetInteger("firingState") == 0)
        {
            reloadSFX.Play();
            reloading = true;
            animState.SetInteger("firingState", 2);
            if (player.name.Contains("Jack"))
            {
                Invoke("DoneReload", 3);
            } else
            {
                Invoke("DoneReload", 2);
            }
        }
    }

    private void FixedUpdate()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0) {
        if (!reloading)
        {
            if (!player.name.Contains("Fiona"))
            {
                DoneFiring();
            }

            int rateOfFire = 5; // the fastest you can shoot

            if (!CharacterAbilites.isJackAutoActive)
            {
                rateOfFire *= 4; //if Jack's ability is not active, slow down fire rate by four
            }
            if (player.name.Contains("Fiona"))
            {
                rateOfFire *= 2; //if Fiona, slow fire rate by 2 times
            }

            if (bulletcounter % rateOfFire == 0)
            {
                canShoot = true; //Controls how fast the user can shoot
            }

            if (Input.GetButton("Fire1") && canShoot && ammoInClip != 0) // Hold down for automatic firing
            {
                Fire();
            }
            bulletcounter++;

            if (ammoInClip <= maxAmmoInClip / 4)
            {
                LevelManager.instance.reloadPopUp.SetActive(true);
            }
            else
            {
                LevelManager.instance.reloadPopUp.SetActive(false);
            }
        }
        }
    }

       private void Fire()
{
    if (MouseLook.enable)
    {
        if (CharacterAbilites.isStormOfArrowsActive)
        {
            Debug.Log("Storm of Arrows unleashed!");
            int numberOfProjectiles = 20;
            float arcAngle = 60f; // Total spread angle
            float angleStep = arcAngle / (numberOfProjectiles - 1);
            float startAngle = -arcAngle / 2;

            for (int i = 0; i < numberOfProjectiles; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Quaternion spreadRotation = Quaternion.Euler(0, currentAngle, 0);
                Vector3 spreadDirection = spreadRotation * transform.forward;
                
                GameObject projectile = Instantiate(ammoPrefab, 
                    transform.position + transform.forward, 
                    transform.rotation * spreadRotation) as GameObject;
                
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.AddForce(spreadDirection * 100, ForceMode.VelocityChange);
            }
            fireCount--;
            if (fireCount == 0)
            {
                CharacterAbilites.isStormOfArrowsActive = false;
            }
        }

        if (!player.name.Contains("Fiona"))
        {
            animState.SetInteger("firingState", 1);
            ammoInClip--;
        }

        Quaternion bulletOrientation = transform.rotation * Quaternion.Euler(90, 13, 0);

        projectile = Instantiate(ammoPrefab, transform.position +
        transform.forward, bulletOrientation) as GameObject;

        rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);

        projectile.transform.SetParent(GameObject.FindGameObjectWithTag("AmmoTrash").transform);

        canShoot = false;

        ammoCount.text = ammoInClip + "/" + maxAmmoInClip;

        
    }
}

    void DoneFiring()
    {
        animState.SetInteger("firingState", 0);
    }

    void DoneReload()
    {
        reloading = false;
        ammoInClip = maxAmmoInClip;
        ammoCount.text = (ammoInClip + "/" + maxAmmoInClip);
    }


}
