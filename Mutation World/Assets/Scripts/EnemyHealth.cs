using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
public int startHealth = 100;

    public int damageTaken = 10;
    public AudioClip deathSFX;
    public Slider healthSlider;
    public int currentHealth;
    public GameObject pickupPrefab; 
    public float dropRate = 0.7f;

     void Awake() {
    healthSlider.GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startHealth;
        healthSlider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = Mathf.Clamp(currentHealth, 0, 100);

if (healthSlider.value <= 0) {
    healthSlider.gameObject.SetActive(false);
}

    }

    public void TakeDamage(int dam) {
        if(currentHealth > 0) {
            currentHealth -= dam;
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("Current Health: " + healthSlider.value);
    }

        public void Heal(int healing) {
        if(currentHealth < 100) {
            currentHealth += healing;
            
        }
    }

    private void Die()
    {
        DropPickup();
        Destroy(gameObject);
    }

    private void DropPickup()
    {
        if (pickupPrefab != null && Random.value < dropRate)
        {
            Instantiate(pickupPrefab, transform.position, Quaternion.identity);
        }
    }

private void OnTriggerEnter(Collider other) {
    if(other.CompareTag("Projectile")) {
        TakeDamage(damageTaken);
    }
}

}
