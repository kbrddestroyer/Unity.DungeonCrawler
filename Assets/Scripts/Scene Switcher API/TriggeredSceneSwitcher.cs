using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggeredSceneSwitcher : SceneSwitcher
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
            TrySwitchScene();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        var selfCollider = GetComponent<Collider2D>();
        selfCollider.isTrigger = true;
    }
#endif
}
