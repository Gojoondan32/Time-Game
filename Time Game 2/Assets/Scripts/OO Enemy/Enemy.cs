using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IEnemyBehaviour
{

    public enum EnemyState { chasing, attacking, healing, idle };

    [Header("States")]
    protected EnemyState state = EnemyState.chasing;

    [Header("References")]
    protected Transform player;
    protected NavMeshAgent agent;
    protected Animator animator;
    [SerializeField] private GameObject smokeParticle;
    private Health enemyHealth;

    [Header("SearchCooldown")]
    private float searchCountdown = 1f;

    [Header("Rotation")]
    protected float rotationSpeed = 5f;


    [Header("AttackCooldown")]
    public float attackDistance = 30f;
    protected float nextAttackAllowed = -1.0f;
    protected float attackDelay = 1.0f;
    protected float attackCooldown = 0f;

    protected bool emergencyAttack = false;
    protected float attackTime = 1f;
    protected bool superAttack = false;


    [Header("Bullet")]
    ObjectPooler objectPooler;
    public Transform spawn;
    [SerializeField] private GameObject bulletPrefab;

    public bool smokeSignal = true;

    [Header("Healing")]
    protected bool canHeal = false;
    protected bool enragedMode = false;
    private float currentHealthPercentage;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();

        state = EnemyState.chasing;

        smokeSignal = true;

        emergencyAttack = false;

        canHeal = false;

        objectPooler = ObjectPooler.Instance;

        animator = this.gameObject.GetComponent<Animator>();

        enemyHealth = this.gameObject.GetComponent<Health>();

        //Set currentHealth to 20% of the enemies max HP
        currentHealthPercentage = this.enemyHealth.GetMaxHealth() * 0.2f;
        //Put it back on the floor
        //agent.height = 0.5f;
        //agent.baseOffset = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.GetComponent<Health>().GetHealth() <= currentHealthPercentage && enragedMode == false || canHeal)
        {
            if (smokeSignal)
            {
                //this.gameObject.transform.SetParent(smokeParticle.transform);
                Instantiate(smokeParticle, transform.position, Quaternion.Euler(new Vector3(270, 0, 0)));
                smokeSignal = false;
            }
            animator.SetBool("isFiring", false);
            state = EnemyState.healing;
            EnemyLowOnHealth();
            return;
        }

        if (player != null)
        {
            
            if (state == EnemyState.chasing)
            {
                Debug.Log("Chasing Player");
                ChasePlayer();
            }
            else if (state == EnemyState.attacking)
            {
                AttackPlayer();
            }

        }
        FacePlayer();
    }
    public void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        if (CanAttackPlayer())
        {
            //Debug.Log("Can attack player");
            state = EnemyState.attacking;

        }

    }

    public bool PlayerIsAlive()
    {
        searchCountdown -= Time.unscaledDeltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            //Check if the player is alive
            if (player == null)
            {
                return false;
            }
        }
        return true;
    }
    public virtual void AttackPlayer()
    {
        
        agent.isStopped = true;
        attackCooldown += Time.deltaTime;

        
        //Fire when the cooldown is up
        if (attackCooldown >= attackTime)
        {
            //Set cooldown back to 1
            attackCooldown = 0f;
            //objectPooler.SpawnFromPool("Bullet", spawn.position, transform.rotation)
            animator.SetBool("isFiring", true);

            //Spawn bullet
            Instantiate(bulletPrefab, spawn.position, transform.rotation);
        }

        //Debug.Log("Attacking player");
        if (!CanAttackPlayer())
        {
            //Debug.Log("Cannot attack player");
            animator.SetBool("isFiring", false);

            //Return to chase state
            state = EnemyState.chasing;

        }
    }
    public void FacePlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);

        direction.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }

    public virtual bool CanAttackPlayer()
    {

        
        float distance = (player.position - transform.position).magnitude;

        if (distance < attackDistance)
        {
            
            return true;
        }
        return false;
        
    }

    public abstract void EnemyLowOnHealth();

}
