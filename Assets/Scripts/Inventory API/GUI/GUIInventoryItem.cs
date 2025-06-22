using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GUIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image icon;

    private InventoryItemData _associatedItem;

    public InventoryItemData AssociatedData
    {
        get => _associatedItem;
        set
        {
            if (_associatedItem)
                PlayerInventoryController.Instance.InventoryRef.RemoveItem(_associatedItem);
            
            _associatedItem = value;

            if (value)
            {
                SetImage(value.Icon);
            }
        }
    }

    private void SetImage(Sprite sprite) => icon.sprite = sprite;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!icon)
            icon = GetComponent<Image>();
    }
#endif
}
