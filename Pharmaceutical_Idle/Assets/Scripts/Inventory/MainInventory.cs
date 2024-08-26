using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInventory : Singleton<MainInventory>
{
    public SerializableDictionary<int, int> _itemDict = new SerializableDictionary<int, int>();

    public int credit;

    private void Start()
    {
        InitDict();
    }
    
    private void InitDict()
    {
        foreach (var itemID in ItemDB.Instance.GetItemIDList())
        {
            _itemDict.Add(itemID, 0);
        }
    }

    public void AddItem(int itemID, int amount)
    {
        if (_itemDict.ContainsKey(itemID))
        {
            _itemDict[itemID] += amount;
        }
    }
    
    public void RemoveItem(int itemID, int amount)
    {
        if (_itemDict.ContainsKey(itemID))
        {
            _itemDict[itemID] -= amount;
        }
    }
}
