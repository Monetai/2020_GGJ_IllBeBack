using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public abstract class RuntimeSet<T> : ScriptableObject
{
    [SerializeField]
    [HideInPlayMode]
    private List<T> defaultItems = new List<T>();

    [ShowInInspector]
    [HideInEditorMode]
    private List<T> currentItems = new List<T>();

    public List<T> CurrentItems
    {
        get { return currentItems; }
    }

    private void OnEnable()
    {
        currentItems = new List<T>(defaultItems);
    }

    public void Add(T thing)
    {
        if (!currentItems.Contains(thing))
            currentItems.Add(thing);
    }

    public void Remove(T thing)
    {
        if (currentItems.Contains(thing))
            currentItems.Remove(thing);
    }
}
