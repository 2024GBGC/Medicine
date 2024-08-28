using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<Button> upgradeFireButtons;
    [SerializeField] private List<Button> upgradeSizeButtons;

    private void Start()
    {
        foreach (var button in upgradeSizeButtons)
        {
            button.onClick.AddListener(() =>
            {
                PotSlotManager.Instance.UpgradePot(upgradeSizeButtons.IndexOf(button), false);
            });
        }
        
        foreach (var button in upgradeFireButtons)
        {
            button.onClick.AddListener(() =>
            {
                PotSlotManager.Instance.UpgradePot(upgradeFireButtons.IndexOf(button), true);
            });
        }
    }
}
