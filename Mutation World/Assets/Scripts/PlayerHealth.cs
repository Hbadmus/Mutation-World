using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 200;
    public Slider healthSlider;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int dam) {
        if (gameObject.name == "Jack")
        {
            dam /= 2;
        }
        if(currentHealth > 0) {
            currentHealth -= dam;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0) {
            PlayerDies();
        }
        Debug.Log("Current Health: " + currentHealth);
    }

        public void TakeBossDamage(int dam) {
        if(currentHealth > 0) {
            currentHealth -= dam;
            healthSlider.value = currentHealth;
        }

        if(currentHealth <= 0) {
            PlayerDies();
        }
        Debug.Log("Current Health: " + currentHealth);
    }

    public void HealDamage(int healing) {
     if(currentHealth < 100) {
         currentHealth += healing;
         if(currentHealth > 100) {
            currentHealth = 100;
         }
         healthSlider.value = currentHealth;
     }
    }

    void PlayerDies() {

        transform.Rotate(-90, 0, 0, Space.Self);
        var rb = GetComponent<Rigidbody>();
        if(rb != null) {
            rb.isKinematic = true; 
            rb.detectCollisions = false; // Stops the Rigidbody from detecting collisions
        }
        Debug.Log("You died");
        LevelManager.instance.PlayerDied();
    }

        void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Throwable"))
        {
            
            TakeBossDamage(10);
        }
    }
}