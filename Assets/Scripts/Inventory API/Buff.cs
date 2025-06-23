using System;
using UnityEngine;

[Serializable]
public class Buff
{
    public enum Type
    {
        Health,
        Damage
    }

    
    [SerializeField] private FloatDynamicProperty buff;
    [SerializeField] private Type itemBuffType;
    
    public FloatDynamicProperty Multiplier => buff;
    public Type ItemBuffType => itemBuffType;
}
