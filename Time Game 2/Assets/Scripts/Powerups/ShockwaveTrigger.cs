using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveTrigger : Shockwave
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Happend");
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().TakeDamage(500f);
            
            Debug.Log(damageDealt.ToString());
        }
    }
}
