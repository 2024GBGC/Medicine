using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    [SerializeField] private List<Button> categories;
    
    [SerializeField] private List<GameObject> categoryObjects;
    
    private int currentCategoryIndex = -1;

    private void Start()
    {
        foreach (Button button in categories)
        {
            button.onClick.AddListener(() =>
            {
                ToggleCategory(categories.IndexOf(button));
            });
        }
    }

    private void ToggleCategory(int categoryIndex)
    {
        foreach (var category in categoryObjects)
        {
            category.GetComponent<CanvasGroup>().alpha = 0;
        }
        if(currentCategoryIndex == categoryIndex)
        {
            currentCategoryIndex = -1;
            categoryObjects[categoryIndex].GetComponent<CanvasGroup>().alpha = 0;
            return;
        }

        categoryObjects[categoryIndex].GetComponent<CanvasGroup>().alpha = 1;
        currentCategoryIndex = categoryIndex;
        categoryObjects[categoryIndex].GetComponent<RectTransform>().SetAsLastSibling();
    }
    
}
