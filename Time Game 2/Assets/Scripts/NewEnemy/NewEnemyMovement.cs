using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewEnemyMovement : MonoBehaviour
{
    
    public enum EnemyState { chasing, attacking, healing, idle};

    [Header("States")]
    private EnemyState state = EnemyState.chasing;

    [Header("References")]
    private Transform player;
    private NavMeshAgent agent;

    
    [Header("SearchCooldown")]
    private float searchCountdown = 1f;

    [Header("Rotation")]
    private float rotationSpeed = 5f;


    [Header("AttackCooldown")]
    private float attackDistance = 30f;
    private float nextAttackAllowed = -1.0f;
    private float attackDelay = 1.0f;
    private float attackCooldown = 1f;

    private bool emergencyAttack = false;


    [Header("Bullet")]
    public GameObject bullet;
    [SerializeField] private Transform spawn;

    [Header("Healing")]
    private float radius = 10f;
    [SerializeField]private bool enemiesInRange = false;
    public LayerMask enemyMask;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        state = EnemyState.chasing;

        enemiesInRange = false;

        emergencyAttack = false;

        //Put it back on the floor
        //agent.height = 0.5f;
        //agent.baseOffset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            //Checks the current state every frame and calls the appropriate function
            if(state == EnemyState.chasing)
            {
                ChasePlayer();
            }
            else if (state == EnemyState.attacking)
            {
                AttackPlayer();
            }
            
        }
        FacePlayer();
        //HealRobot();
    }
    private void ChasePlayer()
    {
        agent.isStopped = false;
        //Chase the player
        agent.SetDestination(player.position);
        if (CanAttackPlayer())
        {
            //Switch to attack state
            state = EnemyState.attacking;
            
        }
        
    }

    private bool PlayerIsAlive()
    {
        //Checks if the player is alive every second since it doesn't need to be every frame
        searchCountdown -= Time.unscaledDeltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            //Check if the player is alive
            if(player == null)
            {
                return false;
            }
        }
        return true;
    }
    private void AttackPlayer()
    {
        float attackTime;
        agent.isStopped = true;
        attackCooldown -= Time.deltaTime;

        //Attack quicker if the player is closer
        if (emergencyAttack)
        {
            attackTime = 0.2f;
        }
        else
        {
            attackTime = 1f;
        }

        if(attackCooldown <= 0f)
        {
            //Set cooldown back to 1
            attackCooldown = attackTime;
            Instantiate(bullet, spawn.position, Quaternion.identity);
        }

        if (!CanAttackPlayer())
        {
            //Chase the player when it can no longer attack
            state = EnemyState.chasing;
            
        }
    }
    private void FacePlayer()
    {
        //Calculate the angle and direction to the player
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        direction.y = 0;

        //Turn towards the calculated direction
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }
    
    private bool CanAttackPlayer()
    {
        //Get the distance between the enemy and the player
        float distance = (player.position - transform.position).magnitude;
        
        if (distance < attackDistance)
        {
            //Use a better attack if the player is closer 
            if(distance < attackDistance / 2)
            {
                emergencyAttack = true;
            }
            else
            {
                emergencyAttack = false;
            }
            return true;
        }
        return false;
    }

    private void HealRobot()
    {
        // THIS CODE IS NOT BEING USED
        Collider[] enemiesHurt = Physics.OverlapSphere(transform.position, radius, enemyMask);

        if(enemiesHurt.Length <= 1)
        {
            enemiesInRange = false;
            Debug.Log("No enemies in range");
            
        }
        else
        {
            enemiesInRange = true;
        }


        if (enemiesInRange)
        {
            foreach (Collider enemies in enemiesHurt)
            {
                Debug.Log("Healing Allies");
                if(enemies != gameObject)
                {
                    Health enemyHealth = enemies.GetComponent<Health>();
                    enemyHealth.TakeDamage(1);
                }
                
                
                
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
