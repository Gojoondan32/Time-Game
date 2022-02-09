using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float nextAttackAllowed = -1.0f;

    [SerializeField] float attackDelay = 1.0f;

    [SerializeField] int damageDealt = 5;

    

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Time.time >= nextAttackAllowed)
        {
            Health playerHealth = other.GetComponent<Health>();
            //Deal damage to the player

            playerHealth.TakeDamage(damageDealt);
            nextAttackAllowed = Time.time + attackDelay;

            //Decrease the players combatScore back to 0
            PlayerAbility.comboScore = 0;

            Debug.Log("Attacking the player");
        }
    }
}
