using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    
    [Header("Attack Variables")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private GameObject mortarBullet;

    private float cooldown = 1.0f;
    private int count = 3;
    private bool startWaitTime = true;
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
        //Spawn more enemies 
        Debug.Log("On low health");

    }

    //Override attack method from main script
    public override void AttackPlayer()
    {
        agent.isStopped = true;
        

        cooldown -= Time.deltaTime;
        //Start Railgun

        
        if(cooldown <= 0f && count > 0)
        {
            Instantiate(mortarBullet, spawn.position, Quaternion.identity);
            cooldown = 1f;
            count--;
        }
        
        if (!CanAttackPlayer())
        {
            state = EnemyState.chasing;
        }

        if (startWaitTime)
        {
            StartCoroutine(WaitTime());
        }
    }



    private IEnumerator WaitTime()
    {
        startWaitTime = false;
        yield return new WaitForSeconds(15f);

        count = 3;
        startWaitTime = true;
    }


    private void OnDrawGizmos()
    {


        
    }
}
