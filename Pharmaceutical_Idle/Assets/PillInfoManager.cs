using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PillInfoManager : MonoBehaviour
{
    [SerializeField] private GameObject unlockObject;
    [SerializeField] private Image pillCapsuleImage;
    [SerializeField] private TextMeshProUGUI redColorText;
    [SerializeField] private TextMeshProUGUI greenColorText;
    [SerializeField] private TextMeshProUGUI blueColorText;
    
    [SerializeField] private TextMeshProUGUI priceText;


    private void Start()
    {
        unlockObject.SetActive(true);
    }

    public void UpdatePillInfo(Color pillColor, int price, int time)
    {
        unlockObject.SetActive(false);
        redColorText.text = $"R: {pillColor.r:F3}";
        greenColorText.text = $"R: {pillColor.g:F3}";
        blueColorText.text = $"R: {pillColor.b:F3}";

        pillCapsuleImage.color = pillColor;
        
        priceText.text = $"수익: {price} / {time}";
    }
}
