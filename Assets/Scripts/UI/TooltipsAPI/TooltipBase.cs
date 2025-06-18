using System;
using UnityEngine;


public abstract class TooltipBase : MonoBehaviour, ITooltip
{
    private bool _shouldShow;
    
    public bool ShowTooltip
    {
        get => _shouldShow;
        set
        {
            _shouldShow = value;
            Show(_shouldShow);
            
            Debug.Log($"Tooltip {gameObject.name} set {_shouldShow}");
        }
    }

    protected abstract void Show(bool value);
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var trigger = GetComponent<Collider2D>();
        if (trigger && !trigger.isTrigger)
            trigger.isTrigger = true;
    }
#endif
}
