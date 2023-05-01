using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PuzzleManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap indicatorTilemap;
    [SerializeField] private GameObject ingredientPrefab;
    [SerializeField] private GameObject puddingPrefab;

    [Header("Settings")]

    [SerializeField] private Tile breadTile;
    [SerializeField] private Tile validSelectTile;
    [SerializeField] private Tile invalidSelectTile;

    [Header("Debugging")]
    [SerializeField] private Vector3Int selectedCell;

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
        // Bread (Flour + Milk) = 0 + 1 = 1
        // Pudding (Sugar + Milk) = 3 + 1 = 4
        // Noodle  (Flour + Egg) = 0 + 2 = 2
        // Baking Soda + Vinegar (Flour + Sugar) = 0 + 3 = 3
        int sum = (int)ingredient1.type + (int)ingredient2.type;
        if (sum > 4)
            return false;

        Vector3Int cellPosition = groundTilemap.WorldToCell(startingPosition);
        Vector3Int facingCellPosition = groundTilemap.WorldToCell(startingPosition + (Vector3)facingDirection);

        switch (sum)
        {
            case 1: // Bread
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
                return PlacePudding(facingCellPosition);
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

    private bool PlacePudding(Vector3Int position)
    {
        // Make sure there is ground
        if (!groundTilemap.HasTile(position))
        {
            print("Must place pudding on a valid ground tile.");
            return false;
        }

        // Create GO
        var worldPosition = groundTilemap.GetCellCenterWorld(position);
        Instantiate(puddingPrefab, worldPosition, Quaternion.identity);

        return true;
    }

    public bool IfOutOfBounds(Vector3 position)
    {
        // Make sure there is ground
        var cellPosition = groundTilemap.WorldToCell(position);
        return !groundTilemap.HasTile(cellPosition);
    }

    public void SelectPuddingPosition(Vector3 position)
    {
        // Deselect
        if (this.selectedCell != Vector3Int.back)
        {
            indicatorTilemap.SetTile(this.selectedCell, null);
        }

        if (position == Vector3.back)
        {
            // Dip
            return;
        }

        var cellPosition = groundTilemap.WorldToCell(position);

        // If on ground
        if (groundTilemap.HasTile(cellPosition))
        {
            indicatorTilemap.SetTile(cellPosition, validSelectTile);
        }
        else
        {
            indicatorTilemap.SetTile(cellPosition, invalidSelectTile);
        }

        // Save
        this.selectedCell = cellPosition;
    }

    public void SelectBreadPosition(Vector3 position)
    {
        // Deselect
        if (this.selectedCell != Vector3Int.back)
        {
            indicatorTilemap.SetTile(this.selectedCell, null);
        }

        if (position == Vector3.back)
        {
            // Dip
            return;
        }

        var cellPosition = groundTilemap.WorldToCell(position);

        // If on ground
        if (groundTilemap.HasTile(cellPosition))
        {
            indicatorTilemap.SetTile(cellPosition, invalidSelectTile);
        }
        else
        {
            indicatorTilemap.SetTile(cellPosition, validSelectTile);
        }

        // Save
        this.selectedCell = cellPosition;
    }
}
