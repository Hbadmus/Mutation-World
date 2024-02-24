using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyBehavior : MonoBehaviour
{
    public Transform player;
    public float minDistance = 10;
    public int damageAmt = 20;
    Animator animator;


    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        transform.LookAt(player);



        animator.SetBool("attackPlayer", distance < minDistance);
        animator.SetBool("movementAnimationTrigger", distance > minDistance);
        
        if (animator.GetBool("attackPlayer")) {
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damageAmt);
        }
    }
}
