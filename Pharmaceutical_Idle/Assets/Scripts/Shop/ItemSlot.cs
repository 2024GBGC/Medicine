using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private int curSlotItemID;
    [SerializeField] public ShopItemGrid itemGrid;
    [SerializeField] private Button buyButton;
    [SerializeField] private int itemTimer = 10;

    private void Start()
    {
        buyButton.onClick.AddListener(StartProcessing);
    }

    private void StartProcessing()
    {
        StartCoroutine(Processing());
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(UpgradeItem);
    }
    
    IEnumerator Processing()
    {
        yield return new WaitForSeconds(itemTimer);
        MainInventory.Instance.AddItem(curSlotItemID, 1);
        if(itemGrid.isItemOnGrid == false)
        {
            AddItemAtItemGrid();
        }
        StartCoroutine(Processing());
    }
    

    private void AddItemAtItemGrid()
    {
        itemGrid.AddItem(curSlotItemID);
    }
    
    private void UpgradeItem()
    {
        itemTimer -= 1;
    }
}
