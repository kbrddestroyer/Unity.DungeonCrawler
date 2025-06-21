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

    private readonly Dictionary<uint, Stack<GameObject>> _gameObjectById = new();
    
    public void EnableGUI() => inventoryRoot.SetActive(true);
    public void DisableGUI() => inventoryRoot.SetActive(false);

    public void AddItem(InventoryItemData item)
    {
        if (!item.Icon)
            return;
        
        var guiObject = Instantiate(itemPrefab, root);
        var guiObjectScript = guiObject.GetComponent<GUIInventoryItem>();

        if (!guiObjectScript)
            return;
        
        guiObjectScript.SetImage(item.Icon);
        guiObjectScript.AssociatedData = item;
        
        if (!_gameObjectById.ContainsKey(item.UniqueId))
            _gameObjectById.Add(item.UniqueId, new Stack<GameObject>());
        _gameObjectById[item.UniqueId].Push(guiObject);
    }
    
    public void RemoveItem(uint id) => Destroy(_gameObjectById[id].Pop().gameObject);
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
