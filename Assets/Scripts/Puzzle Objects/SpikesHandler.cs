using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesHandler : MonoBehaviour
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
        animator.Play("Triggered");

        if (debugMode)
        {
            // Debug
            print("Spikes were triggered by: " + other.name);
        }
    }
}
