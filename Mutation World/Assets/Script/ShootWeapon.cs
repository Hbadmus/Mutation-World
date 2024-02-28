using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{
    public GameObject ammoPrefab;

    public float speed = 100;

    Color originalColor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            GameObject projectile = Instantiate(ammoPrefab, transform.position +
            transform.forward, transform.rotation) as GameObject;

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);

            projectile.transform.SetParent(GameObject.FindGameObjectWithTag("AmmoTrash").transform);
        }
    }
}
