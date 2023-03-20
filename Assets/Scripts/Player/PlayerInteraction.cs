using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private bool isInteracting;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Look for NPC in range
            var hit = Physics2D.OverlapCircle(transform.position, interactionRadius, interactionLayer);
            if (hit && hit.TryGetComponent(out NPCHandler NPCHandler))
            {
                // Open dialogue with NPC
                var dialogue = NPCHandler.GetDialogue();
                DialogueUI.instance.Open(dialogue);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
