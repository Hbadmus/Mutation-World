using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    public GameObject ammoPrefab;
    public AudioSource fireSound;

    public float speed = 50;
    int fireCount = 3;
    GameObject projectile;
    Animator akAnimation;

    Rigidbody rb;

    Color originalColor;

    int bulletcounter = 0; // Limit firing to only 1 bullet every "bulletcounter" fixed frames

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").name == "Jack")
        {
            speed *= 3;
        }
        akAnimation = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Single Fire
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        DoneFiring();
        int rateOfFire = 5; // the high
        if (!CharacterAbilites.isJackAutoActive)
        {
            rateOfFire *= 3; //if Jack's ability is not active, slow down fire rate by three
        }

        if (Input.GetButton("Fire1")) // Hold down for automatic firing
        {
            if (bulletcounter % rateOfFire == 0)
            {
                Fire();
            }
            bulletcounter++;
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

                if (GameObject.FindGameObjectWithTag("Player").name == "Jack")
                {
                    playSound();
                    akAnimation.SetInteger("firingState", 1);
                }
                // Making sure projectile goes straight.
                Quaternion bulletOrientation = transform.rotation * Quaternion.Euler(90, 13, 0);

                projectile = Instantiate(ammoPrefab, transform.position +
                transform.forward, bulletOrientation) as GameObject;


                rb = projectile.GetComponent<Rigidbody>();

                rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);

                projectile.transform.SetParent(GameObject.FindGameObjectWithTag("AmmoTrash").transform);
            }
        }

    void DoneFiring()
    {
        akAnimation.SetInteger("firingState", 0);
    }

    void playSound()
    {
        fireSound.Play();
    }
}
