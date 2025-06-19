using System;
using System.ComponentModel;
using UnityEngine;

[Description("Item structure, that will be stored in inventory")]
[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/InventoryItemData")]
public class InventoryItemData : ScriptableObject, IRegistryItem
{
    [SerializeField] private string itemName;
    [SerializeField, Multiline] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject pickable;
    [SerializeField] private uint uniqueId;
    
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public GameObject Pickable => pickable;
    public uint UniqueId { get => uniqueId; internal set => uniqueId = value; }
}
