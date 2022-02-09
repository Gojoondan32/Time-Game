using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    Health playerHealth;
    private float speedBuff = 5f;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = PlayerManager.instance.player.GetComponent<Health>();
    }



    public void IdChecker(string name)
    {
        Debug.Log(name.ToString());
        
        switch (name)
        {
            case "Health":
                Debug.Log("Powerup 1 recieved");
                HealthBoost();
                break;
            case "Planet":
                PlanetAttack();
                break;
            case "Regen":
                StartCoroutine(RegenerateHealth());
                break;
            case "Speed":
                StartCoroutine(IncreaseSpeed());
                break;
            case "Windblade":
                Windblade();
                break;
        }

    }

    private IEnumerator RegenerateHealth()
    {
        
        int tickAmount = 10;
        //Get 2% of the players max health
        float healAmount = playerHealth.GetMaxHealth() * 0.02f;
        for(int i = 0; i < tickAmount; i++)
        {
            //Stop overcapping
            if (playerHealth.GetHealth() >= playerHealth.GetMaxHealth())
                yield break;
            
            playerHealth.Heal(healAmount);
            //Wait to heal 
            yield return new WaitForSeconds(3f);
        }
        Debug.Log("Regeneration complete");
    }

    private void HealthBoost()
    {
        //Heal 35% of health - Can be used to over heal
        float healAmount = playerHealth.GetMaxHealth() * 0.35f;

        playerHealth.Heal(healAmount);
        Debug.Log("Player health boost active");

    }

    private void PlanetAttack()
    {
        PlayerRangedCombat.planetAttack = true;
    }



    private IEnumerator IncreaseSpeed()
    {
        PlayerMovement playerMovement = PlayerManager.instance.player.gameObject.GetComponent<PlayerMovement>();
        playerMovement.speed += speedBuff;
        yield return new WaitForSeconds(10f);
        playerMovement.speed -= speedBuff;
    }

    private void Windblade()
    {
        PlayerCombat.windbladeActive = true;
    }

    private IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("Coroutine ran");
        
    }
}
