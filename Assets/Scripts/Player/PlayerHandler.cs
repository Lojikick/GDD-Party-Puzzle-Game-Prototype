using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private enum PlayerState { Idle, Walking, Interacting, Stunned }

    [Header("Components")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteraction interaction;

    [Header("Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Debug")]
    [SerializeField] private PlayerState playerState;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
    }

    private void Start()
    {
        playerState = PlayerState.Idle;
    }

    private void Update()
    {
        switch (playerState)
        {
            case PlayerState.Idle:

                // Movement
                movement.CheckForMovement();

                // Check for interactables
                interaction.SearchForInteractables();

                // Attempt to interact
                if (Input.GetKeyDown(interactKey))
                {
                    // If interaction attempt sucesses
                    if (interaction.AttemptInteraction())
                    {
                        // Stop any movement
                        movement.StopMovement();

                        // Change states
                        playerState = PlayerState.Interacting;
                    }
                }

                break;
            case PlayerState.Interacting:

                // Continue to check for interacting
                if (Input.GetKeyDown(interactKey))
                {
                    // If we fail
                    if (!interaction.AttemptInteraction())
                    {
                        // Change states
                        playerState = PlayerState.Idle;
                    }
                }

                break;
            case PlayerState.Stunned:

                break;
            default:
                // Debug
                print("Unimplemented state: " + playerState.ToString());
                break;
        }
    }
}
