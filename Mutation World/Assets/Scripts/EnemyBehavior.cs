using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
public Transform player;
    public float damRate = 2;
    public int damAmt = 10;
    
    float elapsedTime = 0;
    NavMeshAgent agent;
    int health;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        agent = GetComponent<NavMeshAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        health = GetComponent<EnemyHealth>().currentHealth;

        agent.SetDestination(player.position);
                elapsedTime += Time.deltaTime;

        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }

        private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
           EnemyDamage();
        }
    }

        void EnemyDamage() {
        if(elapsedTime >= damRate) {
        Invoke("DoDamage", 0.3f);
        elapsedTime = 0.0f;
        }
    }

    void DoDamage() {
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damAmt);
    }

}
