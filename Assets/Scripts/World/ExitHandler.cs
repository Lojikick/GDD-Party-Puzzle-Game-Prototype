using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExitHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Collider2D collider2d;

    [Header("Debug")]
    [SerializeField] private bool debugMode;

    private void Awake()
    {
        collider2d = GetComponentInChildren<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (debugMode)
        {
            // Debug
            print("Exit was touched by: " + other.name);
        }
    }
}
