using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum FSMStates
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    public FSMStates currentState;
    public float Attackdistance = 4;
    public GameObject throwable;
    public GameObject spawnpoint;
    public Transform player;
    public int damRate = 2;
    public int damAmt = 10;
    public int timer = 1;
    public Transform enemyEyes;
    public int FOV;
   
    EnemyHealth enemyHealth; 
    int health;
    NavMeshAgent agent;
    Vector3 nextDestination;
    float disToPlayer;
    float elapsedTime = 0;
    bool hasAttacked;
    bool isDead;
    ZombieAI zombieAI;

    void Start()
    {
        if (player == null)
        {
            if(gameObject.CompareTag("Boss")) {
        Debug.Log(player);
            }
            player = GameObject.FindGameObjectWithTag("Player").transform;
                   if(gameObject.CompareTag("Boss")) {
        Debug.Log(player);
            }
        }

        currentState = FSMStates.Idle;

        enemyHealth = GetComponent<EnemyHealth>();
        health = enemyHealth.currentHealth;
        isDead = false;

        agent = GetComponent<NavMeshAgent>();
        zombieAI = GetComponent<ZombieAI>();

        InvokeRepeating("IncrementTimer", 0f, 1f);
    }

    void Update()
    {
        health = enemyHealth.currentHealth;
        disToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (currentState)
        {
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

        if (health <= 0)
        {
            if (gameObject.CompareTag("Boss"))
            {
                LevelManager.instance.PlayerWon();
            }
            currentState = FSMStates.Dead;
        }
    }

    void UpdateIdleState()
    {
        if (disToPlayer > Attackdistance && IsPlayerInClearPOV())
        {
            agent.isStopped = false;
            zombieAI.SetIdleAnimation(false);
            zombieAI.SetMovementAnimationTrigger();
            agent.SetDestination(player.position);
            currentState = FSMStates.Chase;
        }
    }

    void UpdateChaseState()
    {
        FaceTarget(player.transform.position);
        if (disToPlayer <= Attackdistance)
        {
            agent.isStopped = true;
            zombieAI.SetAttackAnimation(true);
            zombieAI.SetMovementAnimationTrigger();
            currentState = FSMStates.Attack;
        }
        else if(!IsPlayerInClearPOV())
        {
            agent.isStopped = true;
            zombieAI.SetIdleAnimation(true);
            zombieAI.SetMovementAnimationTrigger();
            currentState = FSMStates.Idle;
        }
        else if (timer % 10f == 0 && gameObject.CompareTag("Boss") && disToPlayer > Attackdistance)
        {
            agent.isStopped = true;
            zombieAI.SetAttackAnimation(true);
            zombieAI.SetMovementAnimationTrigger();
            currentState = FSMStates.Attack;
        }
    }

    void UpdateAttackState()
    {
        FaceTarget(player.transform.position);
        if (gameObject.CompareTag("Boss") && disToPlayer > Attackdistance)
        {
            GameObject attack = Instantiate(throwable,
                spawnpoint.transform.position + transform.forward, transform.rotation) as GameObject;
            Vector3 directionToPlayer = (player.position - spawnpoint.transform.position).normalized;
            Rigidbody rb = attack.GetComponent<Rigidbody>();
            rb.AddForce(directionToPlayer * 200, ForceMode.VelocityChange);

            hasAttacked = true;
            if (hasAttacked)
            {
                agent.isStopped = false;
                zombieAI.SetMovementAnimationTrigger();
                agent.SetDestination(player.position);
                zombieAI.SetAttackAnimation(false);
                currentState = FSMStates.Chase;

            }
        }
        if (disToPlayer > Attackdistance)
        {
            agent.isStopped = false;
            zombieAI.SetMovementAnimationTrigger();
            agent.SetDestination(player.position);
            zombieAI.SetAttackAnimation(false);
            currentState = FSMStates.Chase;

        } else if (!IsPlayerInClearPOV()) {
            zombieAI.SetAttackAnimation(false);
            zombieAI.SetIdleAnimation(true);
            currentState = FSMStates.Idle;
        }

        EnemyDamage();
    }

    void UpdateDeadState()
    {
        zombieAI.SetZeroHealthAnimation();
        isDead = true;
        Destroy(gameObject, 5);
    }

    void FaceTarget(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        directionToTarget.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    void EnemyDamage()
    {
        if (!isDead)
        {
            if (gameObject.CompareTag("Boss"))
            {

            }
            if (elapsedTime >= damRate && currentState == FSMStates.Attack)
            {
                Invoke("DoDamage", 0.3f);
                elapsedTime = 0.0f;
            }
        }
    }

    void DoDamage()
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.TakeDamage(damAmt);
    }

    void IncrementTimer()
    {
        timer++;
    }

        bool IsPlayerInClearPOV() {

        RaycastHit hit;
        Vector3 directToPlayer = player.transform.position - enemyEyes.position;
        if(Vector3.Angle(directToPlayer, enemyEyes.forward) <= FOV) {
            if(Physics.Raycast(enemyEyes.position, directToPlayer, out hit)) {
                if(hit.collider.CompareTag("Player")) {
                    print("player in sight");
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    } 

}
