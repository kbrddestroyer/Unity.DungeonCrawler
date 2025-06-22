using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    [SerializeField] private InventoryItemData first;
    [SerializeField] private InventoryItemData second;
    [SerializeField] private InventoryItemData result;

    public InventoryItemData ValidateRecipe(InventoryItemData left, InventoryItemData right)
    {
        if (!left || !right) return null;

        return
            (left == first && right == second) ||
            (left == second && right == first) ? result : null;
    }
}
