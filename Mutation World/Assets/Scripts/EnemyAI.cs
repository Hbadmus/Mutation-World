using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
     public enum FSMStates {
        Idle,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;
    public float Attackdistance = 4;
    public int ChaseDis = 50;
    public GameObject throwable;
    public GameObject spawnpoint;
    public Transform player;
    public float damRate = 2;
    public int damAmt = 10;
    public int damBossAmt = 10;
    EnemyHealth enemyHealth;
    int health;

    Animator anim;
    Vector3 nextDestination;
    int currentDestinationIndex = 0;
    float disToPlayer;
    float elapsedTime = 0;
    public int timer = 1;
    Transform deadTransform;
    bool hasAttacked;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
                if(player == null) {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentState = FSMStates.Idle;
         enemyHealth = GetComponent<EnemyHealth>();

         health = enemyHealth.currentHealth;
         isDead = false;
         anim = GetComponent<Animator>();
         InvokeRepeating("IncrementTimer", 0f, 1f);


    }

    // Update is called once per frame
    void Update()
    {
    
        health = enemyHealth.currentHealth;
        disToPlayer = Vector3.Distance(transform.position, player.transform.position);
    if (transform.rotation.eulerAngles.x != 0 || transform.rotation.eulerAngles.z != 0) {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

        switch(currentState){
         case FSMStates.Idle:
            UpdateIdleState();
            break;
         case FSMStates.Chase:
            UpdateChaseState();
            break;
        case FSMStates.Attack:
            UpdateAttackState();
            break;
         case FSMStates.Dead:
            UpdateDeadState();
            break;   
        }

        elapsedTime += Time.deltaTime;

        if (health <= 0) {
            if(gameObject.CompareTag("Boss"))
            {
                LevelManager.instance.PlayerWon();
            }
            currentState = FSMStates.Dead;
        }
    }

void UpdateIdleState() {


if (disToPlayer > Attackdistance) {
        anim.SetTrigger("movementAnimationTrigger");
        currentState = FSMStates.Chase;
    }
}



void UpdateChaseState() {
    FaceTarget(player.transform.position);
    if (disToPlayer <= Attackdistance) {
       anim.ResetTrigger("movementAnimationTrigger");
       anim.SetBool("attackPlayer", true);
       currentState = FSMStates.Attack;
    } else if(timer % 10f == 0 && gameObject.CompareTag("Boss")  && disToPlayer > Attackdistance) {
       anim.ResetTrigger("movementAnimationTrigger");
       anim.SetBool("attackPlayer", true);
       currentState = FSMStates.Attack;
       } else {
        anim.SetBool("attackPlayer", false);
}
}

void UpdateAttackState() {
    FaceTarget(player.transform.position);
    if(gameObject.CompareTag("Boss") && disToPlayer > Attackdistance) {
        GameObject attack = Instantiate(throwable, 
        spawnpoint.transform.position + transform.forward, transform.rotation) as GameObject;
            Vector3 directionToPlayer = (player.position - spawnpoint.transform.position).normalized;
            Rigidbody rb = attack.GetComponent<Rigidbody>();
            rb.AddForce(directionToPlayer * 200, ForceMode.VelocityChange);

            hasAttacked = true;
    if(hasAttacked){
        anim.SetTrigger("movementAnimationTrigger");
        currentState = FSMStates.Chase;
    }
    }
    if (disToPlayer > Attackdistance) {
        anim.SetTrigger("movementAnimationTrigger");
        currentState = FSMStates.Chase;
    }

    EnemyDamage();
}


    void UpdateDeadState() {
        anim.SetBool("0Health", true);
        isDead = true;
        deadTransform = gameObject.transform;
        Destroy(gameObject, 5);
    }
 


    void FaceTarget(Vector3 target) {
Vector3 directionToTarget = (target - transform.position).normalized;
directionToTarget.y = 0;
Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemyDamage() {
        if(!isDead) 
        {
            if(gameObject.CompareTag("Boss")) {
 
            }
            //make sure you loop the attack animation
        if(elapsedTime >= damRate && currentState == FSMStates.Attack) {
        Invoke("DoDamage", 0.3f);
        elapsedTime = 0.0f;
        }
    }
    }

    void DoDamage() {
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damAmt);
    }

    void IncrementTimer()
{
    timer++; 
}

}
