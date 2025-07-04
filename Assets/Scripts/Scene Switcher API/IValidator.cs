﻿using System;
using UnityEngine;

[Serializable]
public abstract class IValidator : ScriptableObject
{
    public abstract bool Validate();
    
    public abstract string DisplayGoal { get; }
}
