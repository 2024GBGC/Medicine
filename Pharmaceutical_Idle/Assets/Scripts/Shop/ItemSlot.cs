using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private int curItemLevel;
    [SerializeField] private int upgradeCost;
    [SerializeField] private int curSlotItemID;
    [SerializeField] public ShopItemGrid itemGrid;
    [SerializeField] private Button buyButton;
    [SerializeField] private Slider progressSlider; // 슬라이더 추가
    [SerializeField] private int itemTimer = 10;
    [SerializeField] private TextMeshProUGUI itemNameText; // TextMeshProUGUI 필드 추가
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private void Start()
    {
        buyButton.onClick.AddListener(StartProcessing);
        progressSlider.maxValue = itemTimer; // 슬라이더 최대값을 타이머로 설정
        progressSlider.value = itemTimer; // 슬라이더 초기값 설정
        itemNameText.text = ItemDB.Instance.GetItemByID(curSlotItemID).itemName; // 아이템 이름 설정
        itemCost.text = ItemDB.Instance.GetItemByID(curSlotItemID).itemPrice.ToString(); // 아이템 가격 설정
    }

    private void Update()
    {
        UpdateItemCount();
    }

    private void StartProcessing()
    {
        StartCoroutine(Processing());
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(UpgradeItem);
    }
    
    IEnumerator Processing()
    {
        float timeRemaining = itemTimer;
        
        while (timeRemaining > 0)
        {
            progressSlider.value = timeRemaining; // 슬라이더 업데이트
            yield return new WaitForSeconds(1);
            timeRemaining -= 1;
        }
        
        MainInventory.Instance.AddItem(curSlotItemID, 1);
        if (itemGrid.isItemOnGrid == false)
        {
            AddItemAtItemGrid();
        }
        
        // 코루틴을 다시 시작하기 전에 슬라이더를 초기화
        progressSlider.value = itemTimer;
        StartCoroutine(Processing());
    }

    private void AddItemAtItemGrid()
    {
        itemGrid.AddItem(curSlotItemID);
    }
    
    private void UpgradeItem()
    {
        curItemLevel += 1;
        itemTimer -= 1;
        upgradeCost = curItemLevel * curItemLevel * ItemDB.Instance.GetItemByID(curSlotItemID).itemCost;
        itemCost.text = upgradeCost.ToString();
        progressSlider.maxValue = itemTimer; // 슬라이더 최대값 재설정
    }
    
   private void UpdateItemCount()
    {   
        int curTextItemCount = int.Parse(itemCountText.text);
        if(curTextItemCount != MainInventory.Instance._itemDict[curSlotItemID])
            itemCountText.text = MainInventory.Instance._itemDict[curSlotItemID].ToString();
    }
}