using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 64;  
    public const float tileSizeHeight = 64;

    InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform; 

    [SerializeField] int girdSizeWidth = 20;
    [SerializeField] int girdSizeHeight = 10;
    
    Vector2 positionOntheGrid = new Vector2();  //Inventory상의 기준점 설정
    Vector2Int tileGridePosition = new Vector2Int();

    [SerializeField] 
    private SerializableDictionary<int, int> _itemDict = new SerializableDictionary<int, int>();
    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); //UI의 기준점 위치
        Init(girdSizeWidth, girdSizeHeight);
    }

    //Set Inventory size
    public void Init(int width, int height)
    {
        girdSizeWidth = width;
        girdSizeHeight = height;
        inventoryItemSlot = new InventoryItem[width,height];
        Vector2 size = new Vector2(width * tileSizeWidth,height*tileSizeHeight);
        rectTransform.sizeDelta = size;
    }
    
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)    
    {
        positionOntheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOntheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridePosition.x = (int)(positionOntheGrid.x/tileSizeWidth);
        tileGridePosition.y = (int)(positionOntheGrid.y/tileSizeHeight);
        return tileGridePosition;
    }

    private bool CheckPlaceItem(InventoryItem inventoryItem,int posX,int posY)
    {
        // 인벤의 크기 외부에 있으면 위치 불가 
        if (BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height) == false)
        {
            return false;
        }

        return true;
    }
    
    public virtual bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        if(!CheckPlaceItem(inventoryItem,posX,posY))
        {
            return false;
        }
        
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
        AddValue(inventoryItem.itemData.itemID);

        return true;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.Width / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.Height / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height)
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(inventoryItemSlot[posX+x,posY+y]!= null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public virtual InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem pickUpItem = inventoryItemSlot[x, y];

        if (pickUpItem == null) return null; 
        CleanGridReference(pickUpItem);
        SubtractValue(pickUpItem.itemData.itemID);
        return pickUpItem;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.Width; ix++)
        {
            for (int iy = 0; iy < item.Height; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }

    bool PositionCheck(int posX,int posY)
    {
        if(posX < 0 || posY < 0)
            return false;
        
        if(posX >= girdSizeWidth || posY >= girdSizeHeight)
            return false;
        
        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width,int height)
    {
        if(!PositionCheck(posX,posY)) return false;
        posX += width-1;
        posY += height-1;
        if (!PositionCheck(posX,posY)) return false;       
        
        return true;
    }

    [CanBeNull]
    internal InventoryItem GetItem(int x, int y)
    {
        if (x < 0 || girdSizeWidth <= x) return null;
        if (y < 0 || girdSizeHeight <= y) return null;
        return inventoryItemSlot[x,y];
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = girdSizeHeight - itemToInsert.Height+1;
        int width = girdSizeWidth - itemToInsert.Width+1;
        
        for (int y=0; y< height; y++){
            for (int x = 0; x < width; x++)
            {
                 if(CheckAvailableSpace(x,y,itemToInsert.Width,itemToInsert.Height)==true){
                     return new Vector2Int(x,y);
                 }
            }
        }
        return null;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    public void AddValue(int key)
    {
        Debug.Log("AddValue KEY" + key);
        if (!_itemDict.TryAdd(key, 1))
        {
            _itemDict[key] += 1;
        }
    }
    
    public void SubtractValue(int key)
    {
        if (_itemDict.ContainsKey(key))
        {
            // 키가 있으면 값을 -1 감소
            _itemDict[key] -= 1;

            // 값이 0이 되면 딕셔너리에서 키를 제거
            if (_itemDict[key] == 0)
            {
                _itemDict.Remove(key);
            }
        }
        else
        {
            Debug.LogWarning($"키 '{key}'가 딕셔너리에 없습니다.");
        }
    }
    
    public bool AddItem(int itemID)
    {
        InventoryItem inventoryItem = Instantiate(ItemDB.Instance.itemPrefab).GetComponent<InventoryItem>();
        inventoryItem.Set(itemID);
        Vector2Int? posOnGrid = FindSpaceForObject(inventoryItem);
        if (posOnGrid != null)
        {
            PlaceItem(inventoryItem, posOnGrid.Value.x, posOnGrid.Value.y);
            return true;
        }
        inventoryItem.Rotate();
        posOnGrid = FindSpaceForObject(inventoryItem);
        if (posOnGrid != null)
        {
            PlaceItem(inventoryItem, posOnGrid.Value.x, posOnGrid.Value.y);
            return true;
        }
        return false;
    }
    
    public Dictionary<int,int> GetItemDict()
    {
        return _itemDict;
    }
}
