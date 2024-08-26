using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemGrid : ItemGrid
{
    public bool isItemOnGrid = false;

    public override bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        if (isItemOnGrid) return false;
        
        if (base.PlaceItem(inventoryItem, posX, posY))
        {
            isItemOnGrid = true;
            return true;
        }
        return false;
    }

    public override InventoryItem PickUpItem(int x, int y)
    {
        isItemOnGrid = false;
        return base.PickUpItem(x, y);
    }
}
