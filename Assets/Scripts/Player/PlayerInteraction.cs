using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Vector3 interactionOffset;
    [SerializeField] private NPCHandler nPCHandler;

    [Header("Debugging")]
    [SerializeField] private Interactable interactable;

    public void SearchForInteractables()
    {
        // Look for any interactables in range
        var hit = Physics2D.OverlapCircle(transform.position + interactionOffset, interactionRadius, interactionLayer);
        if (hit && hit.TryGetComponent(out Interactable interactable))
        {
            // If same one, then do nothing
            if (this.interactable == interactable) return;

            // Save ref
            this.interactable = interactable;

            // Highlight it
            this.interactable.Highlight();
        }
        else
        {
            // IF we did have one before
            if (this.interactable != null)
            {
                // De-select it
                this.interactable.UnHighlight();
            }

            // Remove ref
            this.interactable = null;
        }
    }

    public bool AttemptInteraction()
    {
        // Do nothing if nothing to interact with
        if (interactable == null)
            return false;

        // Interact
        bool sucess = interactable.Interact();

        // Reset interaction
        if (!sucess) interactable = null;

        return sucess;
    }

    // public bool CheckForInteraction()
    // {
    //     // Look for NPC in range
    //     var hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
    //     if (hit && hit.TryGetComponent(out NPCHandler NPCHandler))
    //     {
    //         // Open dialogue with NPC
    //         var dialogue = NPCHandler.StartInteraction();
    //         DialogueUI.instance.Open(dialogue);

    //         // TODO FIX THIS
    //         nPCHandler = NPCHandler;

    //         return true;
    //     }

    //     return false;
    // }

    // public bool CheckForCookbook()
    // {
    //     // Look for NPC in range
    //     var hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
    //     if (hit && hit.TryGetComponent(out CookbookHandler cookbook))
    //     {
    //         // Open cookbook
    //         CookbookMenuUI.instance.Show();

    //         return true;
    //     }

    //     return false;
    // }

    // public bool TalkWithNPC()
    // {
    //     // If you are not talking to NPC, finish here
    //     if (nPCHandler == null) return true;

    //     // Increment message
    //     bool finished = DialogueUI.instance.NextMessage();
    //     if (finished)
    //     {
    //         nPCHandler = null;
    //     }

    //     // Return result
    //     return finished;
    // }

    // public void CloseCookbook()
    // {
    //     // Close cookbook
    //     CookbookMenuUI.instance.Hide();
    // }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + interactionOffset, interactionRadius);
    }
}
