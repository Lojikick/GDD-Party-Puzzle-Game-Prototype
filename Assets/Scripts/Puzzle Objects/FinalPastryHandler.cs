using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPastryHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Settings")]
    [SerializeField] private float offset;
    [SerializeField] private float duration;

    bool isCollected;

    private void Start()
    {
        // Hover animation
        LeanTween.moveLocalY(spriteRenderer.gameObject, offset, duration).setEaseInOutQuad().setLoopPingPong();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Do nothing if already collected
        if (isCollected) return;

        // Debug
        print("Collected!");

        // Change scene
        TransitionManager.instance.LoadSelectedScene(1);

        // Change state
        isCollected = true;
    }
}
