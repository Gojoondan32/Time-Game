using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolEnemy : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    State currentState;
    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, agent, player);
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Process();

        if(currentState.CanAttackPlayer())
        {
            //Debug.Log("Attacking player");
        }
    }
}
