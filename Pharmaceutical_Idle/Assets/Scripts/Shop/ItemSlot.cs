using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    private int curItemLevel = 0;
    [SerializeField] private int upgradeCost;
    [SerializeField] private int curSlotItemID;
    [SerializeField] public ShopItemGrid itemGrid;
    [SerializeField] private Button buyButton;
    [SerializeField] private Image progressImage; // Image로 변경
    [SerializeField] private int itemTimer = 10;
    [SerializeField] private TextMeshProUGUI itemNameText; // TextMeshProUGUI 필드 추가
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private TextMeshProUGUI itemCountText;

    private void Start()
    {
        buyButton.onClick.AddListener(StartProcessing);
        progressImage.fillAmount = 0; // 초기값을 0으로 설정
        itemNameText.text = ItemDB.Instance.GetItemByID(curSlotItemID).itemName; // 아이템 이름 설정
        itemCost.text = ItemDB.Instance.GetItemByID(curSlotItemID).itemPrice.ToString(); // 아이템 가격 설정
    }

    private void Update()
    {
        UpdateItemCount();
    }

    private void StartProcessing()
    {
        if(MainInventory.Instance.credit < ItemDB.Instance.GetItemByID(curSlotItemID).itemPrice) return;
        curItemLevel += 1;
        MainInventory.Instance.DecreaseCredit(ItemDB.Instance.GetItemByID(curSlotItemID).itemPrice);
        Debug.Log("StartProcessing");
        buyButton.onClick.RemoveAllListeners();
        StartCoroutine(Processing());
        buyButton.onClick.AddListener(UpgradeItem);
    }
    
    IEnumerator Processing()
    {
        float curCreateTime = itemTimer;
        float timeRemaining = itemTimer;
        float elapsedTime = 0f;

        while (timeRemaining > 0)
        {
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            progressImage.fillAmount = Mathf.Clamp01(elapsedTime / curCreateTime); // fillAmount 업데이트
            yield return null; // 다음 프레임까지 대기
            timeRemaining -= Time.deltaTime; // 실제 경과 시간 감소
        }
        
        // 포션이 완료되면 아이템 추가
        MainInventory.Instance.AddItem(curSlotItemID, 1);
        if (!itemGrid.isItemOnGrid)
        {
            AddItemAtItemGrid();
        }
        
        // 코루틴을 다시 시작하기 전에 fillAmount를 0으로 초기화
        progressImage.fillAmount = 0;
        StartCoroutine(Processing()); // 코루틴 재시작
    }

    private void AddItemAtItemGrid()
    {
        itemGrid.AddItem(curSlotItemID);
    }
    
    private void UpgradeItem()
    {
        if(MainInventory.Instance.credit < upgradeCost) return;
        SettingsManager.Instance.upgradeSfxSource.Play();
        curItemLevel += 1;
        itemTimer -= 1;
        if (itemTimer <= 1)
        {
            upgradeCost = 0;
            itemCost.text = "Max Level";
            buyButton.interactable = false;
            return;
        }
        MainInventory.Instance.DecreaseCredit(upgradeCost);
        upgradeCost = curItemLevel * curItemLevel * ItemDB.Instance.GetItemByID(curSlotItemID).itemCost;
        itemCost.text = upgradeCost.ToString();
    }
    
    private void UpdateItemCount()
    {
        int curTextItemCount = int.Parse(itemCountText.text);
        if (curTextItemCount != MainInventory.Instance._itemDict[curSlotItemID])
            itemCountText.text = MainInventory.Instance._itemDict[curSlotItemID].ToString();
    }
}
