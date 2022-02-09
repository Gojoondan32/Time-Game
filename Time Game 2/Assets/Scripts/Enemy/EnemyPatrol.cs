using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform[] waypoints;
    [SerializeField] private int currentPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        PickWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        //Pick the next waypoint before the agent fully reaches the destination
        if(agent.remainingDistance < 10f)
        {
            PickWaypoint();
        }
    }
    private void PickWaypoint()
    {
        //Don't run if the array is empty
        if (waypoints.Length == 0)
            return;

        
        
        //Reset the array back to 0
        if(currentPoint == waypoints.Length - 1)
        {
            currentPoint = 0;
        }
        else
        {
            //Set the next point
            currentPoint++;
        }

        //Move the agent to the designated waypoint
        agent.SetDestination(waypoints[currentPoint].position);
        Debug.Log("Moving to: " + waypoints[currentPoint].ToString());

    }

}
