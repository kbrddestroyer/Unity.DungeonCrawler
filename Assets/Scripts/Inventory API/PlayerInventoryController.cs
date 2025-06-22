using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryController : MonoBehaviour
{
    public static PlayerInventoryController Instance { get; private set; }

    [SerializeField, Range(0f, 10f)] private float pickupDistance;
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Inventory playerBestiary;
    [SerializeField] private Inventory playerMetaProgress;
    
    public Inventory InventoryRef => playerInventory;
    public Inventory BestiaryRef => playerBestiary;
    public Inventory MetaProgressRef => playerMetaProgress;
    
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

    public void AddBestiaryRecord(InventoryItemData itemData)
    {
        if (!itemData)
            return;
        
        BestiaryRef.AddItem(itemData);
    }

    public void AddMetaProgress(InventoryItemData itemData)
    {
        if (!itemData)
            return;
        
        MetaProgressRef.AddItem(itemData);
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (layerMask == 0)
            layerMask = LayerMask.GetMask("Pickables");

        if (!playerInventory)
            playerInventory = GameObject.FindGameObjectWithTag("inventory")?.GetComponent<Inventory>();
        
        if (!playerBestiary)
            playerBestiary = GameObject.FindGameObjectWithTag("bestiary")?.GetComponent<Inventory>();
        
        if (!playerMetaProgress)
            playerMetaProgress = GameObject.FindGameObjectWithTag("metaprogress")?.GetComponent<Inventory>();
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aquamarine;
        Gizmos.DrawWireSphere(transform.position, pickupDistance);
    }
#endif
}
