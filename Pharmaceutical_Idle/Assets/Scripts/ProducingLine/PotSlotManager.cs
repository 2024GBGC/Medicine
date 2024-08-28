using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PotSlotManager : Singleton<PotSlotManager>
{
    [SerializeField] private List<PotManager> pots;
    
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
    }

    public void BuyGridSlot(int gridSlotIndex)
    {
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
