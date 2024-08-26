using UnityEngine;

public class InventoryHighlight : Singleton<InventoryHighlight>
{
    [SerializeField] RectTransform highlighter;
    public RectTransform selector;
    public void Show(bool isShow)
    {
        highlighter.gameObject.SetActive(isShow);
    }

    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.Width * ItemGrid.tileSizeWidth;
        size.y = targetItem.Height * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;
        selector.sizeDelta = size;
    }

    //아이템의 크기와 위치를 받아서 highlighter의 위치를 정한다.
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        if(targetItem == null){
            Debug.Log("Something went wrong");
            return;
        }
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY
        );

        highlighter.localPosition = pos;
        selector.localPosition = pos;
        
        highlighter.SetAsLastSibling();
        selector.SetAsLastSibling();
    }

    public void SetParent(ItemGrid targetGrid)
    {
        if(targetGrid == null){
            return;
        }
        
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
        highlighter.SetAsLastSibling();
    }

    public void SetSelectorParent(ItemGrid targetGrid)
    {
        if(targetGrid == null){
            return;
        }
        
        selector.SetParent(targetGrid.GetComponent<RectTransform>());
        selector.SetAsLastSibling();
    }

    // 마우스 위치에 따라 하이라이터 위치 변경 
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY){
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            posX,
            posY
        );

        highlighter.localPosition = pos;
        highlighter.SetAsLastSibling();
    }
}