using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GUIInventory : MonoBehaviour
{
    public static GUIInventory Instance { get; private set; }

    [SerializeField] private GameObject inventoryRoot;
    [SerializeField] private Transform root;
    [SerializeField] private GameObject itemPrefab;
    [Header("Controls")] 
    [SerializeField] private InputActionAsset playerInput;

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

    private void ProcessToggleInventory(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            inventoryRoot.SetActive(!inventoryRoot.activeInHierarchy);
        }
    }
    
    private void Start()
    {
        playerInput["Inventory Toggle"].performed += ProcessToggleInventory;
    }
}
