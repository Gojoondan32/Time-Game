using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MortarBullet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float force = 5.0f;
    [SerializeField] private float playerForce = 10f;
    [SerializeField] private float downwardsForce = 10f;
    private bool fireUp = true;
    private bool targetPlayer = true;

    [SerializeField] private float baseDamage = 30f;

    private Transform target;
    Vector3 direction = Vector3.zero;
    [SerializeField] private float cooldown = 1f;

    [SerializeField] private float trackingDistance = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        fireUp = true;
        targetPlayer = true;

    }

    private void FixedUpdate()
    {
        cooldown -= Time.deltaTime;
        //Fire the railgun straight up 
        if (fireUp)
        {
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            fireUp = false;
        }

        if (targetPlayer && cooldown <= 0)
        {
            target = PlayerManager.instance.player.transform;
            //Move the railgun bullet above the player
            Vector3 aboveDirection = new Vector3(target.position.x, target.position.y + 20f, target.position.z);
            direction = (aboveDirection - transform.position).normalized;
            rb.velocity = direction * playerForce;

            StartCoroutine(StopBullet());

            


            Debug.Log("Tracking player");
        }
    }

    private IEnumerator StopBullet()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Stoping the bullet");
        targetPlayer = false;

        Vector3 playerDirection = (target.position - transform.position).normalized;
        //StopTracking(playerDirection);

        rb.velocity = playerDirection * force;
    }

    private void StopTracking(Vector3 playerDirection)
    {
        if(playerDirection.magnitude <= trackingDistance)
        {
            playerDirection = Vector3.zero;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.instance.player.gameObject.GetComponent<Health>().TakeDamage(baseDamage);
        }
        Destroy(gameObject);
    }

}
