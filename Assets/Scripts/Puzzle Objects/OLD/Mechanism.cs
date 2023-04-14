using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Mechanism : MonoBehaviour
{
    // This function should be called when the mechanism should activate
    public abstract void Enable();

    // This function should be called when the mechanism should deactivate
    public abstract void Disable();
}
