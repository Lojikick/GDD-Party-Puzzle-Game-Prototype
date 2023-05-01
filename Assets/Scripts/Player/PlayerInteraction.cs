using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Vector3 interactionOffset;

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + interactionOffset, interactionRadius);
    }
}
