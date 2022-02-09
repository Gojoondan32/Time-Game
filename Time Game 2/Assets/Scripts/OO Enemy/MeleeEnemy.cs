using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{

    [Header("Attack Variables")]
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float damage = 10f;


    private float healthTimer = 0f;
    private float regenAmount = 5f;
    private float regenSpeed = 0.5f;

    public override bool CanAttackPlayer()
    {
        float distance = (player.position - transform.position).magnitude;


        if (distance < attackDistance)
        {
            
            return true;
        }
        return false;
    }

    public override void EnemyLowOnHealth()
    {
        if (DoubleDown())
        {
            EnrangedMode();
        }
        else
        {
            Debug.Log("Healing robot");
            Regenerate();
            RunAway();
        }
        
    }



    //Override attack method from main script
    public override void AttackPlayer()
    {
        /*
        LayerMask playerMask = LayerMask.GetMask("Player");
        //Get all the players in range in the case of multiple players active
        Collider[] playerInRange = Physics.OverlapSphere(spawn.position, attackDistance, playerMask);
        
        foreach (Collider player in playerInRange)
        {
            if(Time.time >= nextAttackAllowed)
            {
                animator.SetBool("isFiring", true);
                player.gameObject.GetComponent<Health>().TakeDamage(damage);
                nextAttackAllowed = Time.time + attackDelay;
            }
            
        }
        */
        //Get the distance between the player and the enemy
        float attackDistance = (player.transform.position - transform.position).magnitude;
        if(attackDistance <= 4f )
        {
            //Testing new functionality - remove later
            Vector3 reverseDirection = (transform.position - player.transform.position).normalized;

            if(Time.time >= nextAttackAllowed)
            {

                animator.SetBool("isFiring", true);
                player.gameObject.GetComponent<Health>().TakeDamage(damage);
                nextAttackAllowed = Time.time + attackDelay;
                Debug.Log("Player in attack range");
                
            }

            //player.gameObject.GetComponent<Health>().TakeDamage(damage);
            
            //StartCoroutine(AttackCooldown(reverseDirection));
            Debug.Log(attackDistance.ToString());
        }

        
        if (!CanAttackPlayer())
        {
            //Return to the chasing state
            state = EnemyState.chasing;
            animator.SetBool("isFiring", false);
        }

    }

    private IEnumerator AttackCooldown(Vector3 direction)
    {
        
        yield return new WaitForSeconds(1f);
    }
    void EnrangedMode()
    {
        enragedMode = true;
        agent.speed = 20f;
        attackDelay = 0.3f;
        state = EnemyState.chasing;
        Debug.Log(state.ToString());
    }

    void Regenerate()
    {
        canHeal = true;
        healthTimer += Time.deltaTime;
        
        if (healthTimer > regenSpeed)
        {
            Debug.Log("Regening");
            this.gameObject.GetComponent<Health>().Heal(regenAmount);

            healthTimer = 0f;
        }

        if (this.gameObject.GetComponent<Health>().GetHealth() > 50)
        {
            state = EnemyState.chasing;
            canHeal = false;
        }
    }

    void RunAway()
    {
        Vector3 direction = transform.position - player.position;
        float angle = Vector3.Angle(direction, transform.forward);

        direction.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        transform.position += transform.forward * agent.speed * 1.5f * Time.deltaTime;
    }

    bool DoubleDown()
    {
        float playerCurrentHealth = PlayerManager.instance.player.GetComponent<Health>().GetHealth();
        if (playerCurrentHealth <= 30f)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        

        Gizmos.DrawWireSphere(spawn.position, attackRadius);
    }
}
