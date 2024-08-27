using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryManager : MonoBehaviour
{
    [SerializeField] private List<Button> categories;
    
    [SerializeField] private List<GameObject> categoryObjects;
    
    private void Start()
    {
        int index = 0;
        foreach (var category in categories)
        {
            category.onClick.AddListener(() => SelectCategory(index++));
        }
    }

    private void SelectCategory(int index)
    {
        categoryObjects[index].SetActive(true);
    }
}
