using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class NPCHandler : MonoBehaviour
{
    private enum NPCState { Idle, Roaming, Interacting };

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Data")]
    [SerializeField] private Dialogue dialogue;

    [Header("Settings")]
    [SerializeField] private float roamRadius;
    [SerializeField] private float idleDuration;

    [Header("Debugging")]
    [SerializeField] private NPCState state;

    private float idleTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        // Inital state is idle
        idleTimer = idleDuration;
        state = NPCState.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case NPCState.Idle:
                // Stand still
                idleTimer -= Time.deltaTime;

                // When NPC is finished idling
                if (idleTimer <= 0)
                {
                    // Find new location
                    GoToRandomLocation();
                    state = NPCState.Roaming;
                }

                break;
            case NPCState.Roaming:

                // Movement is handled by agent script

                // If close to end point
                if (agent.remainingDistance < 0.1f)
                {
                    // Start idling
                    idleTimer = idleDuration;
                    state = NPCState.Idle;
                }

                break;
            case NPCState.Interacting:
                // TODO

                break;
            default:
                throw new System.Exception("STATE NOT FOUND: " + state);
        }

    }

    private void GoToRandomLocation()
    {
        // Find a random location around NPC within roam radius
        var position = transform.position + (Vector3)Random.insideUnitCircle * roamRadius;
        while (!agent.SetDestination(position))
            position = Random.insideUnitCircle * roamRadius;
    }

    public Dialogue StartInteraction()
    {
        // Stop agent from moving
        agent.isStopped = true;

        // Change state
        state = NPCState.Interacting;

        // Return approperiate dialogue
        return dialogue;
    }

    public void EndInteraction()
    {
        // Set idle timer
        idleTimer = idleDuration;

        // Change state
        state = NPCState.Idle;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}
