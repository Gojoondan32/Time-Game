using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField]private float currentHealth = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        //Set the current health to the max health at the start of the game
        currentHealth = maxHealth;
        
    }

    //Determine if the object has been killed
    public bool hasDied { get { return currentHealth <= 0; } }

    
    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    //Damage formula
    public void TakeDamage(float damageValue)
    {
        currentHealth -= damageValue;

        /*
        if(this.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<PlayerHealth>().healthBar.SetHealth(currentHealth);
        }
        */


        if(this.gameObject.tag != "Player")
        {
            DamagePopup.Create(transform.position, damageValue);
        }
        if(currentHealth <= 0)
        {
            Debug.Log("This unit has died... ");
            Destroy(gameObject);
        }
    }
    public void Heal(float healValue)
    {
        currentHealth += healValue;
        Debug.Log("Healing");
    }

}
