using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // Enums for defining states of the enemy AI
 // Enums for defining states of the enemy AI
    public enum FSMStates
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    // Serialized fields for editor access
    [Header("AI State Management")]
    public FSMStates currentState;

    [Header("Attack Parameters")]
    public float AttackDistance = 4; // Distance at which the enemy can attack
    public GameObject throwable; // The object to throw at the player
    public GameObject spawnpoint; // Where the throwable is spawned

    [Header("Player Detection")]
    public Transform player; // Reference to the player transform
    public int damRate = 2; // Rate at which damage is inflicted
    public int damAmt = 10; // Amount of damage inflicted
    public int timer = 1; // Timer for actions
    public Transform enemyEyes; // The point from which the enemy detects the player
    public int FOV; // Field of view for detecting the player
    public int vicinityRadius; // Radius within which the player can be detected

    // Private variables
    private EnemyHealth enemyHealth; // Reference to enemy health component
    private int health; // Current health of the enemy
    private NavMeshAgent agent; // NavMesh agent for pathfinding
    private Vector3 nextDestination; // Next destination for the enemy
    private float disToPlayer; // Distance to the player
    private float elapsedTime = 0; // Timer for attacks
    private bool hasAttacked; // Track if the enemy has attacked
    private bool isDead; // Check if the enemy is dead
    private ZombieAI zombieAI; // Reference to the zombie AI component
    private bool hasDroppedPickup = false; // Track if the pickup has been dropped



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
        agent.isStopped = true;
        zombieAI = GetComponent<ZombieAI>();
        agent.isStopped = true;

        InvokeRepeating("IncrementTimer", 0f, 1f);
    }

    void Update()
    {
                agent.SetDestination(player.position);

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

    }

    void UpdateIdleState()
    {
        agent.isStopped = true;
        if ((disToPlayer > AttackDistance && IsPlayerInClearPOV()) 
        || IsPlayerInVicinity(vicinityRadius))
        {
            zombieAI.SetIdleAnimation(false);
            zombieAI.SetMovementAnimationTrigger();
            agent.SetDestination(player.position);
            agent.isStopped = false;
            currentState = FSMStates.Chase;
        }  else if (health <= 0) {
            currentState = FSMStates.Dead;
        }
    }

    void UpdateChaseState()
    {
        FaceTarget(player.transform.position);
        if (disToPlayer <= AttackDistance)
        {
            zombieAI.SetAttackAnimation(true);
            agent.isStopped = true;
            currentState = FSMStates.Attack;
        }
        else if(!IsPlayerInClearPOV()  && !IsPlayerInVicinity(vicinityRadius))
        {
            zombieAI.SetIdleAnimation(true);
            agent.isStopped = true;
            currentState = FSMStates.Idle;
        }
        else if (timer % 10f == 0 && gameObject.CompareTag("Boss") && disToPlayer > AttackDistance)
        {
            zombieAI.SetAttackAnimation(true);
            agent.isStopped = true;
            currentState = FSMStates.Attack;
        } else if (health <= 0) {
            currentState = FSMStates.Dead;
        }
    }

    void UpdateAttackState()
    {
        FaceTarget(player.transform.position);
        if (gameObject.CompareTag("Boss") && disToPlayer > AttackDistance && IsPlayerInClearPOV())
        {
            GameObject attack = Instantiate(throwable,
                spawnpoint.transform.position + transform.forward, transform.rotation) as GameObject;
            Vector3 directionToPlayer = (player.position - spawnpoint.transform.position).normalized;
            Rigidbody rb = attack.GetComponent<Rigidbody>();
            rb.AddForce(directionToPlayer * 200, ForceMode.VelocityChange);

            hasAttacked = true;
            if (hasAttacked)
            {
                zombieAI.SetMovementAnimationTrigger();
                agent.SetDestination(player.position);
                zombieAI.SetAttackAnimation(false);
                agent.isStopped = false;
                currentState = FSMStates.Chase;

            }
        }
        if (disToPlayer > AttackDistance)
        {

            zombieAI.SetMovementAnimationTrigger();
            agent.SetDestination(player.position);
            zombieAI.SetAttackAnimation(false);
            agent.isStopped = false;
            currentState = FSMStates.Chase;

        } else if (!IsPlayerInClearPOV() && !IsPlayerInVicinity(vicinityRadius)) {

            zombieAI.SetAttackAnimation(false);
            zombieAI.SetIdleAnimation(true);
            agent.isStopped = true;
            currentState = FSMStates.Idle;
        }  else if (health <= 0) {
            currentState = FSMStates.Dead;
        }

        EnemyDamage();
    }

void UpdateDeadState()
{
    agent.isStopped = true;
    zombieAI.SetZeroHealthAnimation();
    var enemyHealth = gameObject.GetComponent<EnemyHealth>();
    isDead = true;
    

    if (!hasDroppedPickup) {
        enemyHealth.DropPickup();
        hasDroppedPickup = true; 
        EnemySpawner.currZombieCount--;
    Debug.Log("current count" + EnemySpawner.currZombieCount);
    }

    Destroy(gameObject, 3);

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
if (AttackDistance >= 20 && elapsedTime >= damRate && currentState == FSMStates.Attack && IsPlayerInClearPOV()) 
{
    GameObject attack = Instantiate(throwable, spawnpoint.transform.position + transform.forward, transform.rotation) as GameObject;
    Vector3 directionToPlayer = (player.position - spawnpoint.transform.position).normalized;
    Rigidbody rb = attack.GetComponent<Rigidbody>();
    rb.AddForce(directionToPlayer * 100, ForceMode.VelocityChange);


    elapsedTime = 0.0f;
}
            else if (AttackDistance < 20 && elapsedTime >= damRate && currentState == FSMStates.Attack)
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

    bool IsPlayerInVicinity(int vicinityRadius) {
    Collider[] colliders = Physics.OverlapSphere(enemyEyes.position, vicinityRadius);
    
    foreach(Collider col in colliders) {
        if(col.CompareTag("Player")) {
            print("Player in vicinity");
            return true;
        }
    }
    
    return false;
}




}
