using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    /*
    [YOUR REFERENCES/VARIABLES HERE]
    */
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Vector2 movement;
    public Vector2Int facingDirection;
    public bool stunned;

    private void Start()
    {
        facingDirection = Vector2Int.zero;
    }

    public void CheckForMovement()
    {
        // Get direction based on input
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Set facing direction
        if (direction.x > 0)
        {
            facingDirection = Vector2Int.right; // Right
        }

        if (direction.x < 0)
        {
            facingDirection = Vector2Int.left; // Left
        }

        if (direction.y > 0)
        {
            facingDirection = Vector2Int.up; // Up
        }

        if (direction.y < 0)
        {
            facingDirection = Vector2Int.down; // Down
        }

        // Not moving -> moving
        if (movement == Vector2.zero && direction != Vector2.zero)
        {
            // Play footsteps
            AudioManager.instance.PlaySound("Footsteps SFX");
        }

        // Moving -> Not moving
        if (movement != Vector2.zero && direction == Vector2.zero)
        {
            // Stop footsteps
            AudioManager.instance.StopSound("Footsteps SFX");
        }

        // Normalize
        direction.Normalize();

        // Set movement
        movement = direction;
    }

    public void StopMovement()
    {
        movement = Vector2.zero;
    }

    //Fixed Update, will be used to handle movemement
    void FixedUpdate()
    {
        if (!stunned)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
