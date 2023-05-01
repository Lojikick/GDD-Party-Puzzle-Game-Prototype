using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IngredientType { Flour, Milk, Eggs, Sugar, Pastry };

[CreateAssetMenu]
public class Ingredient : ScriptableObject
{
    public IngredientType type;
    public Sprite sprite;
}
