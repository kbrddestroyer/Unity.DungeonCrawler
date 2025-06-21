using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public abstract class DynamicProperty<T>
{
    [FormerlySerializedAs("_wrapped")] [SerializeField]
    protected T wrapped;

    protected DynamicProperty()
    {
        wrapped = default;
    }

    protected DynamicProperty(T wrapped) => this.wrapped = wrapped;
    
    protected readonly List<DynamicProperty<T>> _properties = new();
    
    public void Add(DynamicProperty<T> property) => _properties.Add(property);
    public void Remove(DynamicProperty<T> property) => _properties.Remove(property);

    public virtual T Value
    {
        get => _properties.Aggregate(wrapped, (current, property) => property.Apply(current));
        set =>  wrapped = value;
    }
    public abstract T Apply(T val);
}
