using UnityEngine;
using UnityEngine.UI;

public class LineActiveButton : MonoBehaviour
{
    [SerializeField] private int linePrice;
    [SerializeField] private Button activeButton;
    [SerializeField] private int currentLine;

    private void Start()
    {
        activeButton.onClick.AddListener(BuyLine);
    }

    private void BuyLine()
    {
        if (MainInventory.Instance.credit >= linePrice)
        {
            MainInventory.Instance.credit -= linePrice;
            GridSlotManager.Instance.BuyGridSlot(currentLine);
            // button binding
            activeButton.onClick.RemoveAllListeners();
            activeButton.onClick.AddListener(ActiveLine);
        }
    }

    private void ActiveLine()
    {
        GridSlotManager.Instance.ActiveGridSlot(currentLine);
    }
}
