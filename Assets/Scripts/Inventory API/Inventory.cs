using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Description("Root inventory controller, should be added as singleton object to avoid race on load/save")]
public class Inventory : IController
{
    [SerializeField] private string filename;
    [SerializeField] private ItemRegistry registry;
    [SerializeField] private List<uint> items = new();
    [SerializeField] private bool saveOnDestroy;
    [SerializeField] private Player player;
    
    public void AddItem(InventoryItemData item)
    {
        if (!item)
            return;
        
        items.Add(item.UniqueId);
        GUIInventory.Instance.AddItem(item);

        switch (item.ItemBuffType)
        {
            case InventoryItemData.Type.Damage:
            {
                player.DamageMul.Add(item.Buff);
                break;
            }
            case InventoryItemData.Type.Health:
            {
                player.HealthMul.Add(item.Buff);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void RemoveItem(InventoryItemData item)
    {
        if (!item)
            return;
        
        items.Remove(item.UniqueId);
        GUIInventory.Instance.RemoveItem(item.UniqueId);
        
        switch (item.ItemBuffType)
        {
            case InventoryItemData.Type.Damage:
            {
                player.DamageMul.Remove(item.Buff);
                break;
            }
            case InventoryItemData.Type.Health:
            {
                player.HealthMul.Remove(item.Buff);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
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

    public void DropItem(GUIInventoryItem item)
    {
        Instantiate(item.AssociatedData, player.transform.position + player.transform.forward, Quaternion.identity);
        RemoveItem(item.AssociatedData);
    }
    
    private void Start() => LoadState();
    public override void OnLevelLoads() => SaveState();

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

        if (!player)
            player = FindAnyObjectByType<Player>();
    }
#endif
}
