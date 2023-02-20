using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesHeader : MonoBehaviour
{
    public Image[] catImages;
    public TextMeshProUGUI[] catText;
    [Space(10)]
    public GameObject[] categories;
    [Space(10)]
    public Color selectedImageColor = Color.white;
    public Color deselectedImageColor = Color.grey;
    [Space(5)]
    public Color selectedTextColor = Color.black;
    public Color deselectedTextColor = Color.white;

    public void ChooseWindow(int index)
    {
        if(index >= catImages.Length || index >= catText.Length)
        {
            Debug.LogError("index out of category array", this);
            return;
        }

        foreach (var img in catImages)
        {
            img.color = deselectedImageColor;
        }
        foreach (var text in catText)
        {
            text.color = deselectedTextColor;
        }
        foreach (var obj in categories)
        {
            obj.SetActive(false);
        }

        catText[index].color = selectedTextColor;
        catImages[index].color = selectedImageColor;
        categories[index].SetActive(true);
    }

    private void OnEnable()
    {
        ChooseWindow(0);
    }
}
