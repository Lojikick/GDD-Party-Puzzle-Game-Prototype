using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class NPCHandler : Interactable
{
    private enum NPCState { Idle, Roaming, Interacting, Stationary };

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;

    [Header("Data")]
    [SerializeField] private Dialogue dialogue;

    [Header("Settings")]
    [SerializeField] private float roamRadius;
    [SerializeField] private float idleDuration;

    [Header("Debugging")]
    [SerializeField] private NPCState state;
    [SerializeField] private DialogueUI ui;

    private float idleTimer;

    protected override void Awake()
    {
        base.Awake();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        // Set priority to a random value
        agent.avoidancePriority = Random.Range(1, 99);
    }

    private void Start()
    {
        // Cache UI
        ui = DialogueUI.instance;

        // Wait a random amount of time
        idleTimer = Random.Range(0, idleDuration);

        // Don't move
        state = NPCState.Stationary;
    }

    private void Update()
    {
        switch (state)
        {
            case NPCState.Stationary:
                // Default state

                break;
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

    public override bool Interact()
    {
        bool sucess;

        if (ui.IsOpen())
        {
            if (ui.IsDone())
            {
                // Close UI
                ui.Close();
                sucess = false;
            }
            else
            {
                // Increment message
                ui.NextMessage();
                sucess = true;
            }
        }
        else
        {
            // If UI is FULLY closed
            if (ui.IsClosed())
            {
                // Open dialogue UI
                var dialogue = GetProperDialogue();
                ui.Open(dialogue);
                sucess = true;
            }
            else if (ui.IsOpening())
            {
                sucess = true;
            }
            else
            {
                sucess = false;
            }
        }

        return sucess;
    }

    private void GoToRandomLocation()
    {
        // Find a random location around NPC within roam radius
        var position = transform.position + (Vector3)Random.insideUnitCircle * roamRadius;
        while (!agent.SetDestination(position))
            position = Random.insideUnitCircle * roamRadius;
    }

    private Dialogue GetProperDialogue()
    {
        // TODO
        // Decide dialogue based on game state

        return dialogue;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
}
