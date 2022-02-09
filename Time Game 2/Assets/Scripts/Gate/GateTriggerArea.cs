using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTriggerArea : MonoBehaviour
{
    public int id;
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.GateTriggerEnter(id);
    }

    private void OnTriggerExit(Collider other)
    {

        GameEvents.current.GateTriggerExit(id);
    }
}
