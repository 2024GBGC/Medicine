using UnityEngine;
using UnityEngine.UI;

public class LineActiveButton : MonoBehaviour
{
    [SerializeField] private int linePrice;
    [SerializeField] private Button activeButton;
    [SerializeField] private int currentLine;
    [SerializeField] private float productionSpeed = 1;
    private float _timer;

    private void Start()
    {
        activeButton.onClick.AddListener(BuyLine);
        _timer = 0;
    }

    private void BuyLine()
    {
        if (MainInventory.Instance.credit >= linePrice)
        {
            MainInventory.Instance.credit -= linePrice;
            ActiveLine();
            activeButton.onClick.RemoveAllListeners();
            activeButton.onClick.AddListener(ActiveLine);
        }
    }

    private void ActiveLine()
    {
        GridSlotManager.Instance.ActiveGridSlot(currentLine);
    }

    private void OperateProductionLine(int potionPrice)
    {
        _timer += Time.deltaTime;

        if (_timer >= (5 / productionSpeed))
        {
            _timer = 0;
            MainInventory.Instance.IncreaseCredit(potionPrice);
        }
    }

    public void IncreaseProductionSpeed()
    {
        productionSpeed += 0.5f;
    }
}
