using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookbookHandler : Interactable
{
    public override bool Interact()
    {
        return CookbookMenuUI.instance.Toggle();
    }
}
