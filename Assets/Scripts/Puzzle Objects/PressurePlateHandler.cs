using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Mechanism")]
    [SerializeField] private Mechanism mechanism;

    [Header("Debug")]
    [SerializeField] private bool debugMode;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Pressed!");
        // Play animation
        animator.Play("Pressed");

        // Debug
        if (debugMode) print("Plate were pressed by: " + other.name);

        // If there is a connected mechanism, enable it
        if (mechanism != null)
        {
            mechanism.Enable();
        } 
        else
        {
            // Debug
            if (debugMode) print("No mechanism connected to " + name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Play animation
        animator.Play("Released");

        // Debug
        if (debugMode) print("Plate were released by: " + other.name);

        // If there is a connected mechanism, disable it
        if (mechanism != null)
        {
            mechanism.Disable();
        }
        else 
        {
            if (debugMode) print("No mechanism connected to " + name);
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
