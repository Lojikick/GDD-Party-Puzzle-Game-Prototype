using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image ingredientImage;

    public static InventoryUI instance;
    private void Awake()
    {
        // Singleton Logic
        if (InventoryUI.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        SetSprite(null);
    }

    public void SetSprite(Sprite sprite)
    {
        // Hide sprite if no input
        if (sprite == null)
        {
            ingredientImage.enabled = false;
        }
        else
        {
            ingredientImage.sprite = sprite;
            ingredientImage.enabled = true;
        }
    }
}
