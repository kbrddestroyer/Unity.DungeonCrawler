using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Root inventory controller, should be added as singleton object to avoid race on load/save")]
public class Inventory : MonoBehaviour
{
    [SerializeField] private string filename;
    [SerializeField] private ItemRegistry registry;
    [SerializeField] private List<uint> items = new();
    [SerializeField] private bool saveOnDestroy;

    public void AddItem(InventoryItemData item)
    {
        if (!item)
            return;
        
        items.Add(item.UniqueId);
        GUIInventory.Instance.AddItem(item);
    }

    public void RemoveItem(InventoryItemData item)
    {
        if (!item)
            return;
        
        items.Remove(item.UniqueId);
        GUIInventory.Instance.RemoveItem(item.UniqueId);
    }
    
    public bool ContainsItem(InventoryItemData item) => items.Contains(item.UniqueId);
    public bool ContainsItem(uint id) => items.Contains(id);
    public void Clear() => items.Clear();

    private void LoadState()
    {
        if (Serializer.ReadData<InventoryStorageData>(filename) is not InventoryStorageData data)
            return;
        
        foreach (var uId in data.ListItems)
        {
            AddItem(registry.GetItem(uId));
        }
    }
    
    private void SaveState()
    {
        InventoryStorageData data = new();
        
        foreach (var item in items)
            data.ListItems.Add(item);
        
        data.Save(filename);
    }
    
    private void Start() => LoadState();
    public void OnLevelLoads() => SaveState();

    private void OnDestroy()
    {
        if (saveOnDestroy)
            SaveState();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!registry)
            registry = FindAnyObjectByType<ItemRegistry>();

        if (filename.Length == 0)
            filename = gameObject.name;
    }
#endif
}
