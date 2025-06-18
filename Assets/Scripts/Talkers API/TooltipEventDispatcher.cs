using UnityEngine;


public abstract class TooltipEventDispatcher : MonoBehaviour
{
    [Header("Event Dispatcher")]
    [SerializeField] protected TooltipBase tooltip;

    protected void DispatchTooltipEvent(bool value) => tooltip.ShowTooltip = value;
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!tooltip)
            tooltip = GetComponent<TooltipBase>();
    }
#endif
}
