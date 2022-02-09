using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedCombat : MonoBehaviour
{
    [Header("References")]
    public GameObject cometPrefab;
    public GameObject largeCometPrefab;
    public GameObject planetPrefab;
    public GameObject supernovaPrefab;
    public Transform rangedAttackPoint;
    public Transform cometSpawn;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Attack Variables")]
    public float rangedAttackRange = 15f;
    [SerializeField] private float cometChargeLevel = 0f;
    [SerializeField] private bool isMouseDown = false;
    private bool largeCometCanSpawn = true;

    public static bool planetAttack = false;


    [Header("Layers")]
    private LayerMask enemyMasks;


    
    // Start is called before the first frame update
    void Start()
    {
        enemyMasks = LayerMask.GetMask("Enemies");
        cometChargeLevel = 0f;
        isMouseDown = false;
        largeCometCanSpawn = true;
        planetAttack = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log("Mouse1 pressed");
            cometChargeLevel += Time.deltaTime;

            if (cometChargeLevel <= 1f && !isMouseDown)
            {
                RangedAttack();
                isMouseDown = true;
            }
            
            
        }
        else
        {
            cometChargeLevel = 0;
            isMouseDown = false;
            
        }

        
    }

    private void RangedAttack()
    {
        
        if (GetCurrentTarget() != null)
        {
            playerMovement.ChargeEnemy(GetCurrentTarget());
            CometAttack.target = GetCurrentTarget();
            Debug.Log(GetCurrentTarget().ToString());
            if (planetAttack)
            {
                Instantiate(planetPrefab, cometSpawn.position, transform.rotation);
                planetAttack = false;
            }
            else
            {
                Instantiate(cometPrefab, cometSpawn.position, Quaternion.identity);
            }
            
            
            
        }
        

    }
    private void OnDrawGizmos()
    {
        if (rangedAttackPoint == null)
            return;

        //Gizmos.DrawWireSphere(rangedAttackPoint.position, rangedAttackRange);
    }

    public Transform GetCurrentTarget()
    {
        Transform newEnemy = null;
        float closest = 200f;
        
        //Get all enemies in range
        Collider[] enemiesHit = Physics.OverlapSphere(rangedAttackPoint.position, rangedAttackRange, enemyMasks);

        foreach (Collider enemies in enemiesHit)
        {
            Debug.Log("Finding enemies");
            float distance = (enemies.gameObject.transform.position - transform.position).magnitude;

            //Find the closest enemy and return its transform
            if(distance < closest)
            {
                closest = distance;
                newEnemy = enemies.transform;
            }
        }
        return newEnemy;


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(rangedAttackPoint.position, rangedAttackRange);
    }


}
