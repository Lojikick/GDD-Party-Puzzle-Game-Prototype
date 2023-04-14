using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateHandler : MonoBehaviour
{
    /*
    [YOUR REFERENCES/VARIABLES HERE]
    // Use Mechanism Enable() and Disable()
    // Animator: Play(string name)
    */


    [Header("Debugging")]
    [SerializeField] private bool debugMode;

    private void Awake()
    {
        // [YOUR CODE HERE]

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debugging feedback
        if (debugMode) print(name + "was pressed.");

        // [YOUR CODE HERE]

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Debugging feedback
        if (debugMode) print(name + "was released.");

        // [YOUR CODE HERE]

    }

    // [ANY HELPER FUNCTIONS HERE]

}
