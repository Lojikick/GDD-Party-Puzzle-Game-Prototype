using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Mechanism")]
    [SerializeField] private Mechanism mechanism;

    [Header("Debug")]
    [SerializeField] private bool debugMode;

    private bool isPressed;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed)
        {
            // Play animation
            animator.Play("Pressed");

            // Debug
            if (debugMode) print("Button were pressed by: " + other.name);

            // If there is a connected mechanism, enable it, PERMANENTLY
            if (mechanism != null)
            {
                mechanism.Enable();
            }
            else
            {
                // Debug
                if (debugMode) print("No mechanism connected to " + name);
            }

            // Set state
            isPressed = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (mechanism != null)
        {
            // Draw a line between this and it's connected mechanism
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, mechanism.transform.position);
        }
    }
}
