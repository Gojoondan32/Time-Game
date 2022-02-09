using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    private Rigidbody rb;

    [Header("Speed")]
    [SerializeField] private float force = 4f;

    [Header("Damage")]
    [SerializeField] private int damageDealt = 10;

    private bool bulletFired = false;
    [SerializeField] private bool canDealDamage = true;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player.transform;
        rb = GetComponent<Rigidbody>();
        bulletFired = false;
        canDealDamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, 6f);
    }
    private void FixedUpdate()
    {
        //Required to only set the targets position the instance that this script is initialised 
        if (!bulletFired)
        {
            Vector3 direction = (player.position - rb.position).normalized;
            
            //Move towards the player
            rb.velocity = direction * force;

            //Make the bullet face the player to avoid any weird bugs 
            gameObject.transform.LookAt(player.position);

            bulletFired = true;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit player");

            //Get the player's health
            Health playerHeath = collision.gameObject.GetComponent<Health>();
            if (canDealDamage)
            {
                playerHeath.TakeDamage(damageDealt);
                canDealDamage = false;
            }
            
            
        }
        Destroy(gameObject);
    }
}
