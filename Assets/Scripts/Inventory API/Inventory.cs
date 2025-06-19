using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[Description("Root inventory controller, should be added as singleton object to avoid race on load/save")]
public class Inventory : MonoBehaviour
{
    [SerializeField] private ItemRegistry registry;
    [SerializeField] private List<InventoryItemData> items = new();

    public void AddItem(InventoryItemData item) => items.Add(item);
    public void RemoveItem(InventoryItemData item) => items.Remove(item);
    public bool ContainsItem(InventoryItemData item) => items.Contains(item);
    public void Clear() => items.Clear();

    private void LoadState()
    {
        if (Serializer.ReadData<InventoryStorageData>() is not InventoryStorageData data)
            return;
        
        foreach (var uId in data.ListItems)
        {
            items.Add(registry.GetItem(uId));
        }
    }
    
    private void SaveState()
    {
        InventoryStorageData data = new();
        
        foreach (var item in items)
            data.ListItems.Add(item.UniqueId);
        
        data.Save();
    }
    
    private void Start() => LoadState();
    public void OnLevelLoads() => SaveState();

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!registry)
            registry = FindAnyObjectByType<ItemRegistry>();
    }
#endif
}
