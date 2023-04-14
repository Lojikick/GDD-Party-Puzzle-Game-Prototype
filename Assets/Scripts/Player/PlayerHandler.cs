using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private enum PlayerState { Idle, Walking, Talking, Reading }

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

                // Interaction
                if (Input.GetKeyDown(interactKey) && DialogueUI.instance.IsReady())
                {
                    // Look for any NPCs to talk to
                    bool found = interaction.CheckForInteraction();
                    if (found)
                    {
                        movement.StopMovement();

                        // Change state
                        playerState = PlayerState.Talking;
                    }

                    // Look for the cookbook
                    found = interaction.CheckForCookbook();
                    if (found)
                    {
                        movement.StopMovement();

                        // Change state
                        playerState = PlayerState.Reading;
                    }
                }

                break;
            case PlayerState.Talking:

                if (Input.GetKeyDown(interactKey))
                {
                    // Talk to NPC
                    bool finished = interaction.TalkWithNPC();
                    if (finished)
                    {
                        // Change state
                        playerState = PlayerState.Idle;
                    }
                }

                break;
            case PlayerState.Reading:

                if (Input.GetKeyDown(interactKey))
                {
                    interaction.CloseCookbook();

                    // Change state
                    playerState = PlayerState.Idle;
                }

                break;
            default:
                // Debug
                print("Unimplemented state: " + playerState.ToString());
                break;
        }
    }
}
