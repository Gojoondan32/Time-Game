using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Transform meleeAttackPoint;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int damageDealt = 10;

    public static bool windbladeActive = false;

    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerRangedCombat playerRangedCombat;

    [SerializeField] private GameObject windblade;

    // Update is called once per frame
    void Update()
    {
        //Called when the player presses the right mouse button
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (windbladeActive)
            {
                case true:
                    WindbladeAttack();
                    break;
                case false:
                    CheckEnemyDistance();
                    break;
            }
            //Attack();
            
        }
    }
    private void CheckEnemyDistance()
    {
        Transform enemyToTarget = null;
        float closest = 100f;
        float distance = 0f;
        Collider[] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            distance = (enemy.gameObject.transform.position - gameObject.transform.position).magnitude;

            if(distance < closest)
            {
                closest = distance;
                enemyToTarget = enemy.transform;
            }
        }
        
        if(enemyToTarget != null)
        {
            playerMovement.ChargeEnemy(enemyToTarget);
            Attack(distance, enemyToTarget);
            Debug.Log("Enemy found");
        }
        
    }

    private void SolarWindAttack(Transform enemy, float distance)
    {
        int charges = 5;
        float damage = 30f;

        if(distance < playerRangedCombat.rangedAttackRange)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            enemyHealth.TakeDamage(damage);

            PlayerAbility.comboScore += 1;
            charges--;
        }
    }

    private void Attack(float distance, Transform enemy)
    {
        if(distance <= attackRange)
        {
            //Deal damage to all enemies in range of the attack
            Health enemyHealth = enemy.GetComponent<Health>();

            enemyHealth.TakeDamage(damageDealt);

            //Increase the players combatScore
            PlayerAbility.comboScore += 1;

            Debug.Log("Hit enemy" + enemy.name);
        }


    }

    private void WindbladeAttack()
    {
        Instantiate(windblade, playerRangedCombat.cometSpawn.position, Quaternion.Euler(90f, 0, 0));
        windbladeActive = false;
    }

    private void OnDrawGizmos()
    {
        //Used for debugging purposes to see the radius of the overlap sphere
        if (meleeAttackPoint == null)
            return;

        Gizmos.DrawWireSphere(meleeAttackPoint.position, attackRange);
    }
    
}
