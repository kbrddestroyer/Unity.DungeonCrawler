using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GUIInventory : MonoBehaviour
{
    [SerializeField] private GameObject inventoryRoot;
    [SerializeField] private string bindingName;
    [SerializeField] private Transform root;
    [SerializeField] private GameObject itemPrefab;
    [Header("Controls")] 
    [SerializeField] private InputActionAsset playerInput;

    private readonly Dictionary<uint, Stack<GameObject>> _gameObjectById = new();

    public void AddItem(InventoryItemData item)
    {
        if (!item.Icon)
            return;
        
        var guiObject = Instantiate(itemPrefab, root);
        var guiObjectScript = guiObject.GetComponent<IGUIElement>();

        if (guiObjectScript == null)
            return;
        
        guiObjectScript.AssociatedData = item;
        
        if (!_gameObjectById.ContainsKey(item.UniqueId))
            _gameObjectById.Add(item.UniqueId, new Stack<GameObject>());
        _gameObjectById[item.UniqueId].Push(guiObject);
    }
    
    public void RemoveItem(uint id) => Destroy(_gameObjectById[id].Pop().gameObject);

    private void ProcessToggleInventory(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            inventoryRoot.SetActive(!inventoryRoot.activeInHierarchy);
        }
    }
    
    private void Start()
    {
        playerInput[bindingName].performed += ProcessToggleInventory;
    }
}
