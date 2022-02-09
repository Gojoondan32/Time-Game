using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private float explosionCountdown = 0f;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float baseDamage = 20f;
    public override bool CanAttackPlayer()
    {
        return base.CanAttackPlayer();

    }
    public override void AttackPlayer()
    {
        base.AttackPlayer();
    }

    public override void EnemyLowOnHealth()
    {

        //Explode when on low health
        Explode();
    }

    

    void Explode()
    {
        LayerMask playerMask = LayerMask.GetMask("Player");
        explosionCountdown += Time.deltaTime;

        //Increase explosion radius proportional to the countdown
        explosionRadius = explosionCountdown * 4;

        if(explosionCountdown >= 3f)
        {
            Collider[] explosionZone = Physics.OverlapSphere(transform.position, explosionRadius, playerMask);

            //Loop through all players in the area and deal damage based on distance
            foreach (Collider playerInZone in explosionZone)
            {
                float distanceFromExplosion = (playerInZone.transform.position - transform.position).magnitude;
                //Calculate damage
                baseDamage = (1 / distanceFromExplosion) * 100;
                playerInZone.gameObject.GetComponent<Health>().TakeDamage(baseDamage);
            }
            //Destroy this game object after the sequence has ended
            this.gameObject.GetComponent<Health>().TakeDamage(1000f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }


}
