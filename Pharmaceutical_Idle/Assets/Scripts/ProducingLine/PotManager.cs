using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PotManager : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] private Image activeImage;
    
    public int activePrice = 1000;
    private RectTransform rectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject PotObject;
    [SerializeField] private ItemGrid itemGrid;
    [SerializeField] private int potionPrice;
    [SerializeField] private Button activeButton;
    [SerializeField] private Image progressImage; // Fill Amount을 표시할 이미지
    [SerializeField] private Image emptyFlaskImage;
    [SerializeField] private int potionCreationTime = 10;
    private Dictionary<int, int> curItemDict;

    private Vector2 deltaFlaskSize;
    
    [Header("Table")]
    [SerializeField] private Image TableFlaskImage_Fill; // Fill Amount을 표시할 이미지
    [SerializeField] private Image TableFlaskImage_Empty;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        deltaFlaskSize = new Vector2(emptyFlaskImage.rectTransform.sizeDelta.x/3,
            emptyFlaskImage.rectTransform.sizeDelta.y/3);
    }

    public void SetActivePot()
    {
        isActive = true;
        activeImage.gameObject.SetActive(false);
        TableFlaskImage_Empty.gameObject.SetActive(true);
        TableFlaskImage_Fill.fillAmount = 0;
        activeButton.onClick.AddListener(SetItem);
    }

    public void TogglePot(bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.blocksRaycasts = isActive;
        canvasGroup.interactable = isActive;
        rectTransform.SetAsLastSibling();
    }

    private void SetItem()
    {
        curItemDict = itemGrid.GetItemDict();
        if(curItemDict.Keys.Count == 0)
        {
            Debug.Log("No Item in Pot");
            return;
        }
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
                yield break; // 코루틴 종료
            }
        }

        // 이미지 초기화
        progressImage.fillAmount = 0; // 처음에 0으로 설정
        TableFlaskImage_Fill.fillAmount = 0;
        // 포션 생성 시작
        yield return StartCoroutine(UpdateProgress());
        
        // 포션 생성 완료
        Debug.Log("Create Complete");
        MainInventory.Instance.IncreaseCredit(potionPrice);
        StartCoroutine(MakePotion()); // 코루틴 재시작
    }

    IEnumerator UpdateProgress()
    {
        float elapsedTime = 0; // 경과 시간
        while (elapsedTime < potionCreationTime)
        {
            elapsedTime += Time.deltaTime; // 프레임에 따라 경과 시간 증가
            progressImage.fillAmount = elapsedTime / potionCreationTime; // fillAmount 업데이트
            TableFlaskImage_Fill.fillAmount = elapsedTime / potionCreationTime;
            yield return null; // 다음 프레임까지 대기
        }
        progressImage.fillAmount = 1; // 마지막에 fillAmount를 1로 설정하여 완료 표시
        TableFlaskImage_Fill.fillAmount = 1;
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
            emptyFlaskImage.rectTransform.sizeDelta += deltaFlaskSize;
            progressImage.rectTransform.sizeDelta += deltaFlaskSize;
        }
    }
}
