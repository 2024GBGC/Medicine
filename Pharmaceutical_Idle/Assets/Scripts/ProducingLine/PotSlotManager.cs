using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PotSlotManager : Singleton<PotSlotManager>
{
    public List<PotManager> pots;
    
    private RectTransform imageTransform;
    private float slideDuration = 0.5f;
    private Vector3 closePosition;  
    private Vector3 openPosition;  
    private Coroutine currentCoroutine;
    
    private bool isOpen = false;  
    private int curOpenIndex = -1;

    private void Start()
    {
        imageTransform = GetComponent<RectTransform>();
        closePosition = imageTransform.anchoredPosition;

        for (int i = 0; i < 3; i++)
        {
            PotUpgradeManager.Instance.upgradeFireTexts[i].text = pots[i].upgradeFirePrice.ToString();
            PotUpgradeManager.Instance.upgradeSlotTexts[i].text = pots[i].upgradeSlotPrice.ToString();
        }
    }

    public void BuyGridSlot(int gridSlotIndex)
    {
        Debug.Log("BUY SLOT : " + gridSlotIndex);
        if(MainInventory.Instance.credit < pots[gridSlotIndex].activePrice)
        {
            Debug.Log("Not enough credit");
            return;
        }
        MainInventory.Instance.DecreaseCredit(pots[gridSlotIndex].activePrice);
        PotUpgradeManager.Instance.activePotButtons[gridSlotIndex].gameObject.SetActive(false);
        pots[gridSlotIndex].SetActivePot();
    }
    
    public void ActiveGridSlot(int activeIndex)
    {
        if (isOpen && curOpenIndex == activeIndex)
        {
            // 집어넣기 
            isOpen = false;
            StartCoroutine(SlideImage(openPosition, closePosition));
            return;
        }
        
        if (!isOpen)
        {
            StartCoroutine(SlideImage(closePosition, openPosition));
            isOpen = true;
        }
        
        foreach (var pot in pots)
        {
            pot.TogglePot(false);
        }
        pots[activeIndex].TogglePot(true);
        curOpenIndex = activeIndex;
    }

    public void UpgradePot(int index, bool isFireUpgrade)
    {
        pots[index].UpgradePot(isFireUpgrade);

        if (isFireUpgrade)
        {
            if (pots[index].fireLevel >= 10)
            {
                PotUpgradeManager.Instance.upgradeFireTexts[index].text = "MaxLevel";
                PotUpgradeManager.Instance.upgradeFireButtons[index].interactable = false;
                return;
            }
        }
        else
        {
            if (pots[index].slotLevel >= 3)
            {
                PotUpgradeManager.Instance.upgradeSlotTexts[index].text = "MaxLevel";
                PotUpgradeManager.Instance.upgradeSizeButtons[index].interactable = false;
                return;
            }
        }

        if (isFireUpgrade)
            PotUpgradeManager.Instance.upgradeFireTexts[index].text = pots[index].upgradeFirePrice.ToString();
        else
            PotUpgradeManager.Instance.upgradeSlotTexts[index].text = pots[index].upgradeSlotPrice.ToString();
        SettingsManager.Instance.upgradeSfxSource.Play();
    }
    
    IEnumerator SlideImage(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0;

        while (elapsedTime < slideDuration)
        {
            // 일정한 비율로 위치를 변경
            imageTransform.anchoredPosition = Vector3.Lerp(start, end, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 정확한 위치에 맞추기 위해 최종 위치를 설정
        imageTransform.anchoredPosition = end;
    }
}
