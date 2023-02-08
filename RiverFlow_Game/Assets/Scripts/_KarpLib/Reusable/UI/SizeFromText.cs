using TMPro;
using UnityEngine;
using NaughtyAttributes;

public class SizeFromText : MonoBehaviour
{
    public RectTransform self;
    public TextMeshProUGUI textSlot;

    [Button]
    private void OnEnable()
    {
        self.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textSlot.GetRenderedValues(true).x + 30);
    }
}
