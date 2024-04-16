using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    TextMeshPro ammoUI; // tells the player how much ammo they have

    Color originalColor;

    int bulletcounter = 0; // Limit firing to only 1 bullet every "bulletcounter" fixed frames

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        ammoCount = FindAnyObjectByType<Text>();

        if (player.name.Contains("Jack"))
        {
            speed *= 3;
            ammoInClip = 30;
        } else if (player.name.Contains("Ace"))
        {
            ammoInClip = 12;
        } else // Fiona doesn't reload
        {
            ammoInClip = 999999999;
            ammoCount.gameObject.SetActive(false);
        }
        maxAmmoInClip = ammoInClip;

        ammoCount.text = (ammoInClip + "/" + maxAmmoInClip);

        animState = GetComponentInChildren<Animator>();
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

        private void Fire()
        {
            if (MouseLook.enable)
            {
                if (CharacterAbilites.isStormOfArrowsActive)
                {
                    Debug.Log("Storm of Arrows unleashed!");
                    int numberOfProjectiles = 20;

                    for (int i = 0; i < numberOfProjectiles; i++)
                    {
                        GameObject projectile = Instantiate(ammoPrefab, transform.position + transform.forward, transform.rotation) as GameObject;
                        Rigidbody rb = projectile.GetComponent<Rigidbody>(); // Declare rb inside the loop

                        rb.AddForce(transform.forward * 100, ForceMode.VelocityChange);
                        Debug.Log("FireCount: " + fireCount);
                    }
                    fireCount--; // Increment fireCount after the for loop
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

                // Making sure projectile goes straight.
                Quaternion bulletOrientation = transform.rotation * Quaternion.Euler(90, 13, 0);

                projectile = Instantiate(ammoPrefab, transform.position +
                transform.forward, bulletOrientation) as GameObject;


                rb = projectile.GetComponent<Rigidbody>();

                rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);

                projectile.transform.SetParent(GameObject.FindGameObjectWithTag("AmmoTrash").transform);
            }

            canShoot = false;

            ammoCount.text = ammoInClip + "/" + maxAmmoInClip;
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
