using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private string powerupName;
    [SerializeField] private GameObject pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        //Create and destroy particle effect
        Destroy(Instantiate(pickupEffect, transform.position, transform.rotation), 2);

        //PowerupManager powerupManager = PlayerManager.instance.player.GetComponent<PowerupManager>();
        //powerupManager.IdChecker(powerupName);

        PlayerPowerupHolder playerPowerupHolder = PlayerManager.instance.player.GetComponent<PlayerPowerupHolder>();
        playerPowerupHolder.UpdatePowerupList(powerupName);

        Destroy(gameObject);
    }
    
}
