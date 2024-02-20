using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public int healthGain = 20;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player"))
            {
                Destroy(gameObject);
                FindObjectOfType<PlayerHealth>().HealDamage(healthGain);
            }
    }
}

