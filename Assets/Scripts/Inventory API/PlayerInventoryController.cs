using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryController : MonoBehaviour
{
    public static PlayerInventoryController Instance { get; private set; }

    [SerializeField, Range(0f, 10f)] private float pickupDistance;
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Inventory playerInventory;

    public Inventory InventoryRef => playerInventory;
    
    private void OnEnable() => Instance = this;
    private void OnDisable() => Instance = null;
    
    private void Start()
    {
        if (!playerInventory)
            Debug.LogError("Inventory instance is null!");

        playerInput["Pick"].performed += ProcessPickup;
    }

    private void ProcessPickup(InputAction.CallbackContext context)
    {
        if (!context.ReadValueAsButton()) return;
        
        // Find inventory items near self
        var result = Physics2D.OverlapCircle(transform.position, pickupDistance, layerMask);

        if (!result)
            return;
        
        // Process pickup
        var pickable = result.gameObject.GetComponent<PickableBase>();
        pickable?.Pickup();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (layerMask == 0)
            layerMask = LayerMask.GetMask("Pickables");
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aquamarine;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
#endif
}
