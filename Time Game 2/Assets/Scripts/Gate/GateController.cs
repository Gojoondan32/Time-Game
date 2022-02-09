using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public int id;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onGateTriggerEnter += OnGateOpen;
        GameEvents.current.onGateTriggerExit += OnGateClose;
    }

    private void OnGateOpen(int id)
    {
        if(id == this.id)
        {
            LeanTween.moveLocalY(this.gameObject, 36f, 2f).setEaseOutQuad();
        }
        
    }

    private void OnGateClose(int id)
    {
        if (id == this.id)
        {
            LeanTween.moveLocalY(this.gameObject, 14.8f, 2f).setEaseInQuad();
        }
        
    }
}
