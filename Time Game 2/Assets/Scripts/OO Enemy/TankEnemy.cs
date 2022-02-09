using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : Enemy
{
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private float damageDealt = 2f;
    [SerializeField] private float tornadoRadius = 5f;
    [SerializeField] private float tornadoSpeed = 2f;
    public override bool CanAttackPlayer()
    {
        return base.CanAttackPlayer();

    }
    public override void AttackPlayer()
    {
        LayerMask playerMask = LayerMask.GetMask("Player");

        Collider[] playerHit = Physics.OverlapSphere(spawn.position, attackRadius, playerMask);

        foreach (Collider player in playerHit)
        {
            if(Time.time >= nextAttackAllowed)
            {
                player.gameObject.GetComponent<Health>().TakeDamage(damageDealt);
                nextAttackAllowed = Time.time + attackDelay;
            }
            
        }
        if (!CanAttackPlayer())
        {
            state = EnemyState.chasing;
        }
    }

    public override void EnemyLowOnHealth()
    {
        agent.isStopped = true;
        //Explode when on low health
        //DefensiveStance();
        Tornado();
    }

    private void Tornado()
    {
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, tornadoRadius, playerMask);

        foreach (Collider player in playerInRange)
        {
            Vector3 direction = (this.gameObject.transform.position - player.transform.position).normalized;
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            playerMovement.canDash = false;
            player.GetComponent<CharacterController>().Move(direction * tornadoSpeed * Time.deltaTime);

            StartCoroutine(ResumeDash(playerMovement));
            Debug.Log("Pulling player in");
        }
    }
    private IEnumerator ResumeDash(PlayerMovement player)
    {
        yield return new WaitForSeconds(7f);
        Debug.Log("Can dash");
        player.canDash = true;
    }
    private void DefensiveStance()
    {
        float defenseRadius = 10f;
        LayerMask enemyMask = LayerMask.GetMask("Enemies");
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, defenseRadius, enemyMask);
        
        foreach(Collider enemy in enemiesInRange)
        {
            if(enemy != this.gameObject)
            {
                enemy.GetComponent<Health>().Heal(1);
                Debug.Log("Healing Allies");
                
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(spawn.position, attackRadius);
        Gizmos.DrawWireSphere(transform.position, tornadoRadius);
    }
}
