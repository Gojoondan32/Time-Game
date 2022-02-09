using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        float playerMaxHealth = this.gameObject.GetComponent<Health>().GetMaxHealth();
        healthBar.SetMaxHealth(playerMaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
