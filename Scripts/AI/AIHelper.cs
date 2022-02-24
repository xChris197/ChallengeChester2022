using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIHelper : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 dest;

    private bool bReadyToMove = false;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Debug.Log(dest);
    }

    private void Update()
    {
        if (bReadyToMove)
        {
            bReadyToMove = false;
            MoveToDestination();
        }
        
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    CustomEvents.Scripts.OnDisableMovement?.Invoke(true);
                    CustomEvents.Scripts.OnDisableCamera?.Invoke(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Destroy(gameObject, 0.3f);
                }
            }
        }
    }

    private void MoveToDestination()
    {
        agent.SetDestination(dest);
        bReadyToMove = false;
    }
    public void SetDestination(Vector3 location)
    {
        dest = location;
        bReadyToMove = true;
    }

    private void OnEnable()
    {
        CustomEvents.AI.OnSetDestination += SetDestination;
    }
}
