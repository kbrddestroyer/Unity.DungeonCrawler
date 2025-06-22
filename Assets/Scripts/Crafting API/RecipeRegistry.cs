using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeRegistry", menuName = "Scriptable Objects/RecipeRegistry")]
public class RecipeRegistry : ScriptableObject
{
    [SerializeField] private List<Recipe> recipes;

    public InventoryItemData ValidateRecipe(InventoryItemData left, InventoryItemData right)
    {
        return recipes.Select(recipe => recipe.ValidateRecipe(left, right)).FirstOrDefault(result => result);
    }
}
