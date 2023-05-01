using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHandler : Interactable
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Debugging")]
    public Ingredient ingredient;

    [Header("Visual Settings")]
    [SerializeField] private float offset;
    [SerializeField] private float duration;

    public override bool Interact()
    {
        return false;
    }

    public void Initialize(Ingredient ingredient, Vector3 position)
    {
        // Do nothing if null
        if (ingredient == null) return;

        transform.position = position;

        this.ingredient = ingredient;
        spriteRenderer.sprite = ingredient.sprite;

        // Start bobbing sprite
        LeanTween.moveLocalY(spriteRenderer.gameObject, offset, duration).setEaseInOutQuad().setLoopPingPong();
    }
}
