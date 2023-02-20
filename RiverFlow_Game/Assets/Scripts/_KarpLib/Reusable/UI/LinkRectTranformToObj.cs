using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Threading.Tasks;

public class LinkRectTranformToObj : MonoBehaviour
{
    private RectTransform self;
    [Space(10)]
    [SerializeField] GameObject target;
    [SerializeField, ReadOnly] Vector2 targetPos;
    [SerializeField] Vector2 offSet;

    void Start()
    {
        self = (RectTransform) this.transform;
    }

    private void Update()
    {
        UpdatePosToObject();
    }
    private void UpdatePosToObject()
    {
        targetPos = Utilities_UI.GetScreenPosOfGameObject(target.transform.position);
        self.anchoredPosition = targetPos + offSet;

        bool inCorner = Mathf.Abs(self.anchoredPosition.x - Screen.currentResolution.width) < offSet.x + self.sizeDelta.x
                     || Mathf.Abs(self.anchoredPosition.y - Screen.currentResolution.height) < offSet.y + self.sizeDelta.y;
        if (inCorner)
        {
            self.anchoredPosition = targetPos - offSet - self.sizeDelta;
        }

        self.anchoredPosition3D = new Vector3(self.anchoredPosition.x, self.anchoredPosition.y, 0f);
    }
}
