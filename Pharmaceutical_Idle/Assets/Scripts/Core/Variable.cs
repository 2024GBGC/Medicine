using System;
using System.Collections.Generic;
using UnityEngine;

public class Variable<T> 
{
    private T _value; 
    
    public event Action<T> OnValueChanged;

    public Variable(T initialValue)
    {
        _value = initialValue;
    }

    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }
}