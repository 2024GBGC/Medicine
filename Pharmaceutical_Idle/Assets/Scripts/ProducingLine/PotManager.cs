using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotManager : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject PotObject;
    [SerializeField] private ItemGrid itemGrid;
    [SerializeField] private int potionPrice;
    [SerializeField] private Button activeButton;
    [SerializeField] private Slider progressSlider; // 슬라이더 추가
    [SerializeField] private int potionCreationTime = 10;
    private Dictionary<int, int> curItemDict;

    public void SetActivePot()
    {
        isActive = true;
        activeButton.onClick.AddListener(SetItem);
    }

    public void TogglePot(bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
    }

    private void SetItem()
    {
        curItemDict = itemGrid.GetItemDict();
        GenerateItem();
        AutoGenerate();
    }
    
    private void GenerateItem()
    {
        potionPrice = 0;
        foreach (var ingredient in curItemDict)
        {
            potionPrice += ItemDB.Instance.GetItemByID(ingredient.Key).itemPrice * ingredient.Value;
        }
    }
    
    private void AutoGenerate()
    {
        StartCoroutine(MakePotion());
        Debug.Log("MakePotion Coroutine Started!");
    }

    IEnumerator MakePotion()
    {
        // 재료가 충분한지 확인
        foreach (var ingredient in curItemDict)
        {
            if (!MainInventory.Instance.UseItem(ingredient.Key, ingredient.Value))
            {
                Debug.Log("Not Enough Ingredients");
                StopCoroutine(MakePotion());
                yield break; // 코루틴 종료
            }
        }
        
        // 슬라이더 초기화
        progressSlider.maxValue = potionCreationTime;
        progressSlider.value = potionCreationTime;

        // 슬라이더 업데이트
        float timeRemaining = potionCreationTime;
        while (timeRemaining > 0)
        {
            progressSlider.value = timeRemaining;
            yield return new WaitForSeconds(1);
            timeRemaining -= 1;
        }

        // 포션 생성 완료
        Debug.Log("Create Complete");
        MainInventory.Instance.IncreaseCredit(potionPrice);
        StartCoroutine(MakePotion()); // 코루틴 재시작
    }

    public void UpgradePot(bool isFireUpgrade)
    {
        if (isFireUpgrade)
        {
            potionCreationTime -= 1;
            Debug.Log(this.name + " Upgrade Fire Complete! : " + potionCreationTime);
        }
        else
        {
            itemGrid.UpgradePot();
            Debug.Log(this.name + " Upgrade Size Complete!");
        }
    }
}
