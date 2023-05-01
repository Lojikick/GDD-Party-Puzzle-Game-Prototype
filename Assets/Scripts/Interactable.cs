using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected InteractionUI interactionUI;

    protected virtual void Awake()
    {
        interactionUI = GetComponentInChildren<InteractionUI>();
    }

    public void Highlight()
    {
        interactionUI.Show();
    }

    public abstract bool Interact();

    public void UnHighlight()
    {
        interactionUI.Hide();
    }
}
