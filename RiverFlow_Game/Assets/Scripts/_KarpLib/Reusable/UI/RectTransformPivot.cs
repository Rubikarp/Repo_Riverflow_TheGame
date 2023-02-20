using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class RectTransformPivot : MonoBehaviour
{
    public RectTransform self;
    public Image image;

    [Button]
    private void OnEnable()
    {
        self.pivot = new Vector2(image.sprite.pivot.x / 256, image.sprite.pivot.y / 256);
    }
}
