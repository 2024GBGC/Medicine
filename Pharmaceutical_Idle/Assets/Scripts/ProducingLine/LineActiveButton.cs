using UnityEngine;
using UnityEngine.UI;

public class LineActiveButton : MonoBehaviour
{
    [SerializeField] private int linePrice;
    [SerializeField] private Button activeButton;
    [SerializeField] private int currentLine;

    private void Start()
    {
        activeButton.onClick.AddListener(ActiveLine);
    }

    private void ActiveLine()
    {
        if(PotSlotManager.Instance.pots[currentLine].isActive)
            PotSlotManager.Instance.ActiveGridSlot(currentLine);
    }
}
