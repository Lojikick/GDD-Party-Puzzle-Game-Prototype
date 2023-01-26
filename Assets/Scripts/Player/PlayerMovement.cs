using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    /*
    [YOUR REFERENCES/VARIABLES HERE]
    */

    // Update is called once per frame, will be used to track inputs
    void Update()
    {
        // Input: Vector 
        //movement.x = Input.GetAxisRaw("Insert Direction Here");
        //movement.y = Input.GetAxisRaw("Insert Direction Here");
    }

    //Fixed Update, will be used to handle movemement
    void FixedUpdate()
    {
        //rb.MovePosition(____+ ____ * ____ * _____);

    }
}
