using System.Collections.Generic;
using UnityEngine;

public class GUIInventory : MonoBehaviour
{
    public static GUIInventory Instance { get; private set; }

    [SerializeField] private GameObject inventoryRoot;
    [SerializeField] private Transform root;
    [SerializeField] private GameObject itemPrefab;

    private Dictionary<uint, GameObject> _gameObjectById = new();
    
    public void EnableGUI() => inventoryRoot.SetActive(true);
    public void DisableGUI() => inventoryRoot.SetActive(false);

    public void AddItem(InventoryItemData item)
    {
        var guiObject = Instantiate(itemPrefab, root);
        guiObject.GetComponent<GUIInventoryItem>()?.SetImage(item.Icon);
    }
    
    public void RemoveItem(uint id) => Destroy(_gameObjectById[id].gameObject);
    private void OnEnable() => Instance = this;
    private void OnDisable() => Instance = null;
}
