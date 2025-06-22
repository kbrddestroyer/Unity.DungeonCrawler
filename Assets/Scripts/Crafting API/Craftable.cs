using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Craftable : GUIInventoryItem, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private static List<Craftable> _allCraftables = new();
    
    [SerializeField] private Camera mainCamera;
    [SerializeField, Range(0f, 100f)] private float distanceToMergeItems;
    [SerializeField] private RecipeRegistry registry;

    private bool _beingDragged;
    private Vector3 _dragStartPosition;
    
    private void Start() => _allCraftables.Add(this);
    private void OnDestroy() => _allCraftables.Remove(this);
    
    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3) eventData.delta;
    }

    private bool HandleMerge(Craftable other)
    {
        if (other == this)
            return false;
        
        var result = registry.ValidateRecipe(AssociatedData, other.AssociatedData);
        
        if (!result) return false;
        
        PlayerInventoryController.Instance.InventoryRef.RemoveItem(other.AssociatedData);
        PlayerInventoryController.Instance.InventoryRef.AddItem(result);
        
        return true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Try merge with other craftable object
        if (!_beingDragged) return;
        _beingDragged = false;
        
        foreach (var craftable in _allCraftables.Where(craftable => Vector2.Distance(craftable.transform.position, transform.position) <= distanceToMergeItems))
        {
            if (HandleMerge(craftable))
            {
                PlayerInventoryController.Instance.InventoryRef.RemoveItem(AssociatedData);    
            }
        }
        
        transform.position = _dragStartPosition;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        _beingDragged = true;
        _dragStartPosition = transform.position;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!mainCamera)
            mainCamera = Camera.main;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawWireSphere(transform.position, distanceToMergeItems);
    }
#endif
}
