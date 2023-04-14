using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private NPCHandler nPCHandler;

    public bool CheckForInteraction()
    {
        // Look for NPC in range
        var hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
        if (hit && hit.TryGetComponent(out NPCHandler NPCHandler))
        {
            // Open dialogue with NPC
            var dialogue = NPCHandler.StartInteraction();
            DialogueUI.instance.Open(dialogue);

            // TODO FIX THIS
            nPCHandler = NPCHandler;

            return true;
        }

        return false;
    }

    public bool CheckForCookbook()
    {
        // Look for NPC in range
        var hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
        if (hit && hit.TryGetComponent(out CookbookHandler cookbook))
        {
            // Open cookbook
            CookbookMenuUI.instance.Show();

            return true;
        }

        return false;
    }

    public bool TalkWithNPC()
    {
        // If you are not talking to NPC, finish here
        if (nPCHandler == null) return true;

        // Increment message
        bool finished = DialogueUI.instance.NextMessage();
        if (finished)
        {
            nPCHandler = null;
        }

        // Return result
        return finished;
    }

    public void CloseCookbook()
    {
        // Close cookbook
        CookbookMenuUI.instance.Hide();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
