using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int Height => rotated ? itemData.width : itemData.height;
    public int Width => rotated ? itemData.height : itemData.width;
    
    public int onGridPositionX;
    public int onGridPositionY;
    
    public bool rotated = false;

    private bool _selected = false;

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            InventoryHighlight.Instance.selector.gameObject.SetActive(_selected);
        }
    }
    internal void Rotate()
    {
        rotated = !rotated;
        
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0,0,rotated ? 90f : 0f);
    }

    internal void Set(int itemID)
    {
        itemData = ItemDB.Instance.GetItemByID(itemID);

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2(
            itemData.width  * ItemGrid.tileSizeWidth,
            itemData.height * ItemGrid.tileSizeHeight);
        onGridPositionX = (int)size.x;
        onGridPositionY = (int)size.y;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}