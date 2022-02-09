using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//REFERENCE:
//  Used from Unity Learn

//Go back to add animations: 

public class State 
{
    public enum STATE
    {
        IDLE, PATROL, CHASE, ATTACK, HEAL
    };
    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    };

    public STATE name;
    protected EVENT stage;
    protected GameObject enemy;
    //Add animation variable here

    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    //Used to detect if the player is in range
    float visibleDistance = 30f;
    float visibleAngle = 60f;
    float visibleShootingDistance = 15f;

    public State(GameObject _enemy, NavMeshAgent _agent, Transform _player)
    {
        enemy = _enemy;
        agent = _agent;
        stage = EVENT.ENTER;
        player = _player;
    }

    public virtual void Enter() { stage = EVENT.UPDATE; }
    public virtual void Update() { stage = EVENT.UPDATE; }
    public virtual void Exit() { stage = EVENT.EXIT; }

    public State Process()
    {
        if (stage == EVENT.ENTER) Enter();
        if (stage == EVENT.UPDATE) Update();
        if(stage == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    public bool CanSeePlayer()
    {
        //Find the direction and distance to the player
        Vector3 direction = player.position - enemy.transform.position;
        //Find the angle the enemy can see
        float angle = Vector3.Angle(direction, enemy.transform.forward);

        if(direction.magnitude < visibleDistance && angle < visibleAngle)
        {
            return true;
        }
        return false;
    }

    public bool CanAttackPlayer()
    {
        //Find the direction and distance to the player
        Vector3 direction = player.position - enemy.transform.position;

        if(direction.magnitude < visibleShootingDistance)
        {
            return true;
        }
        return false;
    }
}

public class Idle : State
{
    public Idle(GameObject _enemy, NavMeshAgent _agent, Transform _player)
                : base(_enemy, _agent, _player)
    {
        name = STATE.IDLE;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (CanSeePlayer())
        {
            nextState = new Chase(enemy, agent, player);
            stage = EVENT.EXIT;
        }
        else if(Random.Range(0, 100) < 10)
        {
            nextState = new Patrol(enemy, agent, player);
            stage = EVENT.EXIT;
        }
        
    }
    public override void Exit()
    {
        base.Exit();
    }
}

public class Patrol : State
{
    private int currentIndex = -1;
    public Patrol (GameObject _enemy, NavMeshAgent _agent, Transform _player)
                : base(_enemy, _agent, _player)
    {
        name = STATE.PATROL;
        agent.speed = 2;
        agent.isStopped = false;
    }

    public override void Enter()
    {
        float lastDistance = Mathf.Infinity;
        for(int i = 0; i < WaypointSingleton.Singleton.Waypoints.Count; i++)
        {
            GameObject thisWP = WaypointSingleton.Singleton.Waypoints[i];
            float distance = Vector3.Distance(enemy.transform.position, thisWP.transform.position);

            if(distance < lastDistance)
            {
                currentIndex = i - 1;
                lastDistance = distance;
            }
        }
        base.Enter();
    }
    public override void Update()
    {
        if(agent.remainingDistance < 5f)
        {
            if(currentIndex >= WaypointSingleton.Singleton.Waypoints.Count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            agent.SetDestination(WaypointSingleton.Singleton.Waypoints[currentIndex].transform.position);
        }
        else if (CanSeePlayer())
        {
            nextState = new Chase(enemy, agent, player);
            stage = EVENT.EXIT;
        }

    }
    public override void Exit()
    {
        base.Exit();
    }

}

public class Chase : State
{
    public Chase (GameObject _enemy, NavMeshAgent _agent, Transform _player)
                : base(_enemy, _agent, _player)
    {
        name = STATE.CHASE;
        agent.speed = 10;
        agent.isStopped = false;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        agent.SetDestination(player.position);
        if (agent.hasPath)
        {
            if (CanAttackPlayer())
            {
                nextState = new Attack(enemy, agent, player);
                stage = EVENT.EXIT;
            }
            else if (!CanSeePlayer())
            {
                nextState = new Patrol(enemy, agent, player);
                stage = EVENT.EXIT;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}

public class Attack : State
{
    float rotationSpeed = 2.0f;
    public Attack(GameObject _enemy, NavMeshAgent _agent, Transform _player)
                : base(_enemy, _agent, _player)
    {
        name = STATE.ATTACK;
        //Add audio source here
    }

    public override void Enter()
    {
        agent.isStopped = true;
        base.Enter();
    }
    public override void Update()
    {
        Vector3 direction = player.position - enemy.transform.position;
        float angle = Vector3.Angle(direction, enemy.transform.forward);

        direction.y = 0;

        //Rotate the enemy to face the player
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);

        if (!CanAttackPlayer())
        {
            nextState = new Idle(enemy, agent, player);
            stage = EVENT.EXIT;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
