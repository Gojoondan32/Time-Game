using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowerupHolder : MonoBehaviour
{
    PowerupManager powerupManager;

    [SerializeField] private Image powerupHolderImage;

    [SerializeField] private List<string> powerupList = new List<string>();

    private void Start()
    {
        powerupManager = GetComponent<PowerupManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && powerupList.Count >= 1)
        {
            powerupManager.IdChecker(powerupList[0]);
            powerupList.Remove(powerupList[0]);
        }
    }

    public void UpdatePowerupList(string name)
    {
        if(powerupList.Count <= 0)
        {
            powerupList.Add(name);
        }
        Debug.Log("Poweruplist called");
    }
}
