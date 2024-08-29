using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemData : ScriptableObject
{
    public int itemID;
    public string itemName;

    public int itemCost;
    public int itemPrice;
    
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;

    public Color color;
}