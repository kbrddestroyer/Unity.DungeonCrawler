using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

[Description("Scriptable, that's capable of storing inventory items")]
[CreateAssetMenu(fileName = "ItemRegistry", menuName = "Scriptable Objects/ItemRegistry")]
public class ItemRegistry : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> items = new();

    public InventoryItemData GetItem(uint itemId) => items[(int) itemId];
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        foreach (var item in items.Where(item => item.UniqueId == 0))
        {
            item.UniqueId = (uint) items.IndexOf(item);
        }
    }
#endif
}
