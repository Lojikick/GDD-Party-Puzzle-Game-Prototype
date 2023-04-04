using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private bool isInteracting;
    [SerializeField] private NPCHandler nPCHandler;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {  
            // If you are talking to an NPC, forward dialogue
            if (isInteracting)
            {
                // TODO
            }
            // Check for any NPCs around to talk to
            else 
            {
                // Look for NPC in range
                var hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
                if (hit && hit.TryGetComponent(out NPCHandler NPCHandler))
                {
                    // Open dialogue with NPC
                    var dialogue = NPCHandler.StartInteraction();
                    DialogueUI.instance.Open(dialogue);

                    // TODO FIX THIS
                    isInteracting = true;
                    nPCHandler = NPCHandler;
                }
            }

            
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
