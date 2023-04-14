using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesHandler : Mechanism
{
    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider2d;

    [Header("Debug")]
    [SerializeField] private bool debugMode;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        collider2d = GetComponentInChildren<Collider2D>();
    }

    public override void Enable()
    {
        // Show spikes
        Trigger();
    }

    public override void Disable()
    {
        // Remove spikes
        Retract();
    }

    private void Trigger()
    {
        // Play animation
        animator.Play("Trigger");

        // Enable contact
        collider2d.enabled = true;

        if (debugMode)
        {
            // Debug
            print("Spikes were triggered!");
        }
    }

    private void Retract()
    {
        // Play animation
        animator.Play("Retract");

        // Disable contact
        collider2d.enabled = false;

        if (debugMode)
        {
            // Debug
            print("Spikes were retracted!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the spikes are triggered, then do stuff
        // TODO
    }
}
