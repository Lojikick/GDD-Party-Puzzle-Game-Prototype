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
    Vector2 movement;

    public void CheckForMovement()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    public void StopMovement()
    {
        movement = Vector2.zero;
    }

    //Fixed Update, will be used to handle movemement
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
