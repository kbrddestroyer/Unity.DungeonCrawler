using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TooltipTriggerEventDispatcher : TooltipEventDispatcher
{
    private void OnTriggerEnter2D(Collider2D other) => DispatchTooltipEvent(true);
    private void OnTriggerExit2D(Collider2D other) =>  DispatchTooltipEvent(false);
    
#if UNITY_EDITOR
    private void OnValidate() => GetComponent<Collider2D>().isTrigger = true;
#endif
}
