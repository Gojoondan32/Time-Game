using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject comet;   
    [SerializeField] private int damageDealt = 10;

    //Temporary variables
    public Transform player;

    public Camera cam;

    private Transform tempTarget;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tempTarget = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            AttackEnemy();
        }
    }

    private void AttackEnemy()
    {
        LayerMask enemyMask = LayerMask.GetMask("Enemy");
        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, 2f, enemyMask);

        //Loop through each enemy in the area
        foreach(Collider enemy in enemiesHit)
        {
            enemy.GetComponent<Health>().TakeDamage(damageDealt);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        

        /*
        if (Input.GetButton("Fire2"))
        {
            Vector3 newDirection = (other.transform.position - player.position).normalized;
            player.rotation = Quaternion.Euler(newDirection);
            Debug.Log("Lock on");
        }

        */

    }
    public void OnTriggerEnter(Collider other)
    {
        
    }
}
