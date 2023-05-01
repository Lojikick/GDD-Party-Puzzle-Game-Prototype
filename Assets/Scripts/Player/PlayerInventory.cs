using UnityEngine;

public enum IngredientType { Empty, Flour, Milk, Eggs, Sugar };

public class PlayerInventory : MonoBehaviour
{
    // private string storedItem;
    private IngredientType heldIngredient;

    // Start is called before the first frame update
    void Start()
    {
        heldIngredient = IngredientType.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Check if you are on an item
            if (IsOnItem())
            {
                if (heldIngredient == IngredientType.Empty)
                {
                    PickUp(IngredientType.Empty); // FIXME
                }
                else if (heldIngredient != IngredientType.Empty)
                {
                    Combine(heldIngredient, IngredientType.Empty); // FIXME
                }
            }
            else
            {
                if (heldIngredient != IngredientType.Empty)
                {
                    Drop();
                }
            }

        }
    }

    private bool IsOnItem()
    {
        // TODO
        return true;
    }

    private void PickUp(IngredientType ingredient)
    {
        // TODO
    }

    private void Drop()
    {
        // TODO
    }

    private void Combine(IngredientType ingredient1, IngredientType ingredient2)
    {
        // TODO
    }

   

}