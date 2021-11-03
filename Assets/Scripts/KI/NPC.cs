using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using SF = UnityEngine.SerializeField;
using System.Collections;

//defines the npc states
enum States
{ 
    Roam,                                                       //Roam around with no goal
    Patrole,                                                    //patrole the defined waypoints
    Chasing                                                     //chase a the player
}

public class NPC : MonoBehaviour
{
    [SF] public Transform Player;
    [SF] private NavMeshAgent Agent;
    [SF] private List<Transform> Waypoints = new List<Transform>();
    [SF] private int CurrentWaypoint = 0;
    [SF] private States CurrentState;
    [SF] private float roamOffset = 3;
    [SF] private float roamRadius = 4;
    [SF] private bool isRoaming = false;

    private void Awake()
    {
        if (Agent && Waypoints.Count > 0) Agent.SetDestination(Waypoints[CurrentWaypoint].position);
    }

    private void FixedUpdate()
    {
        if (Player)
        {
            if ((Player.position - transform.position).magnitude > 5.0f)
            {
                if (Waypoints.Count > 0)
                    Patrole();
                else if (isRoaming == false)
                    StartCoroutine(Roam());
            }
            else
                Chasing();
        }      
    }

    //patrole waypoints
    private void Patrole()
    {
        CurrentState = States.Patrole;

        if (Agent && Agent.remainingDistance < 0.5)
        {
            CurrentWaypoint++;
            if (CurrentWaypoint >= Waypoints.Count)
                CurrentWaypoint = 0;

            Agent.SetDestination(Waypoints[CurrentWaypoint].position);
        }
    }

    //Chase the player
    private void Chasing()
    {
        CurrentState = States.Chasing;
        if(Agent && Player) Agent.SetDestination(Player.position);
    }

    //roam randomly in a given circle
    private IEnumerator Roam()
    {
        CurrentState = States.Roam;

        isRoaming = true;
        while (CurrentState == States.Roam)
        {
            Vector3 p = UnityEngine.Random.insideUnitSphere * roamRadius;
            p.y = 0.0f;
            if (Agent == null) break;
            Agent.SetDestination(p + transform.position);
            yield return new WaitForSeconds(UnityEngine.Random.Range(roamOffset, roamOffset * 2));
        }

        isRoaming = false;
    }
}