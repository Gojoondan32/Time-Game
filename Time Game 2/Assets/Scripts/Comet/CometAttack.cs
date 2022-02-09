using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CometAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public static Transform target;
    protected Rigidbody rb;
    [SerializeField] private GameObject megaPlanet;

    [Header("Rigidbody")]
    public float force;
    public float rotationForce;
    [SerializeField] private float offestY = 2f;
    [SerializeField] private float offestX = 2f;
    
    [Header("Damage")]
    public int damageDealt = 5;

    private bool cometChosenDirection = false;
    PlayerManager player;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //target = GameObject.FindGameObjectWithTag("Enemy").transform;
        cometChosenDirection = false;
        player = PlayerManager.instance;
    }

    private void Update()
    {
        Destroy(gameObject, 5);
        
    }

    

    public void FixedUpdate()
    {
        //Home in the target enemy
        if(target != null)
        {
            //Get the direction to the target
            Vector3 direction = (target.position - rb.position).normalized;

            //Get the angle to rotate towards the target
            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);

            rb.velocity = direction * force;

            //rb.angularVelocity = rotationAmount * rotationForce;

            //rb.velocity = transform.forward * force;
        }
        
        
    }

    

    public void OnCollisionEnter(Collision collision)
    {
        //Find out if the comet collided with an enemy
        if(collision.gameObject.tag == "Enemy")
        {
            //Deal damage to that enemy
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth.TakeDamage(damageDealt);

            //Increase the players combat bar
            PlayerAbility.comboScore += 1;
            
            if(this.gameObject.tag == "Planet")
            {
                SpawnMegaPlanet(collision.gameObject.transform);
            }

            Destroy(gameObject);
        }

    }

    private void SpawnMegaPlanet(Transform target)
    {
        //Calculate offsets for the planets position
        Debug.Log("Planet Called");
        //offestX = Random.Range(-170, -500);
        offestY = Random.Range(90, 120);
        Vector3 planetSpawn = new Vector3(transform.position.x, transform.position.y + offestY, transform.position.z);

        GameObject tempPlanet = Instantiate(megaPlanet, planetSpawn, Quaternion.identity);
        
        //Pass the hit enemy's position to the main planet
        MegaPlanet.target = target.gameObject.transform;
    }
    

}
