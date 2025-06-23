using System;
using System.Linq;

[Serializable]
public class FloatDynamicProperty : DynamicProperty<float>
{
    public FloatDynamicProperty() : base(1) { }
    
    public FloatDynamicProperty(float wrapped, OnValueChanged callback = null) : base(wrapped, callback)
    {
    }

    public override float Apply(float val)
    {
        return val * Value;
    }

    public override float Value
    {
        set
        {
            wrapped = value / Properties.Aggregate(1.0f, (current, property) => property.Apply(current));
            OnValueChangedCallback?.Invoke(wrapped);
        }
    }
}
