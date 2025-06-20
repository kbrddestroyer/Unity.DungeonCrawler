using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[Serializable]
public class ButtonController {
    [Header("UI Setup")]
    [SerializeField] private string id;
    [SerializeField] private UIDocument document;
    [Header("Action")]
    [SerializeField] private UnityEvent action = new();
    [SerializeField] private UnityEvent hover = new();

    public void Initialize()
    {
        var btn = document.rootVisualElement.Q<Button>(id);
        btn.RegisterCallback((ClickEvent _) => { InvokeAction(action); });
        btn.RegisterCallback((MouseOverEvent _) => { InvokeAction(hover); });
    }

    private void InvokeAction(UnityEvent unityEvent)
    {
        unityEvent.Invoke();
    }
}