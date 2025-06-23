using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class DynamicProperty<T>
{    
    public delegate void OnValueChanged(T value);
    protected OnValueChanged OnValueChangedCallback;

    [FormerlySerializedAs("_wrapped")] 
    [SerializeField]
    protected T wrapped;

    protected DynamicProperty(OnValueChanged callback = null)
    {
        OnValueChangedCallback = callback;
        wrapped = default;
        OnValueChangedCallback?.Invoke(wrapped);
    }

    protected DynamicProperty(T wrapped, OnValueChanged callback = null)
    {
        this.wrapped = wrapped;
        OnValueChangedCallback = callback;
        OnValueChangedCallback?.Invoke(wrapped);
    }
    
    protected readonly List<DynamicProperty<T>> Properties = new();

    public void Add(DynamicProperty<T> property)
    {
        Properties.Add(property);
        OnValueChangedCallback?.Invoke(Value);
    }

    public void Remove(DynamicProperty<T> property)
    {
        Properties.Remove(property);
        OnValueChangedCallback?.Invoke(Value);
    }

    public virtual T Value
    {
        get => Properties.Aggregate(wrapped, (current, property) => property.Apply(current));
        set => wrapped = value;
    }
    public abstract T Apply(T val);
}
