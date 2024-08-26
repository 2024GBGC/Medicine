using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDB : Singleton<ItemDB>
{
    [Header("All")]
    [SerializeField] private List<ItemData> allItems = new List<ItemData>();

    // inventoryItemPrefab
    public GameObject itemPrefab;
    private void Awake()
    {
        LoadAllItem();
    }

    private void LoadAllItem()
    {
        // Resources/Items 경로에서 모든 ItemData를 로드
        ItemData[] items = Resources.LoadAll<ItemData>("Items");

        foreach (var item in items)
        {
            allItems.Add(item);
        }
    }
    
    public ItemData GetItemByID(int ID)
    {
        return allItems.FirstOrDefault(item => item.itemID == ID);
    }
    
    public List<int> GetItemIDList()
    {
        List<int> itemIDList = new List<int>();
        foreach (var item in allItems)
        {
            itemIDList.Add(item.itemID);
        }

        return itemIDList;
    }
}
