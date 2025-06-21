using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GUIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image icon;

    public InventoryItemData AssociatedData { get; set; }

    public void SetImage(Sprite sprite) => icon.sprite = sprite;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!icon)
            icon = GetComponent<Image>();
    }
#endif
}
