using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Debug")]
    [SerializeField] private bool debugMode;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Play animation
        animator.Play("Pressed");

        if (debugMode)
        {
            // Debug
            print("Plate were pressed by: " + other.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Play animation
        animator.Play("Released");

        if (debugMode)
        {
            // Debug
            print("Plate were released by: " + other.name);
        }
    }
}
