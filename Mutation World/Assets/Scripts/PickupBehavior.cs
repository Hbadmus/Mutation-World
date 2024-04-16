using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupBehavior : MonoBehaviour
{
    public int healthGain = 20;
    public float respawnTime = 5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<PlayerHealth>().HealDamage(healthGain);
                gameObject.SetActive(false);
            }
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnTime);
        gameObject.SetActive(true);
    }
}

