using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{

    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<int> onGateTriggerEnter;

    public void GateTriggerEnter(int id)
    {
        if(onGateTriggerEnter != null)
        {
            onGateTriggerEnter(id);
        }
    }

    public event Action<int> onGateTriggerExit;

    public void GateTriggerExit(int id)
    {
        if(onGateTriggerExit != null)
        {
            onGateTriggerExit(id);
        }
    }

}
