using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIBestiaryRecord : MonoBehaviour, IGUIElement
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    private InventoryItemData _associatedItem;
    
    public InventoryItemData AssociatedData
    {
        get => _associatedItem;

        set
        {
            if (_associatedItem)
                PlayerInventoryController.Instance.BestiaryRef.RemoveItem(_associatedItem);
            
            _associatedItem = value;
            if (!value)
            {
                icon.sprite = null;
                title.text = "";
                description.text = "";
                return;
            }
            
            icon.sprite = value.Icon;
            icon.preserveAspect = true;
            title.text = value.ItemName;
            description.text = value.Description;
        }
    }
}
