using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private InventoryHighlight _inventoryHighlight;
    private Vector2Int _oldPosition;
    private InventoryItem _itemToHighlight;
    
    private InventoryItem _overlapItem;

    [SerializeField] private Transform canvasTransform;
    
    [SerializeField] private ItemGrid _selectedItemGrid;
    public ItemGrid SelectedItemGrid 
    {
        get => _selectedItemGrid;
        set 
        {
            if (_selectedItemGrid != value)
            {
                _selectedItemGrid = value; 
                _inventoryHighlight.SetParent(value);
            }
        }
    }

    private InventoryItem _selectedItem;

    public event Action<InventoryItem> OnSelectedItemChanged;
    public InventoryItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            if(_selectedItem)
                _selectedItem.Selected = false;
            _selectedItem = value;
            if (_selectedItem)
            {
                _selectedItem.Selected = true;
                _inventoryHighlight.SetPosition(_selectedItemGrid, _itemToHighlight);
                
                OnSelectedItemChanged?.Invoke(_selectedItem);
            }
        }
    }
    
    
    private void Awake() {
        _inventoryHighlight = GetComponent<InventoryHighlight>();
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            RotateItem();
        }

        if (_selectedItemGrid == null) {
            _inventoryHighlight.Show(false);
            return; 
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }

    }

    private void RotateItem()
    {
        if(SelectedItem == null) return;
        SelectedItem.Rotate();
    }
    
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if(_oldPosition == positionOnGrid) return;
        _oldPosition = positionOnGrid;
        if (SelectedItem == null)
        {
            _itemToHighlight = _selectedItemGrid.GetItem(positionOnGrid.x,positionOnGrid.y);
            // 현재 선택된 아이템이 없는데 내가 있는 마우스 포지션에 선택할 수 있는 아이템이 있음 
            if(_itemToHighlight!= null) 
            {
                _inventoryHighlight.Show(true);
                _inventoryHighlight.SetSize(_itemToHighlight);
                _inventoryHighlight.SetPosition(_selectedItemGrid, _itemToHighlight);
                _inventoryHighlight.SetSelectorParent(_selectedItemGrid);
            }
            // 현재 선택된 아이템이 없는데 내가 있는 마우스 포지션이 빈칸임 
            else
            {
                _inventoryHighlight.Show(false);
            }
        }
        else
        { 
            //물건을 잡았을 때 인벤토리에 넣을 수 없으면 highlighter을 숨긴다.
            _inventoryHighlight.Show(_selectedItemGrid.BoundaryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                SelectedItem.Width,
                SelectedItem.Height
            ));
            _inventoryHighlight.SetSize(SelectedItem);
            _inventoryHighlight.SetPosition(_selectedItemGrid, SelectedItem,positionOnGrid.x,positionOnGrid.y);
        }
    }
    
    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (SelectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (SelectedItem != null)
        {
            position.x -= (SelectedItem.Width - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (SelectedItem.Height - 1) * ItemGrid.tileSizeHeight / 2;
        }
        
        return _selectedItemGrid.GetTileGridPosition(position);
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        SelectedItem = _selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
    }
    private void PlaceItem(Vector2Int tileGridPosition)
    {
        if(_selectedItemGrid.PlaceItem(SelectedItem, tileGridPosition.x, tileGridPosition.y))
            SelectedItem = null;
    }

    public void SelectItemByButton(int itemID)
    {
        SelectedItem = Instantiate(ItemDB.Instance.itemPrefab, canvasTransform).GetComponent<InventoryItem>();
        SelectedItem.Set(itemID);
    }
}