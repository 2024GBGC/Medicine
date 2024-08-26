using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSlotManager : Singleton<GridSlotManager>
{
    [SerializeField] private List<GameObject> gridSlot;
    
    public void ActiveGridSlot(int activeIndex)
    {
        foreach (GameObject slot in gridSlot)
        {
            slot.SetActive(false);
        }
        gridSlot[activeIndex].SetActive(true);
    }
}
