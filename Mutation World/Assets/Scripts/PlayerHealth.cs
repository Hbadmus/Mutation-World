using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 100;
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
        Debug.Log("You died");
    }
}