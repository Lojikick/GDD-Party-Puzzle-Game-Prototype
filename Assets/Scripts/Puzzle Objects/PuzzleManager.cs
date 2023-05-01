using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PuzzleManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private GameObject ingredientPrefab;

    [Header("Settings")]
    [SerializeField] private Tile breadTile;

    public static PuzzleManager instance;
    private void Awake()
    {
        // Singleton Logic
        if (PuzzleManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        // Initialize all ingredients on map
        var ingredientHandlers = FindObjectsOfType<IngredientHandler>();
        foreach (var ingredientHandler in ingredientHandlers)
        {
            // Convert world position to cell position
            var tilemapPosition = groundTilemap.WorldToCell(ingredientHandler.transform.position);
            var spawnPosition = groundTilemap.GetCellCenterWorld(tilemapPosition);

            ingredientHandler.Initialize(ingredientHandler.ingredient, spawnPosition);
        }
    }

    public void SpawnIngredient(Ingredient ingredient, Vector3 worldPosition)
    {
        // Convert world position to cell position
        var tilemapPosition = groundTilemap.WorldToCell(worldPosition);
        var spawnPosition = groundTilemap.GetCellCenterWorld(tilemapPosition);

        // Spawn prefab
        var ingredientHandler = Instantiate(ingredientPrefab).GetComponent<IngredientHandler>();
        ingredientHandler.Initialize(ingredient, spawnPosition);
    }

    public bool MixIngredients(Ingredient ingredient1, Ingredient ingredient2, Vector3 startingPosition, Vector2 facingDirection)
    {
        // Dough (Flour + Milk) = 0 + 1 = 1
        // Pudding (Sugar + Milk) = 3 + 1 = 4
        // Noodle  (Flour + Egg) = 0 + 2 = 2
        // Baking Soda + Vinegar (Flour + Sugar) = 0 + 3 = 3
        int sum = (int)ingredient1.type + (int)ingredient2.type;
        if (sum > 4)
            return false;

        // Vector3Int cellPosition = groundTilemap.WorldToCell(startingPosition);
        Vector3Int facingCellPosition = groundTilemap.WorldToCell(startingPosition + (Vector3)facingDirection);

        switch (sum)
        {
            case 1: // Dough/Bread
                print("Combined into Bread.");
                return PlaceBread(facingCellPosition);
            case 2: // Noodle
                print("Combined into Noodle.");
                // TODO
                break;
            case 3: // Baking Soda
                print("Combined into BS+V.");
                // TODO
                break;
            case 4: // Pudding
                print("Combined into Pudding.");
                // TODO
                break;
            default:
                throw new System.Exception("Unimplemented Combination: " + sum);
        }

        return true;
    }

    private bool PlaceBread(Vector3Int position)
    {
        if (groundTilemap.HasTile(position))
        {
            print("Cannot place bread in a filled position.");
            return false;
        }

        // Spawn bread tile
        groundTilemap.SetTile(position, breadTile);

        return true;
    }
}
