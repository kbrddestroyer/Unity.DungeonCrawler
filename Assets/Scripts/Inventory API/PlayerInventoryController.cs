using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float pickupDistance;
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private LayerMask layerMask;
    
    private void Start()
    {
        if (!Inventory.Instance)
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
