using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            ShowTooltip = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            ShowTooltip = false;
        }
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var trigger = GetComponent<Collider2D>();
        if (trigger && !trigger.isTrigger)
            trigger.isTrigger = true;
    }
#endif
}
