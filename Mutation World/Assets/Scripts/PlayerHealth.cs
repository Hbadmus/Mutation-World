using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int startHealth = 200;
    public Slider healthSlider;
    public int currentHealth;
    private bool isAceDamageReductionActive = false;
    private float aceDamageReductionDuration = 5f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("PlayerCanvas").transform.Find("HealthBar")
    .transform.Find("Fill Area").gameObject.SetActive(true);
        currentHealth = startHealth;
        if (healthSlider == null ) 
        {   
            healthSlider = GameObject.Find("PlayerCanvas").transform.Find("HealthBar")
        .GetComponent<Slider>();
        }
        if(gameObject.name.Contains("Ace")) {
        healthSlider.value = currentHealth;
        } else {
            healthSlider.maxValue = currentHealth;
            healthSlider.value = currentHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name.Contains("Ace") && Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(AceDamageReductionMode());
        }
    }

    IEnumerator AceDamageReductionMode()
    {
        isAceDamageReductionActive = true;
        yield return new WaitForSeconds(aceDamageReductionDuration);
        isAceDamageReductionActive = false;
    }

    public void TakeDamage(int dam)
    {
        if (gameObject.name.Contains("Jack"))
        {
            dam /= 2;
        }

        if (gameObject.name.Contains("Ace") && isAceDamageReductionActive)
        {
            dam /= 2;
        }

        if (currentHealth > 0)
        {
            currentHealth -= dam;
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            PlayerDies();
        }
        Debug.Log("Current Health: " + currentHealth);
    }

    public void TakeBossDamage(int dam)
    {
        if (currentHealth > 0)
        {
            currentHealth -= dam;
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {

            PlayerDies();
        }
        Debug.Log("Current Health: " + currentHealth);
    }

    public void HealDamage(int healing)
    {
        if (currentHealth < 100)
        {
            currentHealth += healing;
            if (currentHealth > 100)
            {
                currentHealth = 100;
            }
            healthSlider.value = currentHealth;
        }
    }

    void PlayerDies()
    {
        GameObject.Find("PlayerCanvas").transform.Find("HealthBar")
    .transform.Find("Fill Area").gameObject.SetActive(false);

        transform.Rotate(-90, 0, 0, Space.Self);
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
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