using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PickableBase : MonoBehaviour
{
    [SerializeField] private InventoryItemData info;
    [SerializeField] private UnityEvent onPick;

    public void Pickup()
    {
        PlayerInventoryController.Instance.InventoryRef.AddItem(info);
        onPick.Invoke();
        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!info)
            Debug.LogError($"Item {gameObject.name} has no info assigned. Inventory behaviour is undefined! Please fix immediately!");
    }
#endif
}
