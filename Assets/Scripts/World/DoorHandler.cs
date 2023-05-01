using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{
    private enum DoorState { Closed, Opening, Open, Closing }

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider2d;

    [Header("Settings")]
    [SerializeField] private DoorState state;
    [SerializeField] private float openingDuration = 1f;
    [SerializeField] private float autoOpenDistance = 1f;
    [SerializeField] private LayerMask playerLayer;

    private float openingTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider2d = GetComponent<Collider2D>();
    }

    private void Start()
    {
        state = DoorState.Closed;
        collider2d.enabled = true;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case DoorState.Closed:

                // If player is close enough to door
                if (IsPlayerInRange())
                {
                    // Start opening door
                    state = DoorState.Opening;
                    openingTimer = openingDuration;
                    animator.Play("Open");
                }

                break;
            case DoorState.Opening:

                // Count down timer
                if (openingTimer > 0) { openingTimer -= Time.deltaTime; }
                else
                {
                    // Set door to open
                    state = DoorState.Open;
                    // Disable collider
                    collider2d.enabled = false;
                }

                break;
            case DoorState.Open:

                // If player is far enough from door
                if (!IsPlayerInRange())
                {
                    // Start closing door
                    state = DoorState.Closing;
                    openingTimer = openingDuration;
                    animator.Play("Close");
                }

                break;
            case DoorState.Closing:

                // Count down timer
                if (openingTimer > 0) { openingTimer -= Time.deltaTime; }
                else
                {
                    // Set door to open
                    state = DoorState.Closed;
                    // Disable collider
                    collider2d.enabled = true;
                }

                break;
        }

    }

    bool IsPlayerInRange()
    {
        // Look for player
        return Physics2D.OverlapCircle(transform.position, autoOpenDistance, playerLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoOpenDistance);
    }
}
