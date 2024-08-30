using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainInventory : Singleton<MainInventory>
{
    public SerializableDictionary<int, int> _itemDict = new SerializableDictionary<int, int>();

    public int credit;

    [SerializeField] private TextMeshProUGUI creditText; // TextMeshProUGUI 필드 추가

    private void Start()
    {
        InitDict();
        UpdateCreditUI(); // 초기 크레딧 UI 업데이트
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

    public void IncreaseCredit(int potionPrice)
    {
        SettingsManager.Instance.coinSfxSource.Play();
        credit += potionPrice;
        UpdateCreditUI(); // 크레딧 증가 후 UI 업데이트
    }

    public void DecreaseCredit(int price)
    {
        credit -= price;
        UpdateCreditUI(); // 크레딧 감소 후 UI 업데이트
    }

    public bool UseItem(int id, int count)
    {
        if (_itemDict.ContainsKey(id) && _itemDict[id] >= count)
        {
            _itemDict[id] -= count;
            return true;
        }

        Debug.LogWarning($"키 '{id}'가 딕셔너리에 없습니다.");
        return false;
    }

    private void UpdateCreditUI()
    {
        if (creditText != null)
        {
            creditText.text = credit.ToString(); // 크레딧 값을 TextMeshProUGUI에 표시
        }
        else
        {
            Debug.LogWarning("Credit TextMeshProUGUI is not assigned!");
        }
    }
}