using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorHandler : Mechanism
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
        // Unlock this door
        Unlock();
    }

    public override void Disable()
    {
        // Lock this door
        Lock();
    }

    private void Unlock()
    {
        // Play animation
        animator.Play("Unlock");

        // Allow player to pass through
        collider2d.enabled = true;

        if (debugMode)
        {
            // Debug
            print("Locked door was unlocked.");
        }
    }

    private void Lock()
    {
        // Play animation
        animator.Play("Lock");

        // Prevent player from passing through
        collider2d.enabled = true;

        if (debugMode)
        {
            // Debug
            print("Locked door was locked.");
        }
    }
}
