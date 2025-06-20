using UnityEngine;

[CreateAssetMenu(fileName = "OnItemObtained", menuName = "Scriptable Objects/OnItemObtainedValidator")]
public class OnItemObtained : IValidator
{
    [SerializeField] private InventoryItemData item;
    
    public override bool Validate()
    {
        return PlayerInventoryController.Instance.InventoryRef.ContainsItem(item);
    }
}
