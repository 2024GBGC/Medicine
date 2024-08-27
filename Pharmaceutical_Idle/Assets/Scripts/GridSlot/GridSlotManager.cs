using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridSlotManager : Singleton<GridSlotManager>
{
    [SerializeField] private List<PotManager> pots;

    private void Start()
    {
        pots[0].SetActivePot();
        ActiveGridSlot(0);
    }

    public void BuyGridSlot(int gridSlotIndex)
    {
        pots[gridSlotIndex].SetActivePot();
    }
    
    public void ActiveGridSlot(int activeIndex)
    {
        foreach (var pot in pots)
        {
            pot.TogglePot(false);
        }
        pots[activeIndex].TogglePot(true);
    }
}
