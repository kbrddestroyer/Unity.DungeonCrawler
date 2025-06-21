using System;
using System.Linq;

[Serializable]
public class FloatDynamicProperty : DynamicProperty<float>
{
    public FloatDynamicProperty() : base(1) { }
    
    public FloatDynamicProperty(float wrapped) : base(wrapped)
    {
    }

    public override float Apply(float val) => val * Value;

    public override float Value
    {
        set => wrapped = value / _properties.Aggregate(1.0f, (current, property) => property.Apply(current));
    }
}
