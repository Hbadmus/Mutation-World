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
    public int vicinityRadius;
   
    EnemyHealth enemyHealth; 
    int health;
    NavMeshAgent agent;
    Vector3 nextDestination;
    float disToPlayer;
    float elapsedTime = 0;
    bool hasAttacked;
    bool isDead;
    ZombieAI zombieAI;
    bool hasDroppedPickup = false;

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
        agent.isStopped = true;

        InvokeRepeating("IncrementTimer", 0f, 1f);
    }

    void Update()
    {
        health = enemyHealth.currentHealth;
        disToPlayer = Vector3.Distance(transform.position, player.transform.position);
   
     if (health <= 0)
        {
            if (gameObject.CompareTag("Boss"))
            {
                //LevelManager.instance.PlayerWon();
            }

            currentState = FSMStates.Dead;
        }
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
        if ((disToPlayer > Attackdistance && IsPlayerInClearPOV()) 
        || IsPlayerInVicinity(vicinityRadius))
        {
            zombieAI.SetIdleAnimation(false);
            zombieAI.SetMovementAnimationTrigger();
            agent.SetDestination(player.position);
            agent.isStopped = false;
            currentState = FSMStates.Chase;
        }
    }

    void UpdateChaseState()
    {
        FaceTarget(player.transform.position);
        if (disToPlayer <= Attackdistance)
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
        else if (timer % 10f == 0 && gameObject.CompareTag("Boss") && disToPlayer > Attackdistance)
        {
            zombieAI.SetAttackAnimation(true);
            agent.isStopped = true;
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
                zombieAI.SetMovementAnimationTrigger();
                agent.SetDestination(player.position);
                zombieAI.SetAttackAnimation(false);
                agent.isStopped = false;
                currentState = FSMStates.Chase;

            }
        }
        if (disToPlayer > Attackdistance)
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
        }

        EnemyDamage();
    }

void UpdateDeadState()
{
    zombieAI.SetZeroHealthAnimation();
    var enemyHealth = gameObject.GetComponent<EnemyHealth>();
    isDead = true;

    if (!hasDroppedPickup) {
        enemyHealth.DropPickup();
        hasDroppedPickup = true; 
    }

    Destroy(gameObject, 2);
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
