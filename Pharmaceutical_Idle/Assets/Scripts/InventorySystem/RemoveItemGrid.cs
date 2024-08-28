using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveItemGrid : ItemGrid
{
    public override bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, bool isUpgrade = false)
    {
        if (base.PlaceItem(inventoryItem, posX, posY, isUpgrade))
        {
            Destroy(inventoryItem.gameObject);
            GetItemDict().Clear();
            CleanGridReference(inventoryItem);
            return true;
        }

        return false;
    }
}
