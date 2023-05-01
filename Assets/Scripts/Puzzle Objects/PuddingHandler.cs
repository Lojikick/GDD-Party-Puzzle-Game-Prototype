using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuddingHandler : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    [SerializeField] private float duration;

    [Header("Debugging")]
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.TryGetComponent(out PlayerMovement playerMovement))
        {
            // Save ref
            this.playerMovement = playerMovement;

            // Calculate direction
            Vector3 direction = (Vector2)playerMovement.facingDirection;

            // Bounce
            StartCoroutine(Bounce(playerMovement.transform, direction, duration));
        }
    }

    private IEnumerator Bounce(Transform target, Vector3 direction, float duration)
    {
        // Stun
        playerMovement.stunned = true;

        Vector3 startPos = transform.position + offset;
        Vector3 endPos = startPos + direction * distance;
        Vector3 controlPos = startPos + (endPos - startPos) / 2 + Vector3.up * height;

        float elapsed = 0;
        while (elapsed < duration)
        {
            float ratio = elapsed / duration;
            // Lerp position
            Vector3 m1 = Vector3.Lerp(startPos, controlPos, ratio);
            Vector3 m2 = Vector3.Lerp(controlPos, endPos, ratio);
            target.position = Vector3.Lerp(m1, m2, ratio);

            // Increment time
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set final values
        target.position = endPos;

        // Check if player jumped into the void
        if (PuzzleManager.instance.IfOutOfBounds(endPos))
        {
            // Reset
            TransitionManager.instance.ReloadScene();
        }
        else
        {
            // Free player
            playerMovement.stunned = false;
            playerMovement = null;
        }
    }
}
