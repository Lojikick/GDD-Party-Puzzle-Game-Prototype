using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;

    [Header("Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Vector3 interactionOffset;

    [Header("Debugging")]
    [SerializeField] private Ingredient heldIngredient;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Do nothing if no puzzle
        if (PuzzleManager.instance == null) return;

        ShowComboIndicator();

        // Check for input
        if (Input.GetKeyDown(interactKey))
        {
            // Check if you are standing on top of an ingredient
            var groundIngredientHandler = IsOnIngredient();
            if (groundIngredientHandler != null)
            {
                if (heldIngredient == null)
                {
                    PickUp(groundIngredientHandler);
                }
                else // If already holding something
                {
                    Combine(heldIngredient, groundIngredientHandler);
                }
            }
            else
            {
                if (heldIngredient != null)
                {
                    Drop();
                }
            }

        }
    }

    private void ShowComboIndicator()
    {
        // Look for any ingredients in range
        var hit = Physics2D.OverlapCircle(transform.position + interactionOffset, interactionRadius, interactionLayer);
        if (hit && hit.TryGetComponent(out IngredientHandler ingredientHandler) && this.heldIngredient != null)
        {
            int sum = (int)ingredientHandler.ingredient.type + (int)heldIngredient.type;
            if (sum == 1) // If combo is Bread 
            {
                PuzzleManager.instance.SelectBreadPosition(ingredientHandler.transform.position + (Vector3)(Vector2)movement.facingDirection);
                return;
            }
            else if (sum == 4) // Pudding
            {
                PuzzleManager.instance.SelectPuddingPosition(ingredientHandler.transform.position + (Vector3)(Vector2)movement.facingDirection);
                return;
            }
        }

        PuzzleManager.instance.SelectPuddingPosition(Vector3.back);
    }

    private IngredientHandler IsOnIngredient()
    {
        // Look for any ingredients in range
        var hit = Physics2D.OverlapCircle(transform.position + interactionOffset, interactionRadius, interactionLayer);
        if (hit && hit.TryGetComponent(out IngredientHandler ingredientHandler))
        {
            return ingredientHandler;
        }

        return null;
    }

    private void PickUp(IngredientHandler ingredientHandler)
    {
        this.heldIngredient = ingredientHandler.ingredient;

        // Update UI
        InventoryUI.instance.SetSprite(this.heldIngredient.sprite);

        // Destroy ingredient
        Destroy(ingredientHandler.gameObject);

        // Play sound
        AudioManager.instance.PlaySound("Pickup SFX");

        // Check if ingredient is final pastry
        if (heldIngredient.type == IngredientType.Pastry)
        {
            // Complete level
            GameManager.instance.CompletePuzzle();
        }

        // Debugging
        print("Picking up " + heldIngredient.name);
    }

    private void Drop()
    {
        // Debugging
        print("Dropping " + heldIngredient.name);

        // Spawn ingredient ontop of the tile you are standing on
        PuzzleManager.instance.SpawnIngredient(this.heldIngredient, transform.position);

        // Play sound
        AudioManager.instance.PlaySound("Drop SFX");

        // Update UI
        InventoryUI.instance.SetSprite(null);

        this.heldIngredient = null;
    }

    private void Combine(Ingredient heldIngredient, IngredientHandler groundIngredient)
    {
        // Debugging
        print("Combing " + heldIngredient.name + " and " + groundIngredient.ingredient.name);

        // Mix
        bool sucess = PuzzleManager.instance.MixIngredients(heldIngredient, groundIngredient.ingredient, groundIngredient.transform.position, movement.facingDirection);
        if (sucess)
        {
            // Play sound
            AudioManager.instance.PlaySound("Combine SFX");

            // Update UI
            InventoryUI.instance.SetSprite(null);

            // Get rid of both ingredients
            Destroy(groundIngredient.gameObject);
            this.heldIngredient = null;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + interactionOffset, interactionRadius);
    }
}