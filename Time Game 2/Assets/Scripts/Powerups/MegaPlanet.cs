using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaPlanet : MonoBehaviour
{
    [Header("References")]
    public static Transform target;
    protected Rigidbody rb;
    [SerializeField] private GameObject shockwave;
    [SerializeField] private GameObject explosionParticle;
    //[SerializeField] private GameObject impactParticle;

    [Header("Rigidbody")]
    public float force;
    public float rotationForce;


    [Header("Damage")]
    public int damageDealt = 50;
 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        /*
        GameObject tempParticle;
        tempParticle = Instantiate(impactParticle, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0))) as GameObject;
        tempParticle.transform.parent = this.gameObject.transform.parent;
        impactParticle.transform.parent = this.gameObject.transform.parent;
        */
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void FixedUpdate()
    {
        //Home in the target enemy
        if (target != null)
        {
            //Get the direction to the target
            Vector3 direction = (target.position - rb.position).normalized;

            //Get the angle to rotate towards the target
            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);

            rb.velocity = direction * force;

            rb.angularVelocity = rotationAmount * rotationForce;

            rb.velocity = transform.forward * force;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Get the point at where the planet collides with the ground
        ContactPoint contactPoint = collision.GetContact(0);
        Vector3 pos = contactPoint.point;
        Instantiate(shockwave, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        Destroy(Instantiate(explosionParticle, pos, Quaternion.Euler(new Vector3(0, 0, 0))), 2f);
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Deal damage to all enemies 
            collision.gameObject.GetComponent<Health>().TakeDamage(damageDealt);

            Destroy(transform.parent.gameObject);

        }
        
    }

    
}
